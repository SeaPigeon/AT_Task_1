using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum AgentState
{
    Inactive,
    Idle,
    Moving,
    Attacking,
    Building,
    Gathering,
    Interacting,
    Selected,
    Knight
}
public enum AgentClass
{
    Villager,
    Knight
}
public class AgentScript : MonoBehaviour
{
    [Header("Agent Variables")]
    [SerializeField] GameObject _villagerMesh;
    [SerializeField] GameObject _knightMesh;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _rotationSpeed;
    [SerializeField] int _idleWalkRange = 10;
    [SerializeField] float _timeBetweenWalks = 3;
    [SerializeField] int _MAX_FOOD_CARRIED = 5;
    [SerializeField] int _MAX_ROCK_CARRIED = 2;
    [SerializeField] int _MAX_WOOD_CARRIED = 4;
    [SerializeField] int _currentHealth;
    [SerializeField] int _MAX_HEALTH = 100;

    [Header("Debug")]
    [SerializeField] private GameObject _stateIndicator;
    [SerializeField] private AgentState _agentState;
    [SerializeField] private AgentClass _agentClass;
    [SerializeField] private int _carriedFood;
    [SerializeField] private int _carriedRock;
    [SerializeField] private int _carriedWood;
    [SerializeField] private bool _movingTowardsInteractable;
    [SerializeField] private Slider _interactionSlider;
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Canvas _agentCanvas;
    private LinkUIScript _UILinker;
    private Vector3 _targetPos;
    private Vector3 _targetRotation;
    private NavMeshAgent _navMeshAgent;
    private GameManagerScript _gameManager;
    private Vector3 _patrolPoint;
    private bool _isPatrolling;
    private float patrolTimer = 0f;
    private bool _isWaiting = false;
    private float _waitTimer;
    private bool _hasMoveTarget;
    private MeshRenderer _stateIndicatorMeshRenderer;
    private ResourceBase _resourceToInteractWith;
    private BuildingBase _buildingToInteractWith;
    private EnemyScript _enemyToAttack;
    private Coroutine _activeCor;
    private bool _hasIdleStarted = false;
    // G&S
    public int MaxFoodCarriable { get { return _MAX_FOOD_CARRIED; } set { _MAX_FOOD_CARRIED = value; } }
    public int MaxRockCarriable { get { return _MAX_ROCK_CARRIED; } set { _MAX_ROCK_CARRIED = value; } }
    public int MaxWoodCarriable { get { return _MAX_WOOD_CARRIED; } set { _MAX_WOOD_CARRIED = value; } }
    public int CarriedFood { get { return _carriedFood; } set { _carriedFood = value; } }
    public int CarriedRocks { get { return _carriedRock; } set { _carriedRock = value; } }
    public int CarriedWood { get { return _carriedWood; } set { _carriedWood = value; } }
    public bool MovingTowardsInteractable { get { return _movingTowardsInteractable; } set { _movingTowardsInteractable = value; } }
    public Vector3 MoveTargetPosition { get { return _targetPos; } set { _targetPos = value; } }
    public ResourceBase ResourceToInteractWith { get { return _resourceToInteractWith; } set { _resourceToInteractWith = value; } }
    public BuildingBase BuildingToInteractWith { get { return _buildingToInteractWith; } set { _buildingToInteractWith = value; } }
    public EnemyScript EnemyToAttack { get { return _enemyToAttack; } set { _enemyToAttack = value; } }
    public AgentState ActiveAgentState { get { return _agentState; } set { _agentState = value; } }
    public NavMeshAgent NavMeshAgent { get { return _navMeshAgent; } }
    public Coroutine ActiveCoR { get { return _activeCor; } set { _activeCor = value; } }
    public Slider AgentSlider { get { return _interactionSlider; } }
    public AgentClass AgentClass { get { return _agentClass; } set { _agentClass = value; } }
    public GameObject VillagerMesh { get { return _villagerMesh; } }
    public GameObject KnightMesh { get { return _knightMesh; } }
    public bool HasMoveTarget { get { return _hasMoveTarget; } set { _hasMoveTarget = value; } }
    public int CurrentHealth { get { return _currentHealth; }}
    private void Start() 
    {
        SetUpReferences();
        SetUpAgent();
    }
    private void Update()
    {
        SetMesh();

        switch (_agentState)
        {
            case AgentState.Inactive:
                ChangeColor(Color.gray);
                if (_activeCor == null)
                {
                    _activeCor = StartCoroutine(StartIdle());
                }
                break;

            case AgentState.Idle:
                ChangeColor(Color.black);
                Idle();
                break;

            case AgentState.Moving:
                ChangeColor(Color.yellow);
                Move(_targetPos);
                if (!_navMeshAgent.pathPending && 
                    _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    StopAgent();
                    if (_movingTowardsInteractable)
                    {
                        if (_resourceToInteractWith != null)
                        {
                            _resourceToInteractWith.StartGathering(this);
                        }
                        else if (_buildingToInteractWith != null)
                        {
                            _buildingToInteractWith.StartInteract(this);
                        }
                        else if (_enemyToAttack != null)
                        {
                            //_enemyToAttack.StartToAttack(this);
                        }
                        
                        _movingTowardsInteractable = false;
                    }
                }
                break;

            case AgentState.Attacking:
                ChangeColor(Color.red);
                break;

            case AgentState.Building:
                ChangeColor(Color.blue);
                break;
            case AgentState.Interacting:
                ChangeColor(Color.magenta);
                break;
            case AgentState.Gathering:
                ChangeColor(Color.cyan);
                break;

            case AgentState.Selected:
                ChangeColor(Color.green);
                break;

            default:
                ChangeColor(Color.white);
                Debug.Log("Agent State ERROR");
                break;
        }
    }

