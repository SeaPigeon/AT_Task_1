using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ResourceType
{
    Rock,
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
    [SerializeField] private int _MAX_WORKERS = 3;
    [SerializeField] GameObject _gatherPoint;

    [Header("Meshes")]
    [SerializeField] Mesh _defaultMesh;
    [SerializeField] Mesh _depletedMesh;

    [Header("Debug")]
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private ResourceState _currentState;
    [SerializeField] private int _currentWorkersCount;
    [SerializeField] private int _currentResQuantity;
    [SerializeField] private bool _isGathering;
    [SerializeField] List<AgentScript> _agentsAssignedList;
    private MeshFilter _meshFilter;
    
    private GameManagerScript _gameManager;

    // G&S
    public ResourceType ResourceType { get { return _resourceType; } set { _resourceType = value; } }
    public int ResourceProduced { get { return _resourceQuantityProduced; } set { _resourceQuantityProduced = value; } }
    public int MaxWorkers { get { return _MAX_WORKERS; } set { _MAX_WORKERS = value; } }
    public int CurrentWorkersCount { get { return _currentWorkersCount; } set { _currentWorkersCount = value; } }
    public GameObject GatherPoint { get { return _gatherPoint; } }
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
                SetMesh(_defaultMesh);
                break;
            case ResourceState.Depleted:
                SetMesh(_depletedMesh);
                StartCoroutine(Respawn());
                break;
            case ResourceState.Regenerating:
                break;
            default:
                Debug.Log("Resource State ERROR");
                break;
        }
    }

    private void SetUpReferences()
    {
        _gameManager = GameManagerScript.GMInstance;
        _meshFilter = GetComponent<MeshFilter>();
    }
    private void SetUpResource()
    {
        _gameManager.ResourcesInGame.Add(this);
        ResetResource();
        
    }
    public void StartGathering(AgentScript agent)
    {
        switch (agent.AgentClass)
        {
            case AgentClass.Villager:
                if (_currentState == ResourceState.Gatherable &&
                    _currentWorkersCount < _MAX_WORKERS)
                {
                    _agentsAssignedList.Add(agent);
                    _currentWorkersCount++;
                    _isGathering = true;
                    agent.ActiveCoR = null;
                    agent.ActiveCoR = StartCoroutine(Gather(agent));
                }
                else
                {
                    agent.ActiveAgentState = AgentState.Inactive;
                }
                break;
            case AgentClass.Knight:
                agent.ActiveAgentState = AgentState.Inactive;
                agent.ResourceToInteractWith = null;
                break;
            default:
                Debug.Log("Class Type ERROR");
                break;
        }
        
    }
    public void StopGathering(AgentScript agent)
    {
        _currentWorkersCount--;
        agent.ActiveAgentState = AgentState.Inactive; // if not from click SendBackToDeposit Later
        agent.ResourceToInteractWith = null;
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

        Debug.Log(name + " GatheringStarted");
        agent.ActiveAgentState = AgentState.Gathering;
        while (_isGathering)
        {
            switch (_resourceType)
            {
                case ResourceType.Rock:
                    if (agent.CarriedRock >= agent.MaxRockCarriable || _currentResQuantity <= 0)
                    {
                        StopGathering(agent);
                        _currentState = ResourceState.Depleted;
                        Debug.Log(name + " Empty, Regenerating...");
                        yield break;
                    }
                    else if(_currentWorkersCount >= _MAX_WORKERS)
                    {
                        StopGathering(agent);
                        Debug.Log(name + " Max Workers Reached");
                        yield break;
                    }
                    break;

                case ResourceType.Wood:
                    if (agent.CarriedWood >= agent.MaxWoodCarriable || _currentResQuantity <= 0)
                    {
                        StopGathering(agent);
                        _currentState = ResourceState.Depleted;
                        Debug.Log(name + " Empty, Regenerating...");
                        yield break;
                    }
                    else if (_currentWorkersCount >= _MAX_WORKERS)
                    {
                        StopGathering(agent);
                        Debug.Log(name + " Max Workers Reached");
                        yield break;
                    }
                    break;

                default:
                    Debug.Log(name + " Resource Type ERROR");
                    break;
            }
            
            yield return new WaitForSeconds(_timeBetweenGather);

            switch (_resourceType)
            {
                case ResourceType.Rock:
                    if (agent.CarriedRock < agent.MaxRockCarriable && _currentResQuantity > 0)
                    {
                        ReduceResource();
                        agent.CarriedRock++;
                        Debug.Log(name + " Gathering Rocks...");
                    }
                    break;

                case ResourceType.Wood:
                    if (agent.CarriedWood < agent.MaxWoodCarriable && _currentResQuantity > 0)
                    {
                        ReduceResource();
                        agent.CarriedWood++;
                        Debug.Log(name + " Gathering Wood...");
                    }
                    break;

                default:
                    Debug.Log(name + " Resource Type ERROR");
                    break;
            }
            Debug.Log(_currentResQuantity);
        }
        Debug.Log(name + " GatheringFinished");
    }
    private void SetMesh(Mesh mesh)
    {
        if (_meshFilter.sharedMesh != mesh)
        {
            _meshFilter.sharedMesh = mesh;
        }
    }

    private void ResetResource()
    {
        _currentResQuantity = _STARTING_RES_QUANTITY;
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

    }
    private IEnumerator Respawn()
    {
        _currentState = ResourceState.Regenerating;

        yield return new WaitForSeconds(_timeToRegenOne);
        ResetResource();
    }
}
