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
    [SerializeField] int _MAX_ROCK_CARRIED = 2;
    [SerializeField] int _MAX_WOOD_CARRIED = 4;

    [Header("Debug")]
    [SerializeField] private GameObject _stateIndicator;
    [SerializeField] private AgentState _agentState;
    [SerializeField] private AgentClass _agentClass;
    [SerializeField] private int _carriedRock;
    [SerializeField] private int _carriedWood;
    [SerializeField] private bool _movingTowardsInteractable;
    [SerializeField] private Slider _slider;
    [SerializeField] private Canvas _sliderCanvas;
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

    // G&S
    public int MaxRockCarriable { get { return _MAX_ROCK_CARRIED; } set { _MAX_ROCK_CARRIED = value; } }
    public int MaxWoodCarriable { get { return _MAX_WOOD_CARRIED; } set { _MAX_WOOD_CARRIED = value; } }
    public int CarriedRock { get { return _carriedRock; } set { _carriedRock = value; } }
    public int CarriedWood { get { return _carriedWood; } set { _carriedWood = value; } }
    public bool MovingTowardsInteractable { get { return _movingTowardsInteractable; } set { _movingTowardsInteractable = value; } }
    public Vector3 MoveTargetPosition { get { return _targetPos; } set { _targetPos = value; } }
    public AgentState ActiveAgentState { get { return _agentState; } set { _agentState = value; } }
    public NavMeshAgent NavMeshAgent { get { return _navMeshAgent; } }
    public ResourceBase ResourceToInteractWith { get { return _resourceToInteractWith; } set { _resourceToInteractWith = value; } }
    public Coroutine ActiveCoR { get { return _activeCor; } set { _activeCor = value; } }
    public BuildingBase BuildingToInteractWith { get { return _buildingToInteractWith; } set { _buildingToInteractWith = value; } }
    public EnemyScript EnemyToAttack { get { return _enemyToAttack; } set { _enemyToAttack = value; } }
    public Slider AgentSlider { get { return _slider; } }
    public AgentClass AgentClass { get { return _agentClass; } set { _agentClass = value; } }
    public GameObject VillagerMesh { get { return _villagerMesh; } }
    public GameObject KnightMesh { get { return _knightMesh; } }

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
                //StartIdle();
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
        _slider = GetComponentInChildren<Slider>();
        _agentClass = AgentClass.Villager;
        _villagerMesh.SetActive(true);
        _knightMesh.SetActive(false);
        _sliderCanvas = _slider.GetComponentInParent<Canvas>();
        _sliderCanvas.enabled = false;

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
        StopCoroutine(StopIdle());
        _navMeshAgent.isStopped = true;
        _hasMoveTarget = false;
        _isPatrolling = false;
    }

    // Idle
    private void StartIdle()
    {
        if (!_isWaiting)
        {
            _isWaiting = true;
            _waitTimer = _timeBetweenWalks;
        }

        _waitTimer -= Time.deltaTime;
        if (_waitTimer <= 0f)
        {
            _isWaiting = false;
            _agentState = AgentState.Idle;
            Idle();
        }
    }
    private void Idle()
    {
        if (!_isPatrolling)
        {
            _isPatrolling = true;
            _patrolPoint = GenerateRandomPoint(transform.position, _idleWalkRange);
            if (Vector3.Distance(transform.position, _patrolPoint) <= _navMeshAgent.stoppingDistance)
            {
                _patrolPoint = GenerateRandomPoint(transform.position, _idleWalkRange);
            }
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_patrolPoint);

            StartCoroutine(StopIdle());
        }
    }
    private IEnumerator StopIdle()
    {
        yield return new WaitUntil(() => Vector3.Distance(transform.position, _patrolPoint) <= _navMeshAgent.stoppingDistance);
        _navMeshAgent.isStopped = true;
        _isPatrolling = false;
        _agentState = AgentState.Inactive;
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
            Debug.Log("Agent Call");
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

    // UI
    public void EnableAgentUI(float time)
    {
        if (_agentState == AgentState.Gathering ||
            _agentState == AgentState.Interacting ||
            _agentState == AgentState.Attacking ||
            _agentState == AgentState.Building)
        {
            _sliderCanvas.enabled = true;
            FillBar(time);
        }
        else
        {
            _sliderCanvas.enabled = false;
        }
        
    }
    private IEnumerator FillBar(float time)
    {
        var startTime = Time.time;
        while (Time.time < startTime + time)
        {
            _slider.value += Time.deltaTime;
        }
        yield break;
    }
    // Actions
    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0);
    } // moveto enemy
    private IEnumerator Death()
    {
        Debug.Log("call1");
        yield return new WaitForSeconds(25);
        _gameManager.AgentsInGame.Remove(gameObject.GetComponent<AgentScript>());
        Destroy(gameObject);
    }


    // AGGIUNGERE CHANGE MESH FUNCTION KNIGHT/FARMER
}
