using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ResourceType
{
    Cloth,
    Iron,
    Wood
}
public enum ResourceState
{
    Gatherable,
    Depleted,
    Regenerating
}
public class ResourceBase : MonoBehaviour
{
    [Header("Variables Resource")]
    [SerializeField] private int _resourceQuantityProduced = 1;
    [SerializeReference] private float _timeBetweenGather = 3;
    [SerializeField] private int _STARTING_RES_QUANTITY = 5;
    private MeshRenderer _fullMesh;
    private MeshRenderer _halfMesh;
    private MeshRenderer _emptyMesh;
    [SerializeField] private int _MAX_WORKERS = 3;

    [Header("Debug")]
    [SerializeField] List<AgentScript> _agentsAssignedList;
    private ResourceType _resourceType;
    [SerializeField] private ResourceState _currentState;
    [SerializeField] private int _currentResQuantity;
    //private MeshRenderer _mesh;
    [SerializeField] private int _currentWorkersCount;
    private GameManagerScript _gameManager;

    private void Awake()
    {
        
    }
    private void Start()
    {
        SetUpReferences();
        SetUpResource();
    }
    private void Update()
    {
        switch (_currentState)
        {
            case ResourceState.Gatherable:
                break;
            case ResourceState.Depleted:
                StartCoroutine(Respawn());
                break;
            case ResourceState.Regenerating:
                break;
            default:
                break;
        }
    }

    public ResourceType ResourceType { get { return _resourceType; } set { _resourceType = value; } }
    public int ResourceProduced { get { return _resourceQuantityProduced; } set { _resourceQuantityProduced = value; } }
    public int MaxWorkers { get { return _MAX_WORKERS; } set { _MAX_WORKERS = value; } }
    public int CurrentWorkersCount { get { return _currentWorkersCount; } set { _currentWorkersCount = value; } }

    private void SetUpReferences()
    {
        _gameManager = GameManagerScript.GMInstance;
    }
    private void SetUpResource()
    {
        _gameManager.ResourcesInGame.Add(this);
        _currentResQuantity = _STARTING_RES_QUANTITY;
    }
    public void StartGathering(AgentScript agent)
    {
        if (_currentState == ResourceState.Gatherable)
        {
            Debug.Log(_agentsAssignedList.Count);
            _agentsAssignedList.Add(agent);
            Debug.Log(_agentsAssignedList.Count);
            _currentWorkersCount++;

            StartCoroutine(Gather(agent));
        }
        else
        {
            // reset player state etc...
        }
    }
    private IEnumerator Gather(AgentScript agent)
    {
        // change resource sprite if needed,
        // send player back to deposit

        Debug.Log("GatheringStarted");
        agent.AgentState = AgentState.Gathering;
        agent.ChangeColor(Color.blue);
        Debug.Log("Start While Lood");
        while (_currentResQuantity > 0 )
        {
            Debug.Log(Time.time);
            yield return new WaitForSeconds(_timeBetweenGather);

            if (agent.CarriedWood <= agent.MaxWoodCarriable)
            {
                Debug.Log(Time.time);
                ReduceResource();
                agent.CarriedWood++;
                Debug.Log("Gathering");
                Debug.Log("carried wood: " + agent.CarriedWood);
                Debug.Log("wood left: " + _currentResQuantity);
            }
            else
            {
                Debug.Log("Call;");
                _currentState = ResourceState.Regenerating;
                _currentWorkersCount--;
                _agentsAssignedList.Remove(agent);
                agent.AgentState = AgentState.Inactive;
                agent.ChangeColor(Color.black);
                PlayerScript.PlayerInstance.HasAgentsInSelection();
            }
        }
        if (_currentResQuantity <= 0)
        {
            
            _currentState = ResourceState.Regenerating;
            _currentWorkersCount--;
            agent.AgentState = AgentState.Inactive;
            agent.ChangeColor(Color.black);
            _agentsAssignedList.Clear();
            PlayerScript.PlayerInstance.ActiveAgentsList.Clear();
            PlayerScript.PlayerInstance.HasAgentsInSelection();
            Debug.Log("Call1;");
        }
        Debug.Log("GatheringFinished");
    }
    private void ResetResource()
    {
        _currentResQuantity = _STARTING_RES_QUANTITY;
        //_mesh = _fullMesh;
        _currentState = ResourceState.Gatherable;

    }
    private void IncreaseResource()
    {
        _currentResQuantity += _resourceQuantityProduced;
        if (_currentResQuantity >= _STARTING_RES_QUANTITY)
        {
            _currentState = ResourceState.Gatherable;
        }
    }
    private void ReduceResource()
    {
        _currentResQuantity -= _resourceQuantityProduced;
        if (_currentResQuantity <= 0)
        {
            _currentState = ResourceState.Depleted;
        }
    }
    private IEnumerator Respawn()
    {
        _currentState = ResourceState.Regenerating;

        while (_currentResQuantity < _STARTING_RES_QUANTITY)
        {
            yield return new WaitForSeconds(3);
            IncreaseResource();
            
        }
        
        yield return new WaitForSeconds(2);
    }
}
