using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManagerScript : MonoBehaviour
{
    private static InputManagerScript _inputManagerInstance = null;

    [SerializeField] private NewInputMap _inputMap;
    [SerializeField] private InputActionMap _activeInputMap;
    [SerializeField] private GameManagerScript _gameManager;

    [SerializeField] Vector2 _dPadInput;
    [SerializeField] bool _buttonSouthInput;
    [SerializeField] bool _buttonWestInput;
    [SerializeField] bool _buttonNorthInput;
    [SerializeField] bool _buttonEastInput;
    [SerializeField] bool _shoulderRInput;
    [SerializeField] bool _shoulderLInput;
    [SerializeField] bool _startInput;
    
    private void Awake()
    {
        InputManagerSingleton();
    }

    private void Start()
    {
        SetUpReferences();
        SubscribeToEvents();
    }

    // Getters & Setters
    public static InputManagerScript IMInstance { get { return _inputManagerInstance; } }
    public Vector2 DPadInput { get { return _dPadInput; } set { _dPadInput = value; } }
    public bool ButtonSouthInput { get { return _buttonSouthInput; } set { _buttonSouthInput = value; } }
    public bool ButtonWestInput { get { return _buttonWestInput; } set { _buttonWestInput = value; } }
    public bool ButtonNorthInput { get { return _buttonNorthInput; } set { _buttonNorthInput = value; } }
    public bool ButtonEastInput { get { return _buttonEastInput; } set { _buttonEastInput = value; } }
    public bool ShoulderRInput { get { return _shoulderRInput; } set { _shoulderRInput = value; } }
    public bool ShoulderLInput { get { return _shoulderLInput; } set { _shoulderLInput = value; } }
    public bool StartInput { get { return _startInput; } set { _startInput = value; } }
    public NewInputMap InputMap {get { return _inputMap; } }
    public InputActionMap ActiveInputMap { get { return _activeInputMap; } set { _activeInputMap = value; } }

    // Methods
    private void InputManagerSingleton()
    {
        if (_inputManagerInstance == null)
        {
            _inputManagerInstance = this;
        }
        else if (_inputManagerInstance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    private void SetUpReferences() 
    {
        _gameManager = GameManagerScript.GMInstance;
    }
    private void SubscribeToEvents()
    {
        _gameManager.OnGMSetUpComplete -= SetUpStartingInputMap;

        _gameManager.OnGMSetUpComplete += SetUpStartingInputMap;
    }
    public void SetUpStartingInputMap()
    {
        if (_inputMap == null)
        {
            _inputMap = new NewInputMap();
            SubscribeUIInputs();
            PlayerScript.PlayerInstance.SubscribeGameInputs();
        }

        if (_gameManager.SceneLoadedIndex == 4 ||
            _gameManager.SceneLoadedIndex == 5 ||
            _gameManager.SceneLoadedIndex == 6) 
        {
            ActivateInputMap(_inputMap.Game);
        }
        else
        {
            ActivateInputMap(_inputMap.UI);
        }
        //Debug.Log("InputManager Call from GMEvent: " + _activeInputMap);
    }
    public void SubscribeUIInputs()
    {
        // UNSUB
        _inputMap.UI.Move.started -= OnMove;
        
        _inputMap.UI.ButtonWest.performed -= OnButtonWest;
        _inputMap.UI.ButtonNorth.performed -= OnButtonNorth;
        _inputMap.UI.ButtonEast.performed -= OnButtonEast;
        _inputMap.UI.ShoulderR.performed -= OnShoulderR;
        _inputMap.UI.ShoulderL.performed -= OnShoulderL;
        _inputMap.UI.StartButton.performed -= OnStartButton;

        _inputMap.UI.ButtonSouth.canceled -= OnButtonSouth;

        // SUB
        _inputMap.UI.Move.started += OnMove;
        //_inputMap.UI.ButtonSouth.started += OnButtonSouth;
        _inputMap.UI.ButtonWest.performed += OnButtonWest;
        _inputMap.UI.ButtonNorth.performed += OnButtonNorth;
        _inputMap.UI.ButtonEast.performed += OnButtonEast;
        _inputMap.UI.ShoulderR.performed += OnShoulderR;
        _inputMap.UI.ShoulderL.performed += OnShoulderL;
        _inputMap.UI.StartButton.performed += OnStartButton;

        //_inputMap.UI.Move.canceled += OnMove;
        _inputMap.UI.ButtonSouth.canceled += OnButtonSouth;
        //_inputMap.UI.ButtonWest.canceled += OnButtonWest;
        //_inputMap.UI.ButtonNorth.canceled += OnButtonNorth;
        //_inputMap.UI.ButtonEast.canceled += OnButtonEast;
        //_inputMap.UI.ShoulderR.canceled += OnShoulderR;
        //_inputMap.UI.ShoulderL.canceled += OnShoulderL;
        //_inputMap.UI.StartButton.canceled += OnStartButton;
    }
    public void ActivateInputMap(InputActionMap map)
    {
        _inputMap.Disable();
        map.Enable();
        _activeInputMap = map;
    }

    // UI Inputs Functions
    private void OnMove(InputAction.CallbackContext context) 
    {
        //DPadInput = context.ReadValue<Vector2>();
        Debug.Log("DPadUI Input Log");
    }
    private void OnButtonSouth(InputAction.CallbackContext context) 
    {
        Debug.Log("SouthUI Input Log");
    }
    private void OnButtonWest(InputAction.CallbackContext context) 
    {
        //ButtonWestInput = context.ReadValueAsButton();
        Debug.Log("WestUI");
    }
    private void OnButtonNorth(InputAction.CallbackContext context) 
    {
        //ButtonNorthInput = context.ReadValueAsButton();
        Debug.Log("NorthUI");
    }
    private void OnButtonEast(InputAction.CallbackContext context) 
    {
        //ButtonEastInput = context.ReadValueAsButton();
        Debug.Log("EastUI");
    }
    private void OnShoulderR(InputAction.CallbackContext context) 
    {
        //ShoulderRInput = context.ReadValueAsButton();
        Debug.Log("SRUI");
    }
    private void OnShoulderL(InputAction.CallbackContext context) 
    {
        //ShoulderLInput = context.ReadValueAsButton();
        Debug.Log("SLUI");
    }
    private void OnStartButton(InputAction.CallbackContext context) 
    {
        //StartInput = context.ReadValueAsButton();
        Debug.Log("StartUI");
    }     
}
