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
    [SerializeReference] private float _timeToRegenOne = 3;
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
    [SerializeField] private bool _isGathering;
    //private MeshRenderer _mesh;
    [SerializeField] private int _currentWorkersCount;
    private GameManagerScript _gameManager;

    // G&S
    public ResourceType ResourceType { get { return _resourceType; } set { _resourceType = value; } }
    public int ResourceProduced { get { return _resourceQuantityProduced; } set { _resourceQuantityProduced = value; } }
    public int MaxWorkers { get { return _MAX_WORKERS; } set { _MAX_WORKERS = value; } }
    public int CurrentWorkersCount { get { return _currentWorkersCount; } set { _currentWorkersCount = value; } }

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

    private void SetUpReferences()
    {
        _gameManager = GameManagerScript.GMInstance;
    }
    private void SetUpResource()
    {
        _gameManager.ResourcesInGame.Add(this);
        ResetResource();
    }
    public void StartGathering(AgentScript agent)
    {
        if (_currentState == ResourceState.Gatherable)
        {
            _agentsAssignedList.Add(agent);
            _currentWorkersCount++;
            _isGathering = true;
            agent.RunningGatherCoR = StartCoroutine(Gather(agent));
        }
        else
        {
            Debug.Log("Resource Not Available ATM, Try Again Later");
            // reset player state etc...
        }
    }
    public void StopGathering(AgentScript agent)
    {
        _currentWorkersCount--;
        agent.AgentState = AgentState.Inactive; // if not from click SendBackToDeposit Later
        _agentsAssignedList.Remove(agent);

        if (_currentResQuantity <= 0 || _agentsAssignedList.Count <= 0)
        {
            _isGathering = false;
        }
    }
    public IEnumerator Gather(AgentScript agent)
    {
        // change resource mesh if needed,
        // send player back to deposit

        Debug.Log("GatheringStarted");
        agent.AgentState = AgentState.Gathering;
        while (_isGathering)
        {
            yield return new WaitForSeconds(_timeBetweenGather);

            if (agent.CarriedWood < agent.MaxWoodCarriable && _currentResQuantity > 0)
            {
                ReduceResource();
                agent.CarriedWood++;
                Debug.Log("Gathering...");
            }
            else
            {
                StopGathering(agent);

                if (_currentResQuantity <= 0)
                {
                    _currentState = ResourceState.Regenerating;
                    Debug.Log(name + " Empty, Regenerating...");
                    yield break;
                }
                else
                {
                    Debug.Log("Full Agent");
                    yield break;
                }
            }
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
            Debug.Log("Resource Ready");
        }
    }
    private void ReduceResource()
    {
        _currentResQuantity -= _resourceQuantityProduced;
        if (_currentResQuantity <= 0)
        {
            _currentState = ResourceState.Depleted;
            Debug.Log("Resource Depleted");
        }
    }
    private IEnumerator Respawn()
    {
        _currentState = ResourceState.Regenerating;

        while (_currentResQuantity < _STARTING_RES_QUANTITY)
        {
            yield return new WaitForSeconds(_timeToRegenOne);
            IncreaseResource();
            
        }
    }
}
