using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BuildingType
{
    Armory,
    Deposit,
    House
}
public class BuildingBase : MonoBehaviour
{
    [SerializeField] private BuildingType _buildingType;
    [SerializeField] private int _MAX_INTERACTIONS;
    [SerializeField] private int _currentInteractions;
    [SerializeField] private float _INTERACTION_COMPLETION_TIME;
    [SerializeField] private float _currentCompletionTime;
    [SerializeField] List<AgentScript> _agentsAssignedList;
    
    private GameManagerScript _gameManager;
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

        while (agent.ActiveAgentState == AgentState.Interacting)
        {
            if (agent.CurrentInteractionDuration < _INTERACTION_COMPLETION_TIME)
            {
                agent.CurrentInteractionDuration += Time.deltaTime;
                Debug.Log("Interacting...");
            }
            else
            {
                StopInteracting(agent);
                Debug.Log("InteractionFinished");
                yield break;
            } 
        }
    }
}
