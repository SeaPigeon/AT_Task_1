using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using Cinemachine;
using Random = UnityEngine.Random;

public enum GameState
{
    Debug = 0,
    InMenu = 1,
    InGame = 2,
    InEditor = 3
}
public class GameManagerScript : MonoBehaviour
{
    [Header("System Variables")]
    private Scene _activeScene;
    [SerializeField] string _activeSceneName;
    [SerializeField] int _sceneLoadedIndex;
    [SerializeField] GameObject _activeCanvas;
    [SerializeField] GameState _activeGameState;
    [SerializeField] CinemachineVirtualCamera _activeCamera;
    [SerializeField] AudioClip _currentAudioClipLoaded;
    [SerializeField] bool _audioClipPlaying;

    [Header("Game Variables")]
    [SerializeField] int _timeBeforeHunger = 15;
    [SerializeField] int _timeBetweenHungerProcs = 15;
    [SerializeField] int _timeBeforeWaves = 30;
    [SerializeField] int _timeBetweenWaves = 30;
    [SerializeField] int _score;
    [SerializeField] bool _victory;
    [SerializeField] int _totalFood;
    [SerializeField] int _totalRocks;
    [SerializeField] int _totalWood;
    [SerializeField] float _delayBeforeFirstSpawn = 10;
    [SerializeField] float _spawnTime = 30;

    [SerializeField] List<AgentScript> _agentsInGame;
    [SerializeField] List<ResourceBase> _resourcesInGame;
    [SerializeField] List<BuildingBase> _buildingsInGame;
    [SerializeField] List<EnemyScript> _enemiesInGame;
    [SerializeField] List<SpawnerScript> _spawnersInGame;

    [Header("Debug")]
    [SerializeField] GameObject _agentPrefab;
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] int _waveCount;
    [SerializeField] int _MAX_NUMBER_OF_AGENTS = 10;
    [SerializeField] int _currentAgentsInScene;

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
    public List<EnemyScript> EnemiesInGame { get { return _enemiesInGame; } set { _enemiesInGame = value; } }
    public List<SpawnerScript> SpawnersInGame { get { return _spawnersInGame; } set { _spawnersInGame = value; } }

    public int TotalFood { get { return _totalFood; } set { _totalFood = value; } }
    public int TotalRocks { get { return _totalRocks; } set { _totalRocks = value; } }
    public int TotalWood { get { return _totalWood; } set { _totalWood = value; } }
    public int WaveCount { get { return _waveCount; } set { _waveCount = value; } }

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
        _totalFood = 4;
        _totalRocks = 2;
        _totalWood = 2;
        _waveCount = 0;
        _currentAgentsInScene = 0;
        _victory = false;
        OnGMSetUpComplete?.Invoke();
        if (_activeGameState == GameState.InGame)
        {
            StartCoroutine(Hunger());
            StartCoroutine(WaveSpawner());
            StartCoroutine(SpawnAgents());
        }
        //Debug.Log("GameManager SetUp");
    }
    private void SetUpGame(Scene scene, LoadSceneMode mode)
    {
        ActiveScene = SceneManager.GetActiveScene();
        ActiveSceneName = SceneManager.GetActiveScene().name;
        SceneLoadedIndex = SceneManager.GetActiveScene().buildIndex;
        SetGameState();
        _totalFood = 4;
        _totalRocks = 2;
        _totalWood = 2;
        _waveCount = 0;
        _currentAgentsInScene = 0;
        _victory = false;
        /*_agentsInGame.Clear();
        _resourcesInGame.Clear();
        _buildingsInGame.Clear();
        _enemiesInGame.Clear();
        _spawnersInGame.Clear();*/
        OnGMSetUpComplete?.Invoke();
        if (_activeGameState == GameState.InGame)
        {
            StartCoroutine(Hunger());
            StartCoroutine(WaveSpawner());
            StartCoroutine(SpawnAgents());
        }
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
    private IEnumerator Hunger()
    {
        yield return new WaitForSeconds(_timeBeforeHunger);
        Debug.Log("Hunger Started");
        while (!_victory)
        {
            if (_victory)
            {
                yield break;
            }
            if (_agentsInGame.Count == 0)
            {
                _victory = true;
                _agentsInGame.Clear();
                _resourcesInGame.Clear();
                _buildingsInGame.Clear();
                _enemiesInGame.Clear();
                _spawnersInGame.Clear();
                UIManagerScript.UIMInstance.GetComponent<LinkUIScript>().ScoreEndScreenUI.text = _score.ToString();
                SceneManagerScript.SMInstance.LoadEndGameScreen();
                yield break;
            }

            yield return new WaitForSeconds(_timeBetweenHungerProcs);

            if (_totalFood >= _agentsInGame.Count)
            {
                Debug.Log("Feeding");
                _totalFood -= _agentsInGame.Count;
            }
            else
            {
                int randomNumber = Random.Range(0, _agentsInGame.Count);
                AgentScript agentToRemove = _agentsInGame[randomNumber];

                _agentsInGame[randomNumber].StopAgentCoroutine(_agentsInGame[randomNumber].ActiveCoR);
                _agentsInGame.RemoveAt(randomNumber);
                Debug.Log("Bye Bye " + agentToRemove.name);
                Destroy(agentToRemove.gameObject);
                _totalFood = 0;
            }
        }
    }
    private IEnumerator WaveSpawner()
    {
        Vector3 spawnPosition = new Vector3(0, 1, 0);
        Quaternion spawnRotation = Quaternion.identity;
        _waveCount++;
        yield return new WaitForSeconds(_timeBeforeWaves);
        Debug.Log("Spawner Started");
        while (!_victory)
        {
            if (_victory)
            {
                yield break;
            }
            for (int i = 0; i < _waveCount; i++)
            {

                Instantiate(_enemyPrefab, spawnPosition, spawnRotation);
     
            }
            UIManagerScript.UIMInstance.GetComponent<LinkUIScript>().EmotionTextUI.text = "Wave " + _waveCount.ToString() + " Incoming";
            _waveCount++;
            /*if (_waveCount > 5)
            {
                _timeBetweenWaves -= 2;
            }*/
            yield return new WaitForSeconds(_timeBetweenWaves);
        }
    }
    private IEnumerator SpawnAgents()
    {
        yield return new WaitForSeconds(_delayBeforeFirstSpawn);
        while (_currentAgentsInScene < _MAX_NUMBER_OF_AGENTS)
        {
            if (_victory)
            {
                yield break;
            }
            int randomIndex = 0;
            randomIndex = Random.Range(0, _spawnersInGame.Count);
            Instantiate(_agentPrefab, _spawnersInGame[randomIndex].transform.position, _spawnersInGame[randomIndex].transform.rotation);
            _currentAgentsInScene++;
            //PlayerScript.PlayerInstance.EmotionTextHandler(3, "A NEW LIFE IS BORN!", "...");
            yield return new WaitForSeconds(_spawnTime);
        }
    }
}

