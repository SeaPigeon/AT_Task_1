using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ResourceType
{
    Corn,
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
                switch (_resourceType)
                {
                    case ResourceType.Corn:
                        if (_currentState == ResourceState.Gatherable &&
                            _currentWorkersCount < _MAX_WORKERS &&
                            agent.CarriedFood < agent.MaxFoodCarriable)
                        {
                            _agentsAssignedList.Add(agent);
                            _currentWorkersCount++;
                            _isGathering = true;
                            agent.ActiveCoR = null;
                            agent.ActiveCoR = StartCoroutine(Gather(agent));
                            PlayerScript.PlayerInstance.ActiveAgentsList.Remove(agent);
                            PlayerScript.PlayerInstance.HasAgentsInSelection();
                        }
                        else
                        {
                            agent.ActiveAgentState = AgentState.Inactive;
                            PlayerScript.PlayerInstance.ActiveAgentsList.Remove(agent);
                            PlayerScript.PlayerInstance.HasAgentsInSelection();
                            Debug.Log(agent.name + " Food Limit Reached");
                        }
                        break;
                    case ResourceType.Rock:
                        if (_currentState == ResourceState.Gatherable &&
                            _currentWorkersCount < _MAX_WORKERS &&
                            agent.CarriedRocks < agent.MaxRockCarriable)
                        {
                            _agentsAssignedList.Add(agent);
                            _currentWorkersCount++;
                            _isGathering = true;
                            agent.ActiveCoR = null;
                            agent.ActiveCoR = StartCoroutine(Gather(agent));
                            PlayerScript.PlayerInstance.ActiveAgentsList.Remove(agent);
                            PlayerScript.PlayerInstance.HasAgentsInSelection();
                        }
                        else
                        {
                            agent.ActiveAgentState = AgentState.Inactive;
                            PlayerScript.PlayerInstance.ActiveAgentsList.Remove(agent);
                            PlayerScript.PlayerInstance.HasAgentsInSelection();
                            Debug.Log(agent.name + " Rocks Limit Reached");
                        }
                        break;
                    case ResourceType.Wood:
                        if (_currentState == ResourceState.Gatherable &&
                            _currentWorkersCount < _MAX_WORKERS &&
                            agent.CarriedWood < agent.MaxWoodCarriable)
                        {
                            _agentsAssignedList.Add(agent);
                            _currentWorkersCount++;
                            _isGathering = true;
                            agent.ActiveCoR = null;
                            agent.ActiveCoR = StartCoroutine(Gather(agent));
                            PlayerScript.PlayerInstance.ActiveAgentsList.Remove(agent);
                            PlayerScript.PlayerInstance.HasAgentsInSelection();
                        }
                        else
                        {
                            agent.ActiveAgentState = AgentState.Inactive;
                            PlayerScript.PlayerInstance.ActiveAgentsList.Remove(agent);
                            PlayerScript.PlayerInstance.HasAgentsInSelection();
                            Debug.Log(agent.name + " Wood Limit Reached");
                        }
                        break;
                    default:
                        Debug.Log("Resource Type ERROR");
                        break;
                }
                break;
            case AgentClass.Knight:
                agent.ActiveAgentState = AgentState.Inactive;
                PlayerScript.PlayerInstance.ActiveAgentsList.Remove(agent);
                PlayerScript.PlayerInstance.HasAgentsInSelection();
                agent.ResourceToInteractWith = null;
                break;
            default:
                Debug.Log("Class Type ERROR");
                break;
        }
        PlayerScript.PlayerInstance.HasAgentsInSelection();
    }
    public void StopGathering(AgentScript agent)
    {
        agent.DisableIteractSlider();
        _currentWorkersCount--;
        agent.ActiveAgentState = AgentState.Inactive;
        agent.ResourceToInteractWith = null;
        if (agent.ActiveCoR != null)
        {
            agent.StopCoroutine(agent.ActiveCoR);
            agent.ActiveCoR = null;
        }
        _agentsAssignedList.Remove(agent);
        if (_currentResQuantity <= 0 || _agentsAssignedList.Count <= 0)
        {
            _isGathering = false;
        }
    }
    
    private IEnumerator Gather(AgentScript agent)
    {
        Debug.Log(name + " GatheringStarted");
        agent.ActiveAgentState = AgentState.Gathering;
        
        while (_isGathering)
        {
            agent.EnableInteractSlider();
            StartCoroutine(agent.FillBar(_timeBetweenGather));
            yield return new WaitForSeconds(_timeBetweenGather);

            if (_currentResQuantity <= 0)
            {
                StopGathering(agent);
                Debug.Log(name + " GatheringFinished: Resource Empty");
                _currentState = ResourceState.Depleted;
                yield break;
            }

            switch (_resourceType)
            {
                case ResourceType.Corn:
                    if (agent.CarriedFood >= agent.MaxFoodCarriable )
                    {
                        StopGathering(agent);
                        yield break;
                    }

                    agent.CarriedFood += IncreaseAgentResource();
                    ReduceResource();
                    break;
                
                case ResourceType.Rock:
                    if (agent.CarriedRocks >= agent.MaxRockCarriable)
                    {
                        StopGathering(agent);
                        yield break;
                    }

                    agent.CarriedRocks += IncreaseAgentResource();
                    ReduceResource();
                    break;

                case ResourceType.Wood: 
                    if (agent.CarriedWood >= agent.MaxWoodCarriable)
                    {
                        StopGathering(agent);
                        yield break;
                    }

                    agent.CarriedWood += IncreaseAgentResource();
                    ReduceResource();
                    break;

                default:
                    Debug.Log(name + " Resource Type ERROR");
                    break;
            }

            if (_currentResQuantity <= 0)
            {
                StopGathering(agent);
                Debug.Log(name + " GatheringFinished: Resource Empty");
                _currentState = ResourceState.Depleted;
                yield break;
            }
            Debug.Log(name + " Gathering...");
        }
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
    private int IncreaseAgentResource()
    {
        if (_currentResQuantity >= _resourceQuantityProduced)
        {
            return _resourceQuantityProduced;
        }
        else
        {
            return _currentResQuantity;
        }
    }
    private void ReduceResource()
    {
        if (_currentResQuantity >= _resourceQuantityProduced)
        {
            _currentResQuantity -= _resourceQuantityProduced;
        }
        else
        {
            _currentResQuantity = 0;
        }
        

    }
    private IEnumerator Respawn()
    {
        _currentState = ResourceState.Regenerating;

        yield return new WaitForSeconds(_timeToRegenOne);
        ResetResource();
    }
}
