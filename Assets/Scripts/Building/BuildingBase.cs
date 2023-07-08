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
        switch (_buildingType)
        {
            case BuildingType.Armory:
                if (_currentInteractions < _MAX_INTERACTIONS)
                {
                    _agentsAssignedList.Add(agent);
                    _currentInteractions++;
                    agent.ActiveAgentState = AgentState.Interacting;
                    agent.ActiveCoR = null;
                    agent.EnableAgentUI(_INTERACTION_COMPLETION_TIME);
                    agent.ActiveCoR = StartCoroutine(Interact(agent));
                }
                else
                {
                    agent.ActiveAgentState = AgentState.Inactive;
                }
                break;
            case BuildingType.Deposit:
                if (agent.AgentClass != AgentClass.Knight)
                {
                    if (_currentInteractions < _MAX_INTERACTIONS && 
                        (agent.CarriedRock > 0 || agent.CarriedWood > 0))
                    {
                        Debug.Log("call");
                        _agentsAssignedList.Add(agent);
                        _currentInteractions++;
                        agent.ActiveAgentState = AgentState.Interacting;
                        agent.ActiveCoR = null;
                        agent.EnableAgentUI(_INTERACTION_COMPLETION_TIME);
                        agent.ActiveCoR = StartCoroutine(Interact(agent));
                    }
                    else
                    {
                        agent.ActiveAgentState = AgentState.Inactive;
                    }
                }
                else
                {
                    agent.ActiveAgentState = AgentState.Inactive;
                    agent.BuildingToInteractWith = null;
                }
                break;
            default:
                Debug.Log("Build Type ERROR");
                break;
        }
        
    }
    public void StopInteracting(AgentScript agent)
    {
        _currentInteractions--;
        agent.ActiveAgentState = AgentState.Inactive; // if not from click SendBackToDeposit Later
        agent.BuildingToInteractWith = null;
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
                switch (agent.AgentClass)
                {
                    case AgentClass.Villager:
                        agent.AgentClass = AgentClass.Knight;
                        break;

                    case AgentClass.Knight:
                        agent.AgentClass = AgentClass.Villager;
                        break;

                    default:
                        Debug.Log("ERROR ARMORY");
                        break;
                }
                break;
            case BuildingType.Deposit:
                _gameManager.TotalRocks += agent.CarriedRock;
                _gameManager.TotalWood += agent.CarriedWood;
                agent.CarriedWood = 0;
                agent.CarriedRock = 0;
                // Send back to resource if resource is not empty
                break;
            default:
                Debug.Log("BUILDINGS ERROR");
                break;
        }
        StopInteracting(agent);
        Debug.Log("InteractionFinished");
    }
}
