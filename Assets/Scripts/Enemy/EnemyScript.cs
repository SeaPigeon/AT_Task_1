using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;



public enum EnemyType
{
    Default
}
public enum EnemyState
{
    Inactive,
    Idle,
    Chase,
    Shoot,
    Dead
}

public class EnemyScript : MonoBehaviour
{
    [Header("Enemy Type")]
    [SerializeField] EnemyType _enemyType;
    [SerializeField] EnemyState _currentState;
    [SerializeField] float _ACTIVATION_RANGE = 50;
    [Header("BaseEnemy")]
    [SerializeField] int _MAX_HEALTH_BASE = 40;
    [SerializeField] int _POINT_VALUE_BASE = 100;

    [Header("Debug")]
    [SerializeField] GameManagerScript _gameManager;
    [SerializeField] SceneManagerScript _sceneManager;
    [SerializeField] AudioManagerScript _audioManager;
    [SerializeField] LinkUIScript _UILinker;
    [SerializeField] PlayerScript _player;
    [SerializeField] int _currentHealth;
    [SerializeField] int _pointValue;
    [SerializeField] float _distanceToPlayer;
    [SerializeField] NavMeshAgent _navMeshAgent;
    [SerializeField] bool _isPatrolling;
    [SerializeField] Vector3 _patrolPoint;

    void Start()
    {
        SetUpReferences();
        SetUpEnemy();
    }

    void Update()
    {
        Behaviour();
    }

    // G&S
    public EnemyState CurrentEnemyState { get { return _currentState; } }
    private void SetUpReferences()
    {
        _gameManager = GameManagerScript.GMInstance;
        _sceneManager = SceneManagerScript.SMInstance;
        _audioManager = AudioManagerScript.AMInstance;
        _UILinker = UIManagerScript.UIMInstance.GetComponent<LinkUIScript>();
        _player = PlayerScript.PlayerInstance;
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    private void SetUpEnemy()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _isPatrolling = false;
        _currentState = EnemyState.Idle;

        switch (_enemyType)
        {
            // SetUp enemy type variables
        }
        if (PlayerInRange(_ACTIVATION_RANGE))
        {
            _currentState = EnemyState.Idle;
        }
    }
    private void Chase()
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(_player.transform.position);
    }
    private void Idle()
    {
        if (!_isPatrolling)
        {
            _patrolPoint = GenerateRandomPoint(transform.position, 2);
            _isPatrolling = true;
            StartCoroutine(WaitAfterPatrol(5f));
        }
        _navMeshAgent.isStopped = false;
        _navMeshAgent.SetDestination(_patrolPoint);

        if (Vector3.Distance(transform.position, _patrolPoint) <= _navMeshAgent.stoppingDistance)
        {
            _navMeshAgent.isStopped = true;
        }
    }
    private Vector3 GenerateRandomPoint(Vector3 position, int range)
    {
        Vector3 randomPoint = position + Random.insideUnitSphere * range;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas);
        return hit.position;
    }
    private IEnumerator WaitAfterPatrol(float time)
    {
        yield return new WaitForSeconds(time);
        _isPatrolling = false;
    }
    private void Behaviour()
    {
        if (_currentState != EnemyState.Dead)
        {
            if (PlayerInRange(_ACTIVATION_RANGE))
            {
                _currentState = EnemyState.Chase;
            }
            else
            {
                _currentState = EnemyState.Idle;
            }

            switch (_currentState)
            {
                case EnemyState.Idle:
                    Idle();
                    break;
                case EnemyState.Chase:
                    Chase();
                    break;
                case EnemyState.Shoot:
                    break;
                case EnemyState.Inactive:
                    _navMeshAgent.isStopped = true;
                    break;
                default:
                    _navMeshAgent.isStopped = true;
                    Debug.Log("Default Enemy ERROR");
                    break;
            }
        }
        
    }
    private Vector3 GetPlayerDirection()
    {
        Vector3 targetPos;
        Vector3 moveDirection;

        targetPos = _player.transform.position;
        moveDirection = (targetPos - transform.position).normalized;
        return moveDirection;
    }
    private bool PlayerInRange(float range) 
    {
        _distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        if (_distanceToPlayer <= range)
        {
            _distanceToPlayer = 0;
            return true;
        }
        else
        {
            _distanceToPlayer = 0;
            return false;
        }
    }
    
    public void TakeDamage(int dmg)
    {
        _currentHealth -= dmg;
        switch (_enemyType)
        {
            default:
                Debug.Log("EnemyTakeDamageDefault");
                break;
        }

        if (_currentHealth <= 0)
        {
            Debug.Log("dfsdfsdfsfsd");
            _gameManager.ChangeScore(_pointValue);
            _UILinker.ScoreTextUI.text = _gameManager.Score.ToString();
            _currentState = EnemyState.Dead;
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            _navMeshAgent.isStopped = true;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enemy Collision: " + other.name);
    }
}
