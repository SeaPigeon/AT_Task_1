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
    [SerializeField] float _fireDelay;
    [SerializeField] string _sceneToLoadOnDeath;
    [SerializeField] GameObject _firePoint;
    [SerializeField] GameObject _activeWeapon;
    [SerializeField] GameObject[] _weaponList;
    [SerializeField] SpriteRenderer _weaponSprite;
    [SerializeField] bool _hasRedKeycard;
    [SerializeField] bool _hasBlueKeycard;
    [SerializeField] bool _hasYellowKeycard;
    [SerializeField] CinemachineVirtualCamera _gameCam;

    private int _activeWeaponIndex;
    private float _lastBulletTime;

    [Header("PlayerInput")]
    [SerializeField] CharacterController _playerCC;
    [SerializeField] Vector2 _movementInput;
    [SerializeField] Vector2 _rotateInput;
    [SerializeField] bool _fireInput;
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
    [SerializeField] bool _isStrafing;

    // G&S
    public static PlayerScript PlayerInstance { get { return _playerInstance; } }
    public Vector2 MovementInput { get { return _movementInput; } set { _movementInput = value; } }
    public Vector2 RotateInput { get { return _rotateInput; } set { _rotateInput = value; } }
    public bool FireInput { get { return _fireInput; } set { _fireInput = value; } }
    public float FireDelay { get { return _fireDelay; } set { _fireDelay = value; } }
    public GameObject[] WeaponList { get { return _weaponList; } }
    //public Transform SpawnPoint { get { return _spawnPoint; } set { _spawnPoint = value; } }
    public bool HasRedKeycard { get { return _hasRedKeycard; } set { _hasRedKeycard = value; } }
    public bool HasBlueKeycard { get { return _hasBlueKeycard; } set { _hasBlueKeycard = value; } }
    public bool HasYellowKeycard { get { return _hasYellowKeycard; } set { _hasYellowKeycard = value; } }
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
        Fire(_fireInput);
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
        _activeWeapon = _weaponList[0].gameObject;
        _activeWeaponIndex = 0;
        _fireDelay = _weaponList[0].GetComponent<BulletScript>().FireDelay;
        ActivateWeapon(0);
        _isStrafing = false;
        _hasRedKeycard = false;
        _hasBlueKeycard = false;
        _hasYellowKeycard = false;
        _lastBulletTime = Time.time;
        LinkUI();
        /*FindSpawnPoint();
        if (SceneManager.GetActiveScene().buildIndex == 4 ||
            SceneManager.GetActiveScene().buildIndex == 5 ||
            SceneManager.GetActiveScene().buildIndex == 6)
        {

            SpawnPlayer();
        }
        else
        {
            TogglePlayerSprite(false);
        }*/
    }
    public void ResetPlayer() 
    {
        CurrentHealth = _MAX_HEALTH;
        _gameManager.ResetScore();
        _gameManager.Victory = false;
        SetUpStartingAmmo(10, 0, 0);
    }
    public void TogglePlayerSprite(bool state)
    {
        _playerSprite.enabled = state;
    }
    public void MoveToSpawnPoint(Vector3 pos)
    {
        transform.position = new Vector3(pos.x, pos.y, pos.z);
        /*if (_spawnPoint != null)
        {
            transform.position = new Vector3(pos.x, pos.y, pos.z);


            gameObject.transform.rotation = new Quaternion(pos.x,
                                                           pos.y,
                                                           pos.z,
                                                           pos.w);
        }
        else
        {
            Debug.Log("Spawn Error");
        }*/
        Debug.Log("Player Spawned from GMEvent: " + transform.position);
    }
    public void SpawnPlayer(Vector3 pos)
    {
        TogglePlayerSprite(true);
        MoveToSpawnPoint(pos);
    }
    /*private void FindSpawnPoint()
    {
        _spawnPoint = FindObjectOfType<SpawnPointScript>().transform;
    }*/

    // Gameplay
    private void Move(Vector2 input)
    {
        if (_isStrafing && (input.x != 0 || input.y != 0))
        {
            Strafe(input);
        }
        else
        {
            _moveVector = Vector3.zero;
            _appliedMoveVector = Vector3.zero;
            _moveVector.z = input.y * _moveSpeed;

            _appliedMoveVector = transform.TransformDirection(_moveVector);
            _playerCC.Move(_appliedMoveVector * Time.deltaTime);
            gameObject.transform.Rotate(new Vector3(0, input.x * _rotationSpeed * Time.deltaTime, 0));
        }
        

    }
    private void Strafe(Vector2 input)
    {
        _moveVector.x = input.x * _moveSpeed;
        _moveVector.z = input.y * _moveSpeed;
        
        _appliedMoveVector = transform.TransformDirection(_moveVector);
        _playerCC.Move(_appliedMoveVector * Time.deltaTime);
    } 
    private void Fire(bool input)
    {
        if (_activeWeapon.GetComponent<BulletScript>().Ammo > 0)
        {
            if (input && Time.time >= _lastBulletTime + _fireDelay)
            {
                _lastBulletTime = Time.time;
                Instantiate(_activeWeapon, _firePoint.transform.position, _firePoint.transform.rotation);
                _activeWeapon.GetComponent<BulletScript>().Ammo -= 1;
                _UILinker.AmmoTextUI.text = _activeWeapon.GetComponent<BulletScript>().Ammo.ToString();
                //PlaySoundOnFire();
                Debug.Log("pew pew");
            }
        }
        else
        {
            AutoSwapWeapon(); 
        }
        
    }
    private void SwapWeapon(int step)
    {
        var newWeaponIndex = _activeWeaponIndex + step;
        if (newWeaponIndex > _weaponList.Length -1)
        {
            newWeaponIndex = 0;
        }
        else if (newWeaponIndex < 0)
        {
            newWeaponIndex = _weaponList.Length -1;
        }
        ActivateWeapon(newWeaponIndex);
    }
    private void AutoSwapWeapon()
    {
        var count = 0;
        for (int i = 0; i < _weaponList.Length; i++)
        {
            if (_weaponList[i].GetComponent<BulletScript>().Ammo > 0)
            {
                ActivateWeapon(i);
            }
            else
            {
                count += 1;
            }
        }
        if (count == 3)
        {
            Debug.Log("NoAmmo");
        }
    }
    public void ActivateWeapon(int index)
    {
        if (_weaponList[index].GetComponent<BulletScript>().Ammo > 0)
        {
            _activeWeapon = _weaponList[index];
            _activeWeaponIndex = index;
            _fireDelay = _weaponList[index].GetComponent<BulletScript>().FireDelay;
            _weaponSprite.sprite = _weaponList[index].GetComponent<BulletScript>().WeaponSprite;
            LinkUI();
        }
    }
    private void SetUpStartingAmmo(int w1, int w2, int w3)
    {
        _weaponList[0].GetComponent<BulletScript>().Ammo = w1;
        _weaponList[1].GetComponent<BulletScript>().Ammo = w2;
        _weaponList[2].GetComponent<BulletScript>().Ammo = w3;
    }
    private void LinkUI()
    {
        _UILinker.HealthTextUI.text = _currentHealth.ToString();
        _UILinker.AmmoTextUI.text = _activeWeapon.GetComponent<BulletScript>().Ammo.ToString();
        _UILinker.ScoreTextUI.text = _gameManager.Score.ToString();
        if (_activeWeapon == _weaponList[0])
        {
            _UILinker.WeaponTextUI.text = "Blaster";
            _UILinker.ActiveWeaponUI.sprite = _weaponList[0].GetComponent<BulletScript>().WeaponSprite;
        }
        else if (_activeWeapon == _weaponList[1])
        {
            _UILinker.WeaponTextUI.text = "AR";
            _UILinker.ActiveWeaponUI.sprite = _weaponList[1].GetComponent<BulletScript>().WeaponSprite;
        }
        else if (_activeWeapon == _weaponList[2])
        {
            _UILinker.WeaponTextUI.text = "Cannon";
            _UILinker.ActiveWeaponUI.sprite = _weaponList[2].GetComponent<BulletScript>().WeaponSprite;

        }
    }
    public void TakeDamage(int damage)
    {
        _audioManager.PlayPlayerHurtSFX();
        _currentHealth -= damage;
        _UILinker.HealthTextUI.text = _currentHealth.ToString();

        if (CurrentHealth <= 0)
        {
            TogglePlayerSprite(false);
            _sceneManager.LoadScene(_sceneToLoadOnDeath);
            _UILinker.ScoreEndScreenUI.text = _gameManager.Score.ToString();
        }
    }

    // Inputs
    private void OnMove(InputAction.CallbackContext context) 
    {
        MovementInput = context.ReadValue<Vector2>();
        Debug.Log("MovePlayer");
    }
    private void OnButtonSouth(InputAction.CallbackContext context) 
    {
        _fireInput = context.ReadValueAsButton();

        Debug.Log("SouthPlayer");
    }
    private void OnButtonWest(InputAction.CallbackContext context) 
    {
        _westButtonInput = context.ReadValueAsButton();
        _isStrafing = _westButtonInput;
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
      
        if (_RSInput)
        {
            SwapWeapon(1);
        }
        
        Debug.Log("ShoulderRPlayer");
    }
    private void OnShoulderL(InputAction.CallbackContext context) 
    {
        _LSInput = context.ReadValueAsButton();
 
        if (_LSInput)
        {
            SwapWeapon(-1);
        }
        
        Debug.Log("ShoulderLPlayer");
    }
    private void OnStartButton(InputAction.CallbackContext context) 
    {
        Debug.Log("StartPlayer");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyScript>() && other.GetComponent<EnemyScript>().CurrentEnemyState != EnemyState.Dead)
        {
            TakeDamage(other.GetComponent<EnemyScript>().MDamage);
        }
        if (other.GetComponent<EnemyBullet>())
        {
            TakeDamage(other.GetComponent<EnemyBullet>().RDamage);
        }
    }
}
