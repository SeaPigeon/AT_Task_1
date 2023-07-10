using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;



public enum EnemyType
{
    Ghost
}
public enum EnemyState
{
    Inactive,
    Chase,
    Attack,
    Dead
}

public class EnemyScript : MonoBehaviour
{
    [Header("Enemy Type")]
    [SerializeField] EnemyType _enemyType;
    [SerializeField] EnemyState _currentState;

    [Header("Ghost")]
    [SerializeField] int _MAX_HEALTH = 40;
    [SerializeField] int _POINT_VALUE = 100;

    [Header("Debug")]
    [SerializeField] GameObject _fightPoint;
    [SerializeField] GameManagerScript _gameManager;
    [SerializeField] AudioManagerScript _audioManager;
    [SerializeField] LinkUIScript _UILinker;
    [SerializeField] int _currentHealth;
    [SerializeField] int _pointValue;
    [SerializeField] float _distanceToPlayer;
    [SerializeField] NavMeshAgent _navMeshAgent;

    // G&S
    public EnemyState CurrentEnemyState { get { return _currentState; } }
    public GameObject FightPoint { get { return _fightPoint; } set { _fightPoint = value; } }

    void Start()
    {
        SetUpReferences();
        SetUpEnemy();
    }

    void Update()
    {
        Behaviour();
    }

    private void SetUpReferences()
    {
        _gameManager = GameManagerScript.GMInstance;
        _audioManager = AudioManagerScript.AMInstance;
        _UILinker = UIManagerScript.UIMInstance.GetComponent<LinkUIScript>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    
    private void SetUpEnemy()
    {
        _gameManager.EnemiesInGame.Add(this);
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _currentHealth = _MAX_HEALTH;
    }
    private void Chase()
    {
        _navMeshAgent.isStopped = false;
        //_navMeshAgent.SetDestination(_player.transform.position);
    }
    
    private Vector3 GenerateRandomPoint(Vector3 position, int range)
    {
        Vector3 randomPoint = position + Random.insideUnitSphere * range;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas);
        return hit.position;
    }
    
    private void Behaviour()
    {
        switch (_currentState)
        {
            case EnemyState.Inactive:
                break;
            case EnemyState.Chase:
                break;
            case EnemyState.Attack:
                break;
            case EnemyState.Dead:
                break;
            default:
                break;
        }

    }
    /*private Vector3 GetPlayerDirection()
    {
        Vector3 targetPos;
        Vector3 moveDirection;

        //targetPos = _player.transform.position;
        //moveDirection = (targetPos - transform.position).normalized;
        return moveDirection;
    }*/
    private bool PlayerInRange(float range) 
    {
        //_distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

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

        if (_currentHealth <= 0)
        {
            _navMeshAgent.isStopped = true;
            _gameManager.ChangeScore(_pointValue);
            _UILinker.ScoreTextUI.text = _gameManager.Score.ToString();
            _currentState = EnemyState.Dead;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Enemy Collision: " + other.name);
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(0);
    } // moveto enemy
}