    // Essentials
    private void SetUpReferences()
    {
        _gameManager = GameManagerScript.GMInstance;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _stateIndicatorMeshRenderer = GetComponentInChildren<MeshRenderer>();
        _agentCanvas = _interactionSlider.GetComponentInParent<Canvas>();
        _UILinker = UIManagerScript.UIMInstance.GetComponent<LinkUIScript>();
    }
    private void SetUpAgent() 
    {
        _targetPos = transform.position;
        _targetRotation = Vector3.zero;
        _agentState = AgentState.Inactive;
        _carriedRock = 0;
        _carriedWood = 0;
        _gameManager.AgentsInGame.Add(this);
        _hasMoveTarget = false;
        _movingTowardsInteractable = false;
        _buildingToInteractWith = null;
        _resourceToInteractWith = null;
        _enemyToAttack = null;
        _agentClass = AgentClass.Villager;
        _villagerMesh.SetActive(true);
        _knightMesh.SetActive(false);
        _agentCanvas.gameObject.SetActive(true);
        _interactionSlider.gameObject.SetActive(false);
        _healthSlider.gameObject.SetActive(false);
        _currentHealth = _MAX_HEALTH;
    }
    public void ResetAgent()
    {
        NavMeshAgent.isStopped = true;
        MoveTargetPosition = Vector3.zero;
        MovingTowardsInteractable = false;
        ResourceToInteractWith = null;
        BuildingToInteractWith = null;
        EnemyToAttack = null;
        StopAgentCoroutine(ActiveCoR);
        ActiveCoR = null;
        ActiveAgentState = AgentState.Inactive;
    }
    
    // Movement
    private void Move(Vector3 targetPos)
    {
        if (!_hasMoveTarget)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(targetPos);
            _hasMoveTarget = true;
        }
    }
    public void StopAgent()
    {
        _agentState = AgentState.Selected;
        _navMeshAgent.isStopped = true;
        _hasMoveTarget = false;
        _isPatrolling = false;
    }

    // Idle
    private IEnumerator StartIdle()
    {
        yield return new WaitForSeconds(_timeBetweenWalks);
        _agentState = AgentState.Idle;
    }
    private void Idle()
    {
        if (!_isPatrolling)
        {
            _patrolPoint = GenerateRandomPoint(transform.position, _idleWalkRange);
            while (Vector3.Distance(_patrolPoint, transform.position) < 0.5f)
            {
                _patrolPoint = GenerateRandomPoint(transform.position, _idleWalkRange);
            }
            _navMeshAgent.isStopped = false;
            _isPatrolling = true;
            _navMeshAgent.SetDestination(_patrolPoint);
        }

        if (!_navMeshAgent.pathPending &&
            _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            StopIdle();
        }
    }
    public void StopIdle()
    {
        _navMeshAgent.isStopped = true;
        _patrolPoint = Vector3.zero;
        _agentState = AgentState.Inactive;
        _isPatrolling = false;
        StopAgentCoroutine(_activeCor);
        _activeCor = null;
    }
    private Vector3 GenerateRandomPoint(Vector3 agentPos, int range)
    {
        Vector3 randomPoint = agentPos + Random.insideUnitSphere * range;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas);
        return hit.position;
    }

    // General
    public void ChangeColor(Color color)
    {
        if (_stateIndicatorMeshRenderer.material.color != color)
        {
            _stateIndicatorMeshRenderer.material.color = color;
        }
    }
    public void StopAgentCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
    public void SetMesh()
    {
        if (_agentClass == AgentClass.Villager)
        {
            _villagerMesh.SetActive(true);
            _knightMesh.SetActive(false);
        }
        else
        {
            _villagerMesh.SetActive(false);
            _knightMesh.SetActive(true);
        }
    }
    private void Death()
    {
        //_gameManager.AgentsInGame.Remove(this);
        Destroy(gameObject);
    }

    // UI
    public void EnableAgentUI(float time)
    {
        if (_agentState == AgentState.Gathering ||
            _agentState == AgentState.Interacting ||
            _agentState == AgentState.Building)
        {
            _interactionSlider.gameObject.SetActive(true);
            _healthSlider.gameObject.SetActive(false);
            StartCoroutine(FillBar(time));
        }
        else if (_agentState == AgentState.Attacking)
        {
            _interactionSlider.gameObject.SetActive(false);
            _healthSlider.gameObject.SetActive(true);
        }
        else
        {
            _interactionSlider.gameObject.SetActive(false);
            _healthSlider.gameObject.SetActive(false);
        }
        
    }
    private IEnumerator FillBar(float maxTime)
    {
        var startTime = 0f;
        _interactionSlider.value = 0f;
        
        while (startTime < maxTime)
        {
            startTime += Time.deltaTime;
            float sliderValue = startTime / maxTime;
            _interactionSlider.value = sliderValue;
            yield return null;
        }

        _agentCanvas.enabled = false;
    }

    // Combat
    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
    }
}
