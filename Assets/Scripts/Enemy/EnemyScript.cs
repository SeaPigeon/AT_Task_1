using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.UI;

public enum EnemyType
{
    Ghost
}
public enum EnemyState
{
    Inactive,
    Chase,
    Combat,
    Dead
}

public class EnemyScript : MonoBehaviour
{
    [Header("Enemy Type")]
    [SerializeField] EnemyType _enemyType;
    [SerializeField] EnemyState _currentState;

    [Header("Ghost")]
    [SerializeField] int _MAX_HEALTH = 40;
    [SerializeField] int _pointValue;
    [SerializeField] int _damage;
    [SerializeField] float _attackDelay;

    [Header("Debug")]
    [SerializeField] GameObject _fightPoint;
    [SerializeField] GameManagerScript _gameManager;
    [SerializeField] AudioManagerScript _audioManager;
    [SerializeField] LinkUIScript _UILinker;
    [SerializeField] int _currentHealth;

    [SerializeField] float _distanceToPlayer;
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] GameObject _mesh;
    [SerializeField] bool _hasTarget;
    [SerializeField] private Slider _healthSlider;
    private AgentScript _closestAgent;
    private Coroutine _activeCor;
    [SerializeField] private bool _isChasing;
    [SerializeField] private bool _inCombat;

    // G&S
    public EnemyState CurrentEnemyState { get { return _currentState; } set { _currentState = value; } }
    public GameObject FightPoint { get { return _fightPoint; } set { _fightPoint = value; } }
    public Coroutine ActiveCoR { get { return _activeCor; } set { _activeCor = value; } }
    public bool InCombat { get { return _inCombat; } set { _inCombat = value; } }

    void Start()
    {
        SetUpReferences();
        SetUpEnemy();
        SpawnEnemy();
    }

    void Update()
    {
        //Behaviour();
    }

    // Essentials
    private void SetUpReferences()
    {
        _gameManager = GameManagerScript.GMInstance;
        _audioManager = AudioManagerScript.AMInstance;
        _UILinker = UIManagerScript.UIMInstance.GetComponent<LinkUIScript>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _mesh = gameObject.transform.GetChild(0).gameObject;
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void SetUpEnemy()
    {
        _gameManager.EnemiesInGame.Add(this);
        _currentHealth = _MAX_HEALTH;
        _isChasing = false;
        _closestAgent = null;
        _inCombat = false;
    }

    // Chase
    private void StartChase()
    {
        _currentState = EnemyState.Chase;
    }
    private void Chase()
    {
        _closestAgent = FindClosestAgent();
        if (!_isChasing)
        {
            _navMeshAgent.isStopped = false;
            _isChasing = true;
            _navMeshAgent.SetDestination(_closestAgent.transform.position);
        }

        if (!_navMeshAgent.pathPending &&
            _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            StopChasing();
            if (Vector3.Distance(transform.position, _closestAgent.transform.position) < 2)
            {
                _closestAgent.StopIdle();
                _currentState = EnemyState.Combat;
                _closestAgent.ActiveAgentState = AgentState.Combat;
                _inCombat = true;
                _closestAgent.InCombat = true;
                if (PlayerScript.PlayerInstance.ActiveAgentsList.Contains(_closestAgent))
                {
                    PlayerScript.PlayerInstance.ActiveAgentsList.Remove(_closestAgent);
                    PlayerScript.PlayerInstance.HasAgentsInSelection();
                }
            }
        }
    }
    private void StopChasing()
    {
        _navMeshAgent.isStopped = true;
        _currentState = EnemyState.Inactive;
        _isChasing = false;
        StopEnemyCoroutine(_activeCor);
        _activeCor = null;
    }
    private AgentScript FindClosestAgent()
    {
        float distance = 10000;
        AgentScript targetAgent = null;
        
        foreach (var agent in _gameManager.AgentsInGame)
        {
            float distanceToAgent = 0;
            distanceToAgent = Vector3.Distance(transform.position, agent.transform.position);
            if (distanceToAgent < distance)
            {
                distance = distanceToAgent;
                targetAgent = agent;
            }
        }
        return targetAgent;
    }
    
    // Combat
    private void EngageCombat()
    {
        if (Vector3.Distance(transform.position, _closestAgent.AgentCombatPoint.transform.position) < _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_closestAgent.AgentCombatPoint.transform.position);
        }
        if (!_navMeshAgent.pathPending &&
            _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance &&
            !_inCombat)
        {
            Debug.Log("call");
            StopEnemyCoroutine(_activeCor);
            _activeCor = null;
            _activeCor = StartCoroutine(Attack());

            _closestAgent.EnemyToAttack = this;
            _closestAgent.StopAgentCoroutine(_closestAgent.ActiveCoR);
            _closestAgent.ActiveCoR = null;
            _closestAgent.ActiveCoR = StartCoroutine(_closestAgent.Attack());
        }
    }
    private IEnumerator Attack()
    {
        Debug.Log("Started");
        while (_inCombat)
        {
            _closestAgent.TakeDamage(_damage);
            yield return new WaitForSeconds(_attackDelay);
        }
        
    } 
    public void TakeDamage(int dmg)
    {
        _currentHealth -= dmg;
        //handle health bar

        if (_currentHealth <= 0)
        {
            _navMeshAgent.isStopped = true;
            _gameManager.ChangeScore(_pointValue);
            _UILinker.ScoreTextUI.text = _gameManager.Score.ToString();
            _currentState = EnemyState.Dead;
            _gameManager.EnemiesInGame.Remove(this);
            _closestAgent.ActiveAgentState = AgentState.Inactive;
            _closestAgent.InCombat = false;
            _closestAgent.StopAgentCoroutine(_closestAgent.ActiveCoR);
            _closestAgent.ActiveCoR = null;
            Destroy(gameObject);

        }
    }

    // Spawn
    private void SpawnEnemy()
    {
        _mesh.SetActive(false);
        transform.position = GenerateRandomPoint(transform.position, 40);
        _mesh.SetActive(true);
        _currentState = EnemyState.Inactive;
    }
    private Vector3 GenerateRandomPoint(Vector3 enemyPos, int range)
    {
        Vector3 randomPoint = enemyPos + Random.insideUnitSphere * range;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas);
        return hit.position;
    }

    // Collisions
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Enemy Collision: " + other.name);
    }

    // Behaviour
    private void Behaviour()
    {
        switch (_currentState)
        {
            case EnemyState.Inactive:
                StartChase();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Combat:
                EngageCombat();
                break;
            case EnemyState.Dead:
                break;
            default:
                break;
        }

    }

    // General
    public void StopEnemyCoroutine(Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }
}
