using System.Linq;
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
    Selecting,
    HoldingSelection
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
    [SerializeField] float _minAngleV;
    [SerializeField] float _maxAngleV;
    [SerializeField] Sprite _baseCrosshairSprite;
    [SerializeField] Sprite _selectedCrosshairSprite;

    [Header("PlayerInput")]
    [SerializeField] CharacterController _playerCC;
    [SerializeField] Vector2 _movementInput;
    [SerializeField] Vector2 _rotateInput;
    [SerializeField] bool _southButtonInput;
    [SerializeField] bool _westButtonInput;
    [SerializeField] bool _eastButtonInput;
    [SerializeField] bool _RSInput;
    [SerializeField] bool _LSInput;
    
    private Vector3 _moveVector;
    private Vector3 _appliedMoveVector;

    [Header("Debug")]
    [SerializeField] PlayerStates _playerState;
    [SerializeField] bool _gravityEnabled;
    [SerializeField] List<AgentScript> _activeAgentsList;
    [SerializeField] List<AgentScript> _agentsInTrigger;
    [SerializeField] ResourceBase _resourceInTrigger;
    [SerializeField] BuildingBase _buildingInTrigger;
    [SerializeField] EnemyScript _enemyInTrigger;
    private CinemachineVirtualCamera _gameCam;
    private static PlayerScript _playerInstance;
    private GameManagerScript _gameManager;
    private SceneManagerScript _sceneManager;
    private InputManagerScript _inputManager;
    private LinkUIScript _UILinker;
    private AudioManagerScript _audioManager;
    private Transform _spawnPoint;
    private SpriteRenderer _playerSprite;
    private Transform _gameCamBody;
    private Vector3 _startingCamPos;
    private Quaternion _startingCamRot;

    private Vector3 _firstPoint;
    private Vector3 _secondPoint;

    private float _bigX = 0;
    private float _smallX = 0;
    private float _bigY = 0;
    private float _smallY = 0;
 
    private Coroutine _activeCor;
    private GameObject _selectionAreaObject = null;

    // G&S
    public static PlayerScript PlayerInstance { get { return _playerInstance; } }
    public Vector2 MovementInput { get { return _movementInput; } set { _movementInput = value; } }
    public Vector2 RotateInput { get { return _rotateInput; } set { _rotateInput = value; } }
    public bool FireInput { get { return _southButtonInput; } set { _southButtonInput = value; } }
    public CinemachineVirtualCamera InGameCamera { get { return _gameCam; } }
    public List<AgentScript> ActiveAgentsList { get { return _activeAgentsList; } }
    public Coroutine ActiveCoR { get { return _activeCor; } set { _activeCor = value; } }

    // Main
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
        if (_gravityEnabled)
        {
            ApplyGravity();
        }
        ResetSelection(_eastButtonInput);
        LinkUI();
        if (_playerState == PlayerStates.Selecting)
        {
            DrawSelectionArea();
        }
    }
    private void LateUpdate()
    {
        RotateH(RotateInput);
        RotateV(RotateInput);
    }

    // G&S
    public Vector3 CursorPosition { get { return transform.position; } }

    // Essentials
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
        _playerSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
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
        _inputManager.InputMap.Game.ButtonWest.performed -= OnButtonWest;
        _inputManager.InputMap.Game.ButtonEast.started -= OnButtonEast;
        _inputManager.InputMap.Game.ShoulderR.started -= OnShoulderR;
        _inputManager.InputMap.Game.ShoulderL.started -= OnShoulderL;

        _inputManager.InputMap.Game.Move.canceled -= OnMove;
        _inputManager.InputMap.Game.Rotate.canceled -= OnRotate;
        _inputManager.InputMap.Game.ButtonSouth.canceled -= OnButtonSouth;
        _inputManager.InputMap.Game.ButtonWest.canceled -= OnButtonWest;
        _inputManager.InputMap.Game.ButtonEast.canceled -= OnButtonEast;
        _inputManager.InputMap.Game.ShoulderR.canceled -= OnShoulderR;
        _inputManager.InputMap.Game.ShoulderL.canceled -= OnShoulderL;

        // SUB
        _inputManager.InputMap.Game.Move.performed += OnMove;
        _inputManager.InputMap.Game.Rotate.performed += OnRotate;
        _inputManager.InputMap.Game.ButtonSouth.started += OnButtonSouth;
        _inputManager.InputMap.Game.ButtonWest.performed += OnButtonWest;
        //_inputManager.InputMap.Game.ButtonNorth.performed += OnButtonNorth;
        _inputManager.InputMap.Game.ButtonEast.performed += OnButtonEast;
        _inputManager.InputMap.Game.ShoulderR.started += OnShoulderR;
        _inputManager.InputMap.Game.ShoulderL.started += OnShoulderL;
        //_inputManager.InputMap.Game.StartButton.performed += OnStartButton;

        _inputManager.InputMap.Game.Move.canceled += OnMove;
        _inputManager.InputMap.Game.Rotate.canceled += OnRotate;
        _inputManager.InputMap.Game.ButtonSouth.canceled += OnButtonSouth;
        _inputManager.InputMap.Game.ButtonWest.canceled += OnButtonWest;
        //_inputManager.InputMap.Game.ButtonNorth.canceled += OnButtonNorth;
        _inputManager.InputMap.Game.ButtonEast.canceled += OnButtonEast;
        _inputManager.InputMap.Game.ShoulderR.canceled += OnShoulderR;
        _inputManager.InputMap.Game.ShoulderL.canceled += OnShoulderL;
        //_inputManager.InputMap.Game.StartButton.canceled += OnStartButton;
    }
    private void SetUpPlayer()
    {
        _playerState = PlayerStates.Rest;
        _playerSprite.sprite = _baseCrosshairSprite;
        _startingCamPos = _gameCamBody.transform.localPosition;
        _startingCamRot = _gameCamBody.localRotation;
        _activeCor = null;
        _UILinker.ScoreTextUI.text = _gameManager.Score.ToString();
        _UILinker.EmotionTextUI.text = "...";

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

    // Player Spawn
    public void ResetPlayer() 
    {
        _gameManager.ResetScore();
        _gameManager.Victory = false;
        _activeAgentsList.Clear();
        
        /*_gameManager.TotalFood = 4;
        _gameManager.TotalRocks = 2;
        _gameManager.TotalWood = 2;
        _gameManager.WaveCount = 0;*/
    }
    public void TogglePlayerMesh(bool state)
    {
        if (state != _playerSprite.enabled)
        {
            _playerSprite.enabled = state;
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

    // PlayerUI
    private void LinkUI()
    {
        _UILinker.AgentCountTextUI.text = _gameManager.AgentsInGame.Count.ToString();
        if (_activeAgentsList.Count > 1)
        {
            _UILinker.FoodTextUI.text = SumFoodValues().ToString();
            _UILinker.RocksTextUI.text = SumRocksValues().ToString();
            _UILinker.WoodTextUI.text = SumWoodValues().ToString();
            _UILinker.HealthTextUI.text = SumHealthValues(_activeAgentsList).ToString();

        }
        else if (_activeAgentsList.Count == 1)
        {
            if (_activeAgentsList != null)
            {
                _UILinker.FoodTextUI.text = _activeAgentsList.FirstOrDefault().CarriedFood.ToString();
                _UILinker.RocksTextUI.text = _activeAgentsList.FirstOrDefault().CarriedRocks.ToString();
                _UILinker.WoodTextUI.text = _activeAgentsList.FirstOrDefault().CarriedWood.ToString();
                _UILinker.HealthTextUI.text = _activeAgentsList.FirstOrDefault().CurrentHealth.ToString();
            }
            
        }
        else
        {
            _UILinker.FoodTextUI.text = _gameManager.TotalFood.ToString();
            _UILinker.RocksTextUI.text = _gameManager.TotalRocks.ToString();
            _UILinker.WoodTextUI.text = _gameManager.TotalWood.ToString();
            _UILinker.HealthTextUI.text = SumHealthValues(_gameManager.AgentsInGame).ToString();
        }
    }
    private int SumHealthValues(List<AgentScript> list)
    {
        int total = 0;
        foreach (var agent in list)
        {
            total += agent.CurrentHealth;
        }
        return total;
    }
    private int SumFoodValues()
    {
        int total = 0;
        foreach (var agent in _activeAgentsList)
        {
            total += agent.CarriedFood;
        }
        return total;
    }
    private int SumRocksValues()
    {
        int total = 0;
        foreach (var agent in _activeAgentsList)
        {
            total += agent.CarriedRocks;
        }
        return total;
    }
    private int SumWoodValues()
    {
        int total = 0;
        foreach (var agent in _activeAgentsList)
        {
            total += agent.CarriedWood;
        }
        return total;
    }

    // Player Movement
    private void Move(Vector2 input)
    {
        _moveVector.x = input.x * _moveSpeed;
        _moveVector.z = input.y * _moveSpeed;

        _appliedMoveVector = transform.TransformDirection(_moveVector);
        _playerCC.Move(_appliedMoveVector * Time.deltaTime);
    }
    private void RotateH(Vector2 input)
    {
        transform.Rotate(new Vector3(0, input.x * _rotationSpeed * Time.deltaTime, 0));
    }
    private void RotateV(Vector2 input)
    {
        float rotationXOverTime = _gameCamBody.transform.eulerAngles.x - input.y * _rotationSpeed * Time.deltaTime;
        float rotationX = Mathf.Clamp(rotationXOverTime, _minAngleV, _maxAngleV);

        _gameCamBody.transform.rotation = Quaternion.Euler(rotationX, _gameCamBody.transform.eulerAngles.y, _gameCamBody.transform.eulerAngles.z);
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
    private void ResetCamPosition(bool input)
    {
        if (input)
        {
            _gameCamBody.transform.localPosition = _startingCamPos;
            _gameCamBody.transform.localRotation = _startingCamRot;
        }
    }
    private void ApplyGravity()
    {
        if (!_playerCC.isGrounded)
        {
            _playerCC.Move(Vector3.down* 9.81f * Time.deltaTime);
        }
    }
    
    // Selection General
    private void ToggleAgentSelection(AgentScript agent)
    {
        agent.StopAgentCoroutine(agent.ActiveCoR);
        agent.ActiveCoR = null;

        switch (agent.ActiveAgentState)
        {
            case AgentState.Inactive:
                break;
            case AgentState.Idle:
                agent.StopIdle();
                break;
            case AgentState.Moving:
                break;
            case AgentState.Combat:
                agent.StopCombat();
                break;
            case AgentState.Building:
                break;
            case AgentState.Gathering:
                agent.ResourceToInteractWith.StopGathering(agent);
                break;
            case AgentState.Interacting:
                agent.BuildingToInteractWith.StopInteracting(agent);
                break;
            case AgentState.Selected:
                break;
            case AgentState.Knight:
                break;
            default:
                break;
        }
        //agent.EnableAgentUI(0);

        if (agent.ActiveAgentState != AgentState.Selected)
        {
            AddAgentToSelection(agent);
            StopActiveCoR();
            _activeCor = StartCoroutine(EmotionTextHandler(3, "Something to doing", "..."));
        }
        else if (agent.ActiveAgentState == AgentState.Selected)
        {
            RemoveAgentFromSelection(agent);
        }
    }
    private void AddAgentToSelection(AgentScript agent)
    {
        _activeAgentsList.Add(agent);
        agent.ActiveAgentState = AgentState.Selected;
        agent.StopAgent();
    }
    public void RemoveAgentFromSelection(AgentScript agent)
    {
        _activeAgentsList.Remove(agent.GetComponent<AgentScript>());
        agent.GetComponent<AgentScript>().ActiveAgentState = AgentState.Inactive;
    }
    private void ResetSelection(bool input)
    {
        if (input)
        {
            var copy = new AgentScript[_activeAgentsList.Count];
            _activeAgentsList.CopyTo(copy, 0);

            foreach (var agent in copy)
            {
                RemoveAgentFromSelection(agent);
            }
            _activeAgentsList.Clear();
            HasAgentsInSelection();
        }
    }
    public void HasAgentsInSelection()
    {
        if (_activeAgentsList.Count >= 1)
        {
            _playerState = PlayerStates.HoldingSelection;
        }
        else
        {
            _playerState = PlayerStates.Rest;
        }
    }
    /*
     * float distance = 0;
     * for each vertex in vertex list
     *      for each vertex2 in vertex list
     *      float newDistance = Vector3.GetDistance(vertex, vertex2)
     *      if(newDistance > distance)
     *      distance = newDistance;
     * 
     * figure out the point between the two furdest points
     * 
     * figure out vertex order to determinate sides
     * 
     * form triangles with the new point generated and the 2 point of each side
     */
    // Selection AoE
    private void StartSelectionArea()
    {
        _firstPoint = transform.position;
        Debug.Log(_firstPoint);
        _playerSprite.sprite = _selectedCrosshairSprite;
        CreateMesh();
        _playerState = PlayerStates.Selecting;
    }
    private void StopSelectionArea()
    {

        foreach (var agent in _gameManager.AgentsInGame)
        {
            if ((agent.transform.position.x >= _smallX && agent.transform.position.x <= _bigX) &&
                (agent.transform.position.z >= _smallY && agent.transform.position.z <= _bigY))
            {
                ToggleAgentSelection(agent);
            }
        }

        _playerSprite.sprite = _baseCrosshairSprite;
        HasAgentsInSelection();
        DestroyMesh();
    }
    private void DrawSelectionArea() 
    {
        _secondPoint = transform.position;
        SortPoints();

        Mesh mesh = _selectionAreaObject.GetComponent<MeshFilter>().mesh;
        
        Vector3 v1 = new Vector3(_smallX, 0, _smallY);
        Vector3 v2 = new Vector3(_smallX, 0, _bigY);
        Vector3 v3 = new Vector3(_bigX, 0, _bigY);
        Vector3 v4 = new Vector3(_bigX, 0, _smallY);

        Vector3[] vertices = new Vector3[4];
        vertices[0] = v1;
        vertices[1] = v2;
        vertices[2] = v3;
        vertices[3] = v4;
        mesh.vertices = vertices;
        /*foreach (var item in mesh.vertices)
        {
            Debug.Log(item);
        }*/
        
        int[] triangles = new int[6] {
            0, 1, 2, 
            2, 3, 0  
        };
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
    private void CreateMesh()
    {
        _selectionAreaObject = new GameObject();
        _selectionAreaObject.transform.position = transform.position;
        _selectionAreaObject.transform.rotation = transform.rotation;
        Debug.Log("AoE Pos: " + _selectionAreaObject.transform.position);
        MeshFilter meshFilter = _selectionAreaObject.AddComponent<MeshFilter>();
        meshFilter.mesh = new Mesh();
        MeshRenderer meshRenderer = _selectionAreaObject.AddComponent<MeshRenderer>();
        Material material = new Material(Shader.Find("Standard"));
        material.color = Color.yellow;
        meshRenderer.sharedMaterial = material;
    }
    private void DestroyMesh()
    {
        if (_selectionAreaObject != null)
        {
            Destroy(_selectionAreaObject);
            _selectionAreaObject = null;
        }
    }
    private void SortPoints()
    {
        if (_firstPoint.x < _secondPoint.x)
        {
            _bigX = _secondPoint.x;
            _smallX = _firstPoint.x;
        }
        else
        {
            _bigX = _firstPoint.x;
            _smallX = _secondPoint.x;
        }

        if (_firstPoint.z < _secondPoint.z)
        {
            _bigY = _secondPoint.z;
            _smallY = _firstPoint.z;
        }
        else
        {
            _bigY = _firstPoint.z;
            _smallY = _secondPoint.z;
        }
    }

    // Selection Move
    private void MoveAgent(AgentScript agent, Vector3 pos)
    {
        agent.HasMoveTarget = false;
        agent.MoveTargetPosition = pos;
        agent.ActiveAgentState = AgentState.Moving;
    }
    private bool SelectedPositionIsOnNavMesh()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (NavMesh.SamplePosition(hit.point, out _, 0.1f, NavMesh.AllAreas))
            {
                return true;
            } 
        }
        return false;        
    }
    
    // Selection Task
    private void AssignTask()
    {
        foreach (var agent in _activeAgentsList)
        {
            agent.ResetAgent();

            Vector3 posToMove = Vector3.zero;

            agent.MovingTowardsInteractable = true;
            
            if (_resourceInTrigger != null)
            {
                agent.ResourceToInteractWith = _resourceInTrigger;
                posToMove = _resourceInTrigger.GatherPoint.transform.position;
                switch (_resourceInTrigger.ResourceType)
                {
                    case ResourceType.Corn:
                        StopActiveCoR();
                        _activeCor = StartCoroutine(EmotionTextHandler(3, "It's time to get sweaty in the field!", "..."));
                        AudioManagerScript.AMInstance.PlayGatherSFX();
                        break;
                    case ResourceType.Rock:
                        StopActiveCoR();
                        _activeCor = StartCoroutine(EmotionTextHandler(3, "Heigh-Hoooooo!", "..."));
                        AudioManagerScript.AMInstance.PlayMiningSFX();
                        break;
                    case ResourceType.Wood:
                        StopActiveCoR();
                        _activeCor = StartCoroutine(EmotionTextHandler(3, "Watch out for splinters!", "..."));
                        AudioManagerScript.AMInstance.PlayWoodCuttingSFX();
                        break;
                    default:
                        break;
                }
            }
            else if (_buildingInTrigger != null)
            {
                agent.BuildingToInteractWith = _buildingInTrigger;
                posToMove = _buildingInTrigger.InteractPoint.transform.position;
                switch (_buildingInTrigger.BuildingType)
                {
                    case BuildingType.Armory:
                        StopActiveCoR();
                        _activeCor = StartCoroutine(EmotionTextHandler(3, "For the peace of the village", "..."));
                        AudioManagerScript.AMInstance.PlaySmithingSFX();
                        break;
                    case BuildingType.Deposit:
                        StopActiveCoR();
                        _activeCor = StartCoroutine(EmotionTextHandler(3, "It's never enough", "..."));
                        break;
                    default:
                        break;
                }
            }
            else if (_enemyInTrigger != null)
            {
                agent.EnemyToAttack = _enemyInTrigger;
                posToMove = _enemyInTrigger.FightPoint.transform.position;
                StopActiveCoR();
                _activeCor = StartCoroutine(EmotionTextHandler(3, "TO THE DEATH!", "..."));
                AudioManagerScript.AMInstance.PlayAttackSFX();
            }
            
            MoveAgent(agent, posToMove);
        }
        HasAgentsInSelection();
    }
    private void StopActiveCoR()
    {
        if (_activeCor != null)
        {
            StopCoroutine(_activeCor);
        }
    }
    public IEnumerator EmotionTextHandler(int time, string string1, string string2)
    {
        _UILinker.EmotionTextUI.text = string1;
        yield return new WaitForSeconds(time);
        _UILinker.EmotionTextUI.text = string2;
    }

    // Inputs
    private void ButtonSouthBehaviour(bool input)
    {
        switch (_playerState)
        {
            case PlayerStates.Rest:
                if (input)
                {
                    if (_agentsInTrigger.Count == 0 && _activeAgentsList.Count == 0)
                    {
                        StartSelectionArea();
                    }
                    else if (_agentsInTrigger.Count > 0)
                    {
                        foreach (var agent in _agentsInTrigger)
                        {
                            ToggleAgentSelection(agent);
                        }
                        HasAgentsInSelection();
                    }
                }
                break;

            case PlayerStates.Selecting:
                if (!input)
                {
                    StopSelectionArea();
                }
                break;

            case PlayerStates.HoldingSelection:
                if (input)
                {
                    if (_resourceInTrigger != null || 
                        _buildingInTrigger != null || 
                        _enemyInTrigger != null)
                    {
                        AssignTask();
                    }
                    else if (_agentsInTrigger.Count == 0 && _activeAgentsList.Count > 0)
                    {
                        if (SelectedPositionIsOnNavMesh())
                        {
                            foreach (var agent in _activeAgentsList)
                            {
                                agent.MovingTowardsInteractable = false;
                                MoveAgent(agent, transform.position);
                            }
                        }
                    }
                    else if (_agentsInTrigger.Count > 0)
                    {
                        foreach (var agent in _agentsInTrigger)
                        {
                            ToggleAgentSelection(agent);
                        }

                        HasAgentsInSelection();
                    }
                }
                break;

            default:
                Debug.Log("Default Case Button South Behaviour");
                break;
        }
    }

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
        ButtonSouthBehaviour(_southButtonInput);
        //Debug.Log("SouthPlayer");
    }
    private void OnButtonWest(InputAction.CallbackContext context) 
    {
        _westButtonInput = context.ReadValueAsButton();
        ResetCamPosition(_westButtonInput);
        //Debug.Log("WestPlayer");
    }
    private void OnButtonNorth(InputAction.CallbackContext context) 
    {
        Debug.Log("NorthPlayer");
    }
    private void OnButtonEast(InputAction.CallbackContext context) 
    {
        _eastButtonInput = context.ReadValueAsButton();
        //Debug.Log("EastPlayer");
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

    // OnTrigger
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<AgentScript>())
        {
            _agentsInTrigger.Add(other.GetComponent<AgentScript>());
        }
        else if (other.GetComponent<ResourceBase>())
        {
            _resourceInTrigger = other.GetComponent<ResourceBase>();
        }
        else if (other.GetComponent<BuildingBase>())
        {
            _buildingInTrigger = other.GetComponent<BuildingBase>();
        }
        else if (other.GetComponent<EnemyScript>())
        {
            _enemyInTrigger = other.GetComponent<EnemyScript>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<AgentScript>())
        {
            _agentsInTrigger.Remove(other.GetComponent<AgentScript>());
        }
        else if (other.GetComponent<ResourceBase>())
        {
            _resourceInTrigger = null;
        }
        else if (other.GetComponent<BuildingBase>())
        {
            _buildingInTrigger = null;
        }
        else if (other.GetComponent<EnemyScript>())
        {
            _enemyInTrigger = null;
        }
    }
}
