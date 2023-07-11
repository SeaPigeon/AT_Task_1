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
    [SerializeField] private int _KNIGHT_COST_ROCKS = 2;
    [SerializeField] private int _KNIGHT_COST_WOOD = 2;
    [SerializeField] private int _currentInteractions;
    [SerializeField] private float _INTERACTION_COMPLETION_TIME;
    [SerializeField] List<AgentScript> _agentsAssignedList;
    [SerializeField] GameObject _interactPoint;
    private GameManagerScript _gameManager;

    public GameObject InteractPoint { get { return _interactPoint; } }
    public BuildingType BuildingType { get { return _buildingType; } }

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
                switch (agent.AgentClass)
                {
                    case AgentClass.Villager:
                        if (_currentInteractions < _MAX_INTERACTIONS &&
                            _gameManager.TotalWood >= _KNIGHT_COST_WOOD &&
                            _gameManager.TotalRocks >= _KNIGHT_COST_ROCKS)
                        {
                            _agentsAssignedList.Add(agent);
                            _currentInteractions++;
                            agent.ActiveAgentState = AgentState.Interacting;
                            agent.ActiveCoR = null;
                            //agent.EnableAgentUI(_INTERACTION_COMPLETION_TIME);
                            agent.ActiveCoR = StartCoroutine(Interact(agent));
                            PlayerScript.PlayerInstance.ActiveAgentsList.Remove(agent);
                            PlayerScript.PlayerInstance.HasAgentsInSelection();
                        }
                        else
                        {
                            Debug.Log("call");
                            if (_gameManager.TotalWood < _KNIGHT_COST_WOOD ||
                                _gameManager.TotalRocks < _KNIGHT_COST_ROCKS)
                            {
                                PlayerScript.PlayerInstance.ActiveCoR = StartCoroutine(PlayerScript.PlayerInstance.EmotionTextHandler(3, "2 Wood and 2 Rocks Needed to get an Armor", "..."));
                            }
                            agent.ActiveAgentState = AgentState.Inactive;
                            PlayerScript.PlayerInstance.ActiveAgentsList.Remove(agent);
                            PlayerScript.PlayerInstance.HasAgentsInSelection();
                        }
                        break;
                    case AgentClass.Knight:
                        if (_currentInteractions < _MAX_INTERACTIONS)
                        {
                            _agentsAssignedList.Add(agent);
                            _currentInteractions++;
                            agent.ActiveAgentState = AgentState.Interacting;
                            agent.ActiveCoR = null;
                            //agent.EnableAgentUI(_INTERACTION_COMPLETION_TIME);
                            agent.ActiveCoR = StartCoroutine(Interact(agent));
                            PlayerScript.PlayerInstance.ActiveAgentsList.Remove(agent);
                            PlayerScript.PlayerInstance.HasAgentsInSelection();
                        }
                        else
                        {
                            agent.ActiveAgentState = AgentState.Inactive;
                            PlayerScript.PlayerInstance.ActiveAgentsList.Remove(agent);
                            PlayerScript.PlayerInstance.HasAgentsInSelection();
                        }
                        break;
                    default:
                        break;
                }
                break;
            case BuildingType.Deposit:
                if (agent.AgentClass != AgentClass.Knight)
                {
                    if (_currentInteractions < _MAX_INTERACTIONS && 
                        (agent.CarriedFood > 0 || agent.CarriedRocks > 0 || agent.CarriedWood > 0))
                    {
                        _agentsAssignedList.Add(agent);
                        _currentInteractions++;
                        agent.ActiveAgentState = AgentState.Interacting;
                        agent.ActiveCoR = null;
                        //agent.EnableAgentUI(_INTERACTION_COMPLETION_TIME);
                        agent.ActiveCoR = StartCoroutine(Interact(agent));
                        PlayerScript.PlayerInstance.ActiveAgentsList.Remove(agent);
                        PlayerScript.PlayerInstance.HasAgentsInSelection();
                    }
                    else
                    {
                        agent.ActiveAgentState = AgentState.Inactive;
                        PlayerScript.PlayerInstance.ActiveAgentsList.Remove(agent);
                        PlayerScript.PlayerInstance.HasAgentsInSelection();
                    }
                }
                else
                {
                    agent.ActiveAgentState = AgentState.Inactive;
                    PlayerScript.PlayerInstance.ActiveAgentsList.Remove(agent);
                    PlayerScript.PlayerInstance.HasAgentsInSelection();
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
        agent.DisableIteractSlider();
        _currentInteractions--;
        agent.ActiveAgentState = AgentState.Inactive; 
        agent.BuildingToInteractWith = null;
        if (agent.ActiveCoR != null)
        {
            agent.StopCoroutine(agent.ActiveCoR);
            agent.ActiveCoR = null;
        }
        _agentsAssignedList.Remove(agent);

    }
    public IEnumerator Interact(AgentScript agent)
    {
        Debug.Log("InteractionStarted");
        agent.EnableInteractSlider();
        StartCoroutine(agent.FillBar(_INTERACTION_COMPLETION_TIME));
        yield return new WaitForSeconds(_INTERACTION_COMPLETION_TIME);
        switch (_buildingType)
        {
            case BuildingType.Armory:
                switch (agent.AgentClass)
                {
                    case AgentClass.Villager:
                        if (_gameManager.TotalWood >= _KNIGHT_COST_WOOD &&
                            _gameManager.TotalRocks >= _KNIGHT_COST_ROCKS)
                        {
                            _gameManager.TotalRocks -= _KNIGHT_COST_ROCKS;
                            _gameManager.TotalWood -= _KNIGHT_COST_WOOD;
                            agent.AgentClass = AgentClass.Knight;
                        }
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
                _gameManager.TotalFood += agent.CarriedFood;
                _gameManager.TotalRocks += agent.CarriedRocks;
                _gameManager.TotalWood += agent.CarriedWood;
                agent.CarriedFood = 0;
                agent.CarriedWood = 0;
                agent.CarriedRocks = 0;
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
