using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AgentState
{
    Inactive,
    Idle,
    Moving,
    Attacking,
    Building,
    Gathering,
    Selected
}
public class AgentScript : MonoBehaviour
{
    [Header("Agent Variables")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _rotationSpeed;
    [SerializeField] int _idleWalkRange = 10;
    [SerializeField] float _timeBetweenWalks = 3;
    [SerializeField] int _MAX_CLOTH_CARRIED = 6;
    [SerializeField] int _MAX_IRON_CARRIED = 2;
    [SerializeField] int _MAX_WOOD_CARRIED = 4;

    [Header("Debug")]
    [SerializeField] AgentState _agentState;
    [SerializeField] private Vector3 _targetPos;
    [SerializeField] private Vector3 _targetRotation;
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] private int _carriedCloth;
    [SerializeField] private int _carriedIron;
    [SerializeField] private int _carriedWood;
    private ResourceType _resType;
    private GameManagerScript _gameManager;
    [SerializeField] Vector3 _patrolPoint;
    [SerializeField] bool _isPatrolling;
    private float patrolTimer = 0f;
    private bool _isWaiting = false;
    private float _waitTimer;
    private bool _hasMoveTarget;

    private void Start() 
    {
        SetUpReferences();
        SetUpAgent();
    }
    private void Update()
    {
        switch (_agentState)
        {
            case AgentState.Inactive:
                //StartIdle();
                break;
            case AgentState.Idle:
                Idle();
                break;
            case AgentState.Moving:
                Move(_targetPos);
                if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
                {
                    StopAgent();
                }
                
                break;
            case AgentState.Attacking:
                break;
            case AgentState.Building:
                break;
            case AgentState.Gathering:
                break;
            case AgentState.Selected:
                break;
            default:
                Debug.Log("Agent State ERROR");
                break;
        }
        //Rotate(_targetPos);
    }

    // G&S
    public Vector3 MoveTargetPosition { get { return _targetPos; } set { _targetPos = value; } }
    public AgentState AgentState { get { return _agentState; } set { _agentState = value; } }
    public NavMeshAgent NavMeshAgent { get { return _navMeshAgent; } }

    // Methods
    private void SetUpReferences()
    {
        _gameManager = GameManagerScript.GMInstance;
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void SetUpAgent() 
    {
        _targetPos = transform.position;
        _targetRotation = Vector3.zero;
        _agentState = AgentState.Inactive;
        _carriedCloth = 0;
        _carriedIron = 0;
        _carriedWood = 0;
        _gameManager.AgentsInGame.Add(this);
        _hasMoveTarget = false;
        //StartCoroutine(Death());
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
        /*if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _agentState = AgentState.Selected;
            _navMeshAgent.isStopped = true;
            //_targetPos = transform.position;

        }*/
        _agentState = AgentState.Selected;
        StopCoroutine(StopIdle());
        _navMeshAgent.isStopped = true;
        _hasMoveTarget = false;
        _isPatrolling = false;
    }
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

    // Actions
    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0);
    } // moveto enemy
    private IEnumerator Build()
    {
        yield return new WaitForSeconds(0);
    } //move to buildings
    private IEnumerator Death()
    {
        Debug.Log("call1");
        yield return new WaitForSeconds(25);
        _gameManager.AgentsInGame.Remove(gameObject.GetComponent<AgentScript>());
        Destroy(gameObject);
    }
}
