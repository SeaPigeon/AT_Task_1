using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;

public enum PlayerStates
{
    Rest,
    Selecting
}
public class PlayerScript : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] float _moveSpeed;
    [SerializeField] float _runSpeed;
    [SerializeField] float _rotationSpeed;
    [SerializeField] float _ascentSpeed;
    [SerializeField] float _maxHeight;
    [SerializeField] float _minHeight;

    [Header("PlayerInput")]
    [SerializeField] CharacterController _playerCC;
    [SerializeField] Vector2 _movementInput;
    [SerializeField] Vector2 _rotateInput;
    [SerializeField] bool _southButtonInput;
    [SerializeField] bool _RSInput;
    [SerializeField] bool _LSInput;
    [SerializeField] bool _westButtonInput;
    private Vector3 _moveVector;
    private Vector3 _appliedMoveVector;

    [Header("Debug")]
    [SerializeField] PlayerStates _state;
    [SerializeField] List<AgentScript> _activeAgentsList;
    [SerializeField] List<Collider> _objectsInTrigger;
    [SerializeField] CinemachineVirtualCamera _gameCam;
    private static PlayerScript _playerInstance;
    [SerializeField] GameManagerScript _gameManager;
    [SerializeField] SceneManagerScript _sceneManager;
    [SerializeField] InputManagerScript _inputManager;
    [SerializeField] LinkUIScript _UILinker;
    [SerializeField] AudioManagerScript _audioManager;
    [SerializeField] Transform _spawnPoint;
    [SerializeField] MeshRenderer _playerMesh;
    [SerializeField] Transform _gameCamBody;
    [SerializeField] Vector3 _startingCamPos;
    [SerializeField] Quaternion _startingCamRot;
    bool isOnNavMesh;

    float corner1 = 0;
    float corner2 = 0;
    float corner3 = 0;
    float corner4 = 0;
    float bigX = 0;
    float smallX = 0;
    float bigY = 0;
    float smallY = 0;

    // G&S
    public static PlayerScript PlayerInstance { get { return _playerInstance; } }
    public Vector2 MovementInput { get { return _movementInput; } set { _movementInput = value; } }
    public Vector2 RotateInput { get { return _rotateInput; } set { _rotateInput = value; } }
    public bool FireInput { get { return _southButtonInput; } set { _southButtonInput = value; } }
    public CinemachineVirtualCamera InGameCamera { get { return _gameCam; } }

    private void Awake() 
    {
        PlayerSingleton();
    }
    private void Start()
    {
        SetUpReferences();
        SubscribeToEvents();
        ResetPlayer();
    }

    private void Update()
    {
        Move(MovementInput);
        Ascend(_RSInput);
        Descend(_LSInput);
        ApplyGravity();
    }
    private void LateUpdate()
    {
        RotateH(RotateInput);
        RotateV(RotateInput);
    }

    // G&S
    public Vector3 CursorPosition { get { return transform.position; } }

    // Methods
    private void PlayerSingleton()
    {
        if (_playerInstance == null)
        {
            _playerInstance = this;
        }
        else if (_playerInstance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    private void SetUpReferences()
    {
        _gameManager = GameManagerScript.GMInstance;
        _inputManager = InputManagerScript.IMInstance;
        _sceneManager = SceneManagerScript.SMInstance;
        _UILinker = UIManagerScript.UIMInstance.GetComponent<LinkUIScript>();
        _audioManager = AudioManagerScript.AMInstance;
        _playerCC = gameObject.GetComponent<CharacterController>();
        _playerMesh = gameObject.GetComponentInChildren<MeshRenderer>();
        _gameCam = GetComponentInChildren<CinemachineVirtualCamera>();
        _gameCamBody = _gameCam.GetComponentInParent<Transform>();
    }
    private void SubscribeToEvents()
    {
        _gameManager.OnGMSetUpComplete -= SetUpPlayer;
        _gameManager.OnGMSetUpComplete += SetUpPlayer;
    }
    public void SubscribeGameInputs()
    {
        // UNSUB
        _inputManager.InputMap.Game.Move.performed -= OnMove;
        _inputManager.InputMap.Game.Rotate.performed -= OnRotate;
        _inputManager.InputMap.Game.ButtonSouth.started -= OnButtonSouth;
        _inputManager.InputMap.Game.ButtonWest.started -= OnButtonWest;
        _inputManager.InputMap.Game.ShoulderR.started -= OnShoulderR;
        _inputManager.InputMap.Game.ShoulderL.started -= OnShoulderL;

        _inputManager.InputMap.Game.Move.canceled -= OnMove;
        _inputManager.InputMap.Game.Rotate.canceled -= OnRotate;
        _inputManager.InputMap.Game.ButtonSouth.canceled -= OnButtonSouth;
        _inputManager.InputMap.Game.ButtonWest.canceled -= OnButtonWest;
        _inputManager.InputMap.Game.ShoulderR.canceled -= OnShoulderR;
        _inputManager.InputMap.Game.ShoulderL.canceled -= OnShoulderL;

        // SUB
        _inputManager.InputMap.Game.Move.performed += OnMove;
        _inputManager.InputMap.Game.Rotate.performed += OnRotate;
        _inputManager.InputMap.Game.ButtonSouth.started += OnButtonSouth;
        _inputManager.InputMap.Game.ButtonWest.started += OnButtonWest;
        //_inputManager.InputMap.Game.ButtonNorth.performed += OnButtonNorth;
        //_inputManager.InputMap.Game.ButtonEast.performed += OnButtonEast;
        _inputManager.InputMap.Game.ShoulderR.started += OnShoulderR;
        _inputManager.InputMap.Game.ShoulderL.started += OnShoulderL;
        //_inputManager.InputMap.Game.StartButton.performed += OnStartButton;

        _inputManager.InputMap.Game.Move.canceled += OnMove;
        _inputManager.InputMap.Game.Rotate.canceled += OnRotate;
        _inputManager.InputMap.Game.ButtonSouth.canceled += OnButtonSouth;
        _inputManager.InputMap.Game.ButtonWest.canceled += OnButtonWest;
        //_inputManager.InputMap.Game.ButtonNorth.canceled += OnButtonNorth;
        //_inputManager.InputMap.Game.ButtonEast.canceled += OnButtonEast;
        _inputManager.InputMap.Game.ShoulderR.canceled += OnShoulderR;
        _inputManager.InputMap.Game.ShoulderL.canceled += OnShoulderL;
        //_inputManager.InputMap.Game.StartButton.canceled += OnStartButton;
    }
    private void SetUpPlayer()
    {
        _state = PlayerStates.Rest;
        _startingCamPos = _gameCamBody.transform.localPosition;
        _startingCamRot = _gameCamBody.localRotation;
        _minHeight = _gameCamBody.position.y;

        LinkUI();
        if (SceneManager.GetActiveScene().buildIndex == 4 ||
            SceneManager.GetActiveScene().buildIndex == 5 ||
            SceneManager.GetActiveScene().buildIndex == 6)
        {
            SpawnPlayer(Vector3.zero);
        }
        else
        {
            TogglePlayerMesh(false);
        }
    }

    public void ResetPlayer() 
    {
        _gameManager.ResetScore();
        _gameManager.Victory = false;
    }
    public void TogglePlayerMesh(bool state)
    {
        if (state != _playerMesh.enabled)
        {
            _playerMesh.enabled = state;
        }
    }
    public void MoveToSpawnPoint(Vector3 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, pos.z);
       
        //Debug.Log("Player Spawned from GMEvent: " + transform.position);
    }
    public void SpawnPlayer(Vector3 pos)
    {
        TogglePlayerMesh(true);
        MoveToSpawnPoint(pos);
    }
    private void LinkUI()
    {
        //Debug.Log("LinkUI Function Call!");
    }

    // Gameplay
    private void Move(Vector2 input)
    {
        _moveVector.x = input.x * _moveSpeed;
        _moveVector.z = input.y * _moveSpeed;

        _appliedMoveVector = transform.TransformDirection(_moveVector);
        _playerCC.Move(_appliedMoveVector * Time.deltaTime);
    }
    private void RotateH(Vector2 input)
    {
        gameObject.transform.Rotate(new Vector3(0, input.x * _rotationSpeed * Time.deltaTime, 0));
    }
    private void RotateV(Vector2 input)
    {
        _gameCamBody.transform.Rotate(new Vector3(input.y * _rotationSpeed * Time.deltaTime, 0, 0));
    }
    private void Ascend(bool input)
    {
        if (_gameCamBody.transform.position.y < _maxHeight && input)
        {
            Vector3 targetPos = new Vector3(_gameCamBody.position.x, _maxHeight, _gameCamBody.position.z);
            _gameCamBody.transform.position = Vector3.MoveTowards(_gameCamBody.transform.position, targetPos, _ascentSpeed * Time.deltaTime);
        }
    }
    private void Descend(bool input)
    {
        if (_gameCamBody.transform.position.y > _minHeight && input)
        {
            Vector3 targetPos = new Vector3(_gameCamBody.position.x, _minHeight, _gameCamBody.position.z);
            _gameCamBody.transform.position = Vector3.MoveTowards(_gameCamBody.transform.position, targetPos, _ascentSpeed * Time.deltaTime);
        }
    }
    private void ResetCamPosition()
    {
        _gameCamBody.transform.localPosition = _startingCamPos;
        _gameCamBody.transform.localRotation = _startingCamRot;
    }
    private void ApplyGravity()
    {
        if (!_playerCC.isGrounded)
        {
            _playerCC.Move(Vector3.down* 9.81f * Time.deltaTime);
        }
    }
    private void AddAgentToSelection(AgentScript agent)
    {
        _activeAgentsList.Add(agent.GetComponent<AgentScript>());
        agent.GetComponent<AgentScript>().IsAgentSelected = true;
        agent.GetComponent<MeshRenderer>().material.color = Color.green;
    }
    private void RemoveAgentFromSelection(AgentScript agent)
    {
        _activeAgentsList.Remove(agent.GetComponent<AgentScript>());
        agent.GetComponent<AgentScript>().IsAgentSelected = false;
        agent.GetComponent<MeshRenderer>().material.color = Color.black;
    }
    private void ToggleSelection(bool input)
    {
        if (input)
        {
            if (_objectsInTrigger.Count > 0 )
            {
                foreach (var agent in _objectsInTrigger)
                {
                    if (!agent.GetComponent<AgentScript>().IsAgentSelected)
                    {
                        AddAgentToSelection(agent.GetComponent<AgentScript>());
                    }
                    else
                    {
                        RemoveAgentFromSelection(agent.GetComponent<AgentScript>());
                    }
                }

                Debug.Log("Agent List: " + _activeAgentsList);
            }
            else
            {
                Debug.Log("No Objects in trigger area");
            }
        }
    }
    private void MoveAgent(bool input)
    {
        if (input && _objectsInTrigger.Count == 0)
        {
            foreach (var agent in _activeAgentsList)
            {
                agent.MoveTargetPosition = transform.position;
                agent.AgentState = AgentState.Moving;
            }
        }
    }
    private bool ClickIsOnNavMesh()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        NavMeshHit navMeshHit;
        //bool isOnNavMesh;
        if (Physics.Raycast(ray, out hit))
        {
            isOnNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, 0f, NavMesh.AllAreas);
        }
        return isOnNavMesh;
    }
    private void SelectionArea(bool input)
    {
        if (input)
        {
            _state = PlayerStates.Selecting;
            corner1 = transform.position.x;
            corner2 = transform.position.z;
        }
        else if (!input)
        {
            corner3 = transform.position.x;
            corner4 = transform.position.y;

            if (corner1 < corner3)
            {
                bigX = corner3;
                smallX = corner1;
            }
            else
            {
                bigX = corner1;
                smallX = corner3;
            }

            if (corner2 < corner4)
            {
                bigY = corner4;
                smallY = corner2;
            }
            else
            {
                bigY = corner2;
                smallY = corner4;
            }

            foreach (var agent in _gameManager.AgentsInGame)
            {
                if ((agent.transform.position.x >= smallX && agent.transform.position.x <= bigX) && 
                    (agent.transform.position.y >= smallY && agent.transform.position.y <= bigY))
                {
                    _activeAgentsList.Add(agent);
                }
            }
            _state = PlayerStates.Rest;
        }
    }
    // Inputs
    private void OnMove(InputAction.CallbackContext context) 
    {
        MovementInput = context.ReadValue<Vector2>();
        //Debug.Log("MovePlayer");
    }
    private void OnRotate(InputAction.CallbackContext context)
    {
        _rotateInput = context.ReadValue<Vector2>();
        //Debug.Log("RotateInput");
    }
    private void OnButtonSouth(InputAction.CallbackContext context) 
    {
        _southButtonInput = context.ReadValueAsButton();
        SelectionArea(_southButtonInput);
        //ToggleSelection(_southButtonInput);
        //MoveAgent(_southButtonInput);
        //ClickIsOnNavMesh();
        //Debug.Log("SouthPlayer");
    }
    private void OnButtonWest(InputAction.CallbackContext context) 
    {
        _westButtonInput = context.ReadValueAsButton();
        ResetCamPosition();
        //Debug.Log("WestPlayer");
    }
    private void OnButtonNorth(InputAction.CallbackContext context) 
    {
        Debug.Log("NorthPlayer");
    }
    private void OnButtonEast(InputAction.CallbackContext context) 
    {
        Debug.Log("EastPlayer");
    }
    private void OnShoulderR(InputAction.CallbackContext context) 
    {
        _RSInput = context.ReadValueAsButton(); 
        //Debug.Log("ShoulderRPlayer");
    }
    private void OnShoulderL(InputAction.CallbackContext context) 
    {
        _LSInput = context.ReadValueAsButton();  
        //Debug.Log("ShoulderLPlayer");
    }
    private void OnStartButton(InputAction.CallbackContext context) 
    {
        Debug.Log("StartPlayer");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AgentScript>())
        {
            _objectsInTrigger.Add(other);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<AgentScript>())
        {
            _objectsInTrigger.Remove(other);
        }
    }
}
