using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public enum DoorType
    {
        Default
    }
    
    public enum DoorState
    {
        Open,
        Closed
    }

    [Header("Door Variables")]
    [SerializeField] DoorType _doorType;
    [SerializeField] DoorState _currentDoorState;
    [SerializeField] float _openingSpeed = 15;
    [SerializeField] Vector3 _openPosition = new Vector3 (0, -2, 0);
    [SerializeField] Vector3 _closedPosition = new Vector3(0, 2, 0);
    [SerializeField] Vector3 _targetPosition;
    [SerializeField] Transform _door;
    [SerializeField] List<Collider> _objectsInTrigger;

    [SerializeField] AudioManagerScript _audioManager;

    // G&S
    public DoorState CurrentDoorState { get { return _currentDoorState; } }

    private void Awake()
    {
        _door = gameObject.transform.GetChild(0).transform;
        _currentDoorState = DoorState.Closed;
    }
    private void Start()
    {
        SetUpReferences();
    }
    private void Update()
    {
        ToggleDoor();
    }
    private void SetUpReferences()
    {
        _audioManager = AudioManagerScript.AMInstance;
    }
    private void ToggleDoor()
    {
        if (_currentDoorState == DoorState.Closed)
        {
            _targetPosition = _closedPosition;
        }
        else if (_currentDoorState == DoorState.Open)
        {
            _targetPosition = _openPosition;
        }

        _door.localPosition = Vector3.MoveTowards(_door.localPosition, _targetPosition, _openingSpeed * Time.deltaTime);
        
    }

    private void OpenDoor(Collider other)
    {
        if (other.GetComponent<PlayerScript>() || other.GetComponent<EnemyScript>())
        {
            _objectsInTrigger.Add(other);
            switch (_doorType)
            {
                case DoorType.Default:
                    _currentDoorState = DoorState.Open;
                    _audioManager.PlayDoorSFX();
                    break;

                default:
                    Debug.Log("Door Error");
                    break;
            }
        }
    }
    private void CloseDoor(Collider other)
    {
        if (other.GetComponent<PlayerScript>() || other.GetComponent<EnemyScript>())
        {
            _objectsInTrigger.Remove(other);
            if (_objectsInTrigger.Count == 0)
            {
                _currentDoorState = DoorState.Closed;
                _audioManager.PlayDoorSFX();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        OpenDoor(other);
    }
    private void OnTriggerExit(Collider other)
    {
        CloseDoor(other);
    }
}
