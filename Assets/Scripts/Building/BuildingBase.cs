using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BuildingType
{
    Armory,
    Deposit
}
public class BuildingBase : MonoBehaviour
{
    [SerializeField] private BuildingType _buildingType;
    [SerializeField] private int _MAX_INTERACTIONS;
    [SerializeField] private int _currentInteractions;
    [SerializeField] private float _INTERACTION_COMPLETION_TIME;
    [SerializeField] private float _currentCompletionTime;
    [SerializeField] List<AgentScript> _agentsAssignedList;
    [SerializeField] GameObject _interactPoint;
    private GameManagerScript _gameManager;

    public GameObject InteractPoint { get { return _interactPoint; } }

    private void Start()
    {
        SetUpReferences();
        SetUpBuilding();
    }
    private void SetUpReferences()
    {
        _gameManager = GameManagerScript.GMInstance;
    }
    private void SetUpBuilding()
    {
        _gameManager.BuildingsInGame.Add(this);
        _currentInteractions = 0;
    }
    public void StartInteract(AgentScript agent)
    {
        if (_currentInteractions < _MAX_INTERACTIONS)
        {
            Debug.Log("call");
            _agentsAssignedList.Add(agent);
            _currentInteractions++;
            agent.ActiveAgentState = AgentState.Interacting;
            agent.RunningInteractCoR = StartCoroutine(Interact(agent));
        }
        else
        {
            agent.ActiveAgentState = AgentState.Inactive;
        }
    }
    public void StopInteracting(AgentScript agent)
    {
        _currentInteractions--;
        agent.CurrentInteractionDuration = 0;
        agent.ActiveAgentState = AgentState.Inactive; // if not from click SendBackToDeposit Later
        _agentsAssignedList.Remove(agent);
    }
    public IEnumerator Interact(AgentScript agent)
    {
        // change resource mesh if needed,
        // send player back to resource

        Debug.Log("InteractionStarted");

        yield return new WaitForSeconds(_INTERACTION_COMPLETION_TIME);
        switch (_buildingType)
        {
            case BuildingType.Armory:
                agent.IsKnight = !agent.IsKnight;
                break;
            case BuildingType.Deposit:
                _gameManager.TotalRocks += agent.CarriedRock;
                _gameManager.TotalWood += agent.CarriedWood;
                agent.CarriedWood = 0;
                agent.CarriedRock = 0;
                
                break;
            default:
                break;
        }
        StopInteracting(agent);
        Debug.Log("InteractionFinished");
    }
}
