using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Cinemachine;

public enum GameState
{
    Debug = 0,
    InMenu = 1,
    InGame = 2,
    InEditor = 3
}
public class GameManagerScript : MonoBehaviour
{
    [Header("Game Variables")]
    private Scene _activeScene;
    [SerializeField] string _activeSceneName;
    [SerializeField] int _sceneLoadedIndex;
    [SerializeField] GameObject _activeCanvas;
    [SerializeField] GameState _activeGameState;
    [SerializeField] CinemachineVirtualCamera _activeCamera;
    [SerializeField] AudioClip _currentAudioClipLoaded;
    [SerializeField] bool _audioClipPlaying;
    [SerializeField] int _score;
    [SerializeField] bool _victory;
    [SerializeField] int _totalCloth;
    [SerializeField] int _totalIron;
    [SerializeField] int _totalWood;

    [SerializeField] List<AgentScript> _agentsInGame;
    [SerializeField] List<ResourceBase> _resourcesInGame;
    [SerializeField] List<BuildingBase> _buildingsInGame;

    public event Action OnGMSetUpComplete;

    private static GameManagerScript _gameManagerInstance = null;

    void Awake()
    {
        GameManagerSingleton();
    }
    void Start()
    {
        SubscribeToEvents();
        SetUpGame();
    }

    // Getters && Setters
    public static GameManagerScript GMInstance { get { return _gameManagerInstance; } }

    // G&S States
    public Scene ActiveScene { get { return _activeScene; } set { _activeScene = value; } }
    public string ActiveSceneName { get { return _activeSceneName; } set { _activeSceneName = value; } }
    public int SceneLoadedIndex { get { return _sceneLoadedIndex; } set { _sceneLoadedIndex = value; } }
    public GameObject ActiveCanvas { get { return _activeCanvas; } set { _activeCanvas = value; } }
    public GameState ActiveGameState { get { return _activeGameState; } set { _activeGameState = value; } }
    public CinemachineVirtualCamera ActiveCamera {get { return _activeCamera; } set { _activeCamera = value; } }
    public AudioClip CurrentAudioClipLoaded { get { return _currentAudioClipLoaded; } set { _currentAudioClipLoaded = value; } }
    public bool AudioClipPlaying { get { return _audioClipPlaying; } set { _audioClipPlaying = value; } }
    public int Score { get { return _score; } set { _score = value; } }
    public bool Victory { get { return _victory; } set { _victory = value; } }
    
    public List<AgentScript> AgentsInGame { get { return _agentsInGame; } set { _agentsInGame = value; } }
    public List<ResourceBase> ResourcesInGame { get { return _resourcesInGame; } set { _resourcesInGame = value; } }
    public List<BuildingBase> BuildingsInGame { get { return _buildingsInGame; } set { _buildingsInGame = value; } }

    public int TotalCloth { get { return _totalCloth; } set { _totalCloth = value; } }
    public int TotalIron { get { return _totalIron; } set { _totalIron = value; } }
    public int TotalWood { get { return _totalWood; } set { _totalWood = value; } }

    // Methods
    private void GameManagerSingleton()
    {
        if (_gameManagerInstance == null)
        {
            _gameManagerInstance = this;
        }
        else if (_gameManagerInstance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    private void SubscribeToEvents()
    {
        SceneManager.sceneLoaded -= SetUpGame;
        SceneManager.sceneLoaded += SetUpGame;
    }
    private void SetUpGame()
    {
        ActiveScene = SceneManager.GetActiveScene();
        ActiveSceneName = SceneManager.GetActiveScene().name;
        SceneLoadedIndex = SceneManager.GetActiveScene().buildIndex;
        SetGameState();
        TotalCloth = 0;
        TotalIron = 0;
        TotalWood = 0;
        _victory = false;
        OnGMSetUpComplete?.Invoke();
        //Debug.Log("GameManager SetUp");
    }
    private void SetUpGame(Scene scene, LoadSceneMode mode)
    {
        ActiveScene = SceneManager.GetActiveScene();
        ActiveSceneName = SceneManager.GetActiveScene().name;
        SceneLoadedIndex = SceneManager.GetActiveScene().buildIndex;
        SetGameState();
        OnGMSetUpComplete?.Invoke();
    }
    public void SetGameState()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            ActiveGameState = GameState.Debug;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            ActiveGameState = GameState.InMenu;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            ActiveGameState = GameState.InMenu;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            ActiveGameState = GameState.InEditor;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4 ||
                 SceneManager.GetActiveScene().buildIndex == 5 ||
                 SceneManager.GetActiveScene().buildIndex == 6)
        {
            ActiveGameState = GameState.InGame;
        }
        else if (SceneManager.GetActiveScene().buildIndex == 7)
        {
            ActiveGameState = GameState.InMenu;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void GameManagerDebugLog()
    {
        Debug.Log("SceneManager_ActiveSceneName: " + SceneManager.GetActiveScene().name);
        Debug.Log("GM_ActiveSceneName: " + ActiveSceneName);
        Debug.Log("GM_ActiveCanvas: " + ActiveCanvas);
        Debug.Log("GM_GameState: " + ActiveGameState);
        Debug.Log("GM_ClipLoaded: " + CurrentAudioClipLoaded);
        Debug.Log("GM_AudioclipPlaying: " + AudioClipPlaying);
    }
    public void ChangeScore(int modifier)
    {
        Score += modifier;
    }
    public void ResetScore()
    {
        Score = 0;
    }
}

