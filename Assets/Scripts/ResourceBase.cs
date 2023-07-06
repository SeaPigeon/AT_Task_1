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
    [SerializeField] private int _resourceProduced = 1;
    [SerializeField] private int _STARTING_RES_QUANTITY = 5;
    [SerializeField] private MeshRenderer _fullMesh;
    [SerializeField] private MeshRenderer _halfMesh;
    [SerializeField] private MeshRenderer _emptyMesh;
    [SerializeField] private int _MAX_WORKERS = 3;

    [Header("Debug")]
    [SerializeField] List<AgentScript> _agentsAssignedList;
    [SerializeField] private ResourceType _resourceType;
    [SerializeField] private ResourceState _currentState;
    [SerializeField] private int _currentResQuantity;
    [SerializeField] private MeshRenderer _mesh;
    [SerializeField] private int _currentWorkers;
    private GameManagerScript _gameManager;

    private void Awake()
    {
        
    }
    private void Start()
    {
        SetUpReferences();
        _gameManager.ResourcesInGame.Add(this);
    }
    private void Update()
    {
      
    }
    public ResourceType ResourceType { get { return _resourceType; } set { _resourceType = value; } }
    public int ResourceProduced { get { return _resourceProduced; } set { _resourceProduced = value; } }
    public int MaxWorkers { get { return _MAX_WORKERS; } set { _MAX_WORKERS = value; } }
    public int CurrentWorkers { get { return _currentWorkers; } set { _currentWorkers = value; } }

    private void SetUpReferences()
    {
        _gameManager = GameManagerScript.GMInstance;
    }
    public void StartGathering(List<AgentScript> agents)
    {
        if (_currentState == ResourceState.Gatherable)
        {
            _agentsAssignedList.AddRange(agents);
            _currentWorkers = _agentsAssignedList.Count;

            foreach (var agent in _agentsAssignedList)
            {
                StartCoroutine(Gather(agent));
            }
        }

    }
    private IEnumerator Gather(AgentScript agent)
    {
        agent.AgentState = AgentState.Gathering;
        yield return new WaitForSeconds(2);
        // Every x second,
        // reduce risource available,
        // increase resource on agent,
        // change resource sprite if needed,
        // stop action if resource empty or agent is full
        // send player back to deposit
        // if empty, change state and start Respawn CoR
        Debug.Log("Swetting");
    }
    private void ResetResource()
    {
        _currentResQuantity = _STARTING_RES_QUANTITY;
        _mesh = _fullMesh;
        _currentState = ResourceState.Gatherable;

    }
    private void ReduceResource()
    {
        _currentResQuantity -= _resourceProduced;
    }
    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(2);
    }
}
