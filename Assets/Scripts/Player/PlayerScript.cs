using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] static int _MAX_HEALTH = 100;
    [SerializeField] int _currentHealth;
    [SerializeField] float _moveSpeed;
    [SerializeField] float _runSpeed;
    [SerializeField] float _rotationSpeed;
    [SerializeField] CinemachineVirtualCamera _gameCam;

    [Header("PlayerInput")]
    [SerializeField] CharacterController _playerCC;
    [SerializeField] Vector2 _movementInput;
    [SerializeField] Vector2 _rotateInput;
    [SerializeField] bool southButtonInput;
    [SerializeField] bool _RSInput;
    [SerializeField] bool _LSInput;
    [SerializeField] bool _westButtonInput;
    private Vector3 _moveVector;
    private Vector3 _appliedMoveVector;

    [Header("Debug")]
    private static PlayerScript _playerInstance;
    [SerializeField] GameManagerScript _gameManager;
    [SerializeField] SceneManagerScript _sceneManager;
    [SerializeField] InputManagerScript _inputManager;
    [SerializeField] LinkUIScript _UILinker;
    [SerializeField] AudioManagerScript _audioManager;
    [SerializeField] SpriteRenderer _playerSprite;
    [SerializeField] Transform _spawnPoint;

    // G&S
    public static PlayerScript PlayerInstance { get { return _playerInstance; } }
    public Vector2 MovementInput { get { return _movementInput; } set { _movementInput = value; } }
    public Vector2 RotateInput { get { return _rotateInput; } set { _rotateInput = value; } }
    public bool FireInput { get { return southButtonInput; } set { southButtonInput = value; } }
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
    }

    // G&S
    public int CurrentHealth { get { return _currentHealth; } set { _currentHealth = value; } }

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
        _playerSprite = GetComponentInChildren<SpriteRenderer>();
        _playerCC = gameObject.GetComponent<CharacterController>();
    }
    private void SubscribeToEvents()
    {
        _gameManager.OnGMSetUpComplete -= SetUpPlayer;
        _gameManager.OnGMSetUpComplete += SetUpPlayer;
    }
    public void SubscribeGameInputs()
    {
        Debug.Log("Call");
        // UNSUB
        _inputManager.InputMap.Game.Move.performed -= OnMove;
        _inputManager.InputMap.Game.ButtonSouth.started -= OnButtonSouth;
        _inputManager.InputMap.Game.ButtonWest.started -= OnButtonWest;
        _inputManager.InputMap.Game.ShoulderR.started -= OnShoulderR;
        _inputManager.InputMap.Game.ShoulderL.started -= OnShoulderL;

        _inputManager.InputMap.Game.Move.canceled -= OnMove;
        _inputManager.InputMap.Game.ButtonSouth.canceled -= OnButtonSouth;
        _inputManager.InputMap.Game.ButtonWest.canceled -= OnButtonWest;
        _inputManager.InputMap.Game.ShoulderR.canceled -= OnShoulderR;
        _inputManager.InputMap.Game.ShoulderL.canceled -= OnShoulderL;

        // SUB
        _inputManager.InputMap.Game.Move.performed += OnMove;
        _inputManager.InputMap.Game.ButtonSouth.started += OnButtonSouth;
        _inputManager.InputMap.Game.ButtonWest.started += OnButtonWest;
        //_inputManager.InputMap.Game.ButtonNorth.performed += OnButtonNorth;
        //_inputManager.InputMap.Game.ButtonEast.performed += OnButtonEast;
        _inputManager.InputMap.Game.ShoulderR.started += OnShoulderR;
        _inputManager.InputMap.Game.ShoulderL.started += OnShoulderL;
        //_inputManager.InputMap.Game.StartButton.performed += OnStartButton;

        _inputManager.InputMap.Game.Move.canceled += OnMove;
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
        LinkUI();
        if (SceneManager.GetActiveScene().buildIndex == 4 ||
            SceneManager.GetActiveScene().buildIndex == 5 ||
            SceneManager.GetActiveScene().buildIndex == 6)
        {

            SpawnPlayer(Vector3.zero);
        }
        else
        {
            TogglePlayerSprite(false);
        }
    }
    public void ResetPlayer() 
    {
        CurrentHealth = _MAX_HEALTH;
        _gameManager.ResetScore();
        _gameManager.Victory = false;
    }
    public void TogglePlayerSprite(bool state)
    {
        if (state != _playerSprite.enabled)
        {
            _playerSprite.enabled = state;
        }
    }
    public void MoveToSpawnPoint(Vector3 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, pos.z);
       
        Debug.Log("Player Spawned from GMEvent: " + transform.position);
    }
    public void SpawnPlayer(Vector3 pos)
    {
        TogglePlayerSprite(true);
        MoveToSpawnPoint(pos);
    }

    // Gameplay
    private void Move(Vector2 input)
    {
        _moveVector = Vector3.zero;
        _appliedMoveVector = Vector3.zero;
        _moveVector.z = input.y * _moveSpeed;

        _appliedMoveVector = transform.TransformDirection(_moveVector);
        _playerCC.Move(_appliedMoveVector * Time.deltaTime);
        gameObject.transform.Rotate(new Vector3(0, input.x * _rotationSpeed * Time.deltaTime, 0));
    }
    
    private void LinkUI()
    {
        Debug.Log("LinkUI Function Call!");
    }

    // Inputs
    private void OnMove(InputAction.CallbackContext context) 
    {
        MovementInput = context.ReadValue<Vector2>();
        Debug.Log("MovePlayer");
    }
    private void OnButtonSouth(InputAction.CallbackContext context) 
    {
        southButtonInput = context.ReadValueAsButton();
        Debug.Log("SouthPlayer");
    }
    private void OnButtonWest(InputAction.CallbackContext context) 
    {
        _westButtonInput = context.ReadValueAsButton();;
        Debug.Log("WestPlayer");
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
        Debug.Log("ShoulderRPlayer");
    }
    private void OnShoulderL(InputAction.CallbackContext context) 
    {
        _LSInput = context.ReadValueAsButton();  
        Debug.Log("ShoulderLPlayer");
    }
    private void OnStartButton(InputAction.CallbackContext context) 
    {
        Debug.Log("StartPlayer");
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player Collision: " + other.name);
    }
}
