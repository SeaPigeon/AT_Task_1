using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AgentState
{
    Inactive,
    Moving,
    Working
}
public class AgentScript : MonoBehaviour
{
    [Header("Agent Variables")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _rotationSpeed;

    [Header("Debug")]
    [SerializeField] AgentState _agentState;
    [SerializeField] private Vector3 _targetPos;
    [SerializeField] private Vector3 _targetRotation;
    [SerializeField] private bool _isSelected;
    [SerializeField] NavMeshAgent _navMeshAgent;

    private void Start() 
    {
        SetUpReferences();
        SetUpAgent();
    }
    private void Update()
    {
        if (_agentState == AgentState.Moving)
        {
            Move(_targetPos);
        }
        Rotate(_targetPos);
    }

    // G&S
    public Vector3 MoveTargetPosition { get { return _targetPos; } set { _targetPos = value; } }
    public bool IsAgentSelected { get { return _isSelected; } set { _isSelected = value; } }
    public AgentState AgentState { get { return _agentState; } set { _agentState = value; } }

    // Methods
    private void SetUpReferences()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
    }
    private void SetUpAgent() 
    {
        _targetPos = transform.position;
        _targetRotation = Vector3.zero;
        _agentState = AgentState.Inactive;
    }
    private void Move(Vector3 targetPos)
    {
        if (transform.position != targetPos)
        {
            _navMeshAgent.isStopped = false;
            _navMeshAgent.SetDestination(_targetPos);
        }
        else
        {
            StopAgent();
        }
    }
    private void Rotate(Vector3 targetPos)
    {
        /*if (Vector3.Dot(transform.position, targetPos) >= 0.90f)
        {
            var targetDirection = targetPos - transform.position;

            _targetRotation = Vector3.RotateTowards(transform.forward, targetDirection, _rotationSpeed * Time.deltaTime, 0f);
            transform.rotation = Quaternion.LookRotation(_targetRotation);
        }*/
        var targetDirection = targetPos - transform.position;

        _targetRotation = Vector3.RotateTowards(transform.forward, targetDirection, _rotationSpeed * Time.deltaTime, 0f);
        transform.rotation = Quaternion.LookRotation(_targetRotation);

    }
    private void StopAgent()
    {
        _agentState = AgentState.Inactive;
        _navMeshAgent.isStopped = true;
    }
    public void OnMoveAgent(Vector3 targetPos)
    {
        _agentState = AgentState.Moving;
        Debug.Log(gameObject.name + " Movement Started");
    }

}
