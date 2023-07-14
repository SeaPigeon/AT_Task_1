using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManagerScript : MonoBehaviour
{
    [Header("Music")]
    [SerializeField] private AudioClip _menuMusic;
    [SerializeField] private AudioClip _editorMusic;
    [SerializeField] private AudioClip _gameMusic;

    [Header("SFX")]
    [SerializeField] private AudioClip _reapingSFX;
    [SerializeField] private AudioClip _hatchetSFX;
    [SerializeField] private AudioClip _miningSFX;
    [SerializeField] private AudioClip _waveSFX;
    [SerializeField] private AudioClip _attackSFX;
    [SerializeField] private AudioClip _smithingSFX;
    [SerializeField] private AudioClip _spawnSFX;
    [SerializeField] private AudioClip _victorySFX;
    [SerializeField] private AudioClip _defeatSFX;

    [Header("Debug")]
    [SerializeField] private GameManagerScript _gameManager;
    [SerializeField] private AudioClip _currentAudioClipLoaded;
    [SerializeField] private bool _audioClipPlaying;
    [SerializeField] private AudioSource _audioSourceInstance;

    private static AudioManagerScript _audioManagerInstance = null;

    [SerializeField] private GameState _currentGameState = GameState.Debug;

    void Awake()
    {
        AudioManagerSingleton();
    }

    private void Start()
    {
        SetUpReferences();
        SetUpEvents();
    }

    // Getters & Setters
    public static AudioManagerScript AMInstance { get { return _audioManagerInstance; } } 
    public AudioSource AudioSourceInstance { get { return _audioSourceInstance; } set { _audioSourceInstance = value; } }
    public AudioClip CurrentAudioClipLoaded { get { return _currentAudioClipLoaded; } set { _currentAudioClipLoaded = value; } }
    public bool AudioClipPlaying { get { return _audioClipPlaying; } set { _audioClipPlaying = value; } }

    // Methods
    private void AudioManagerSingleton()
    {
        if (_audioManagerInstance == null)
        {
            _audioManagerInstance = this;
        }
        else if (_audioManagerInstance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    private void SetUpReferences()
    {
        _gameManager = GameManagerScript.GMInstance;
        _audioSourceInstance = CameraManagerScript.CMInstance.GetComponentInChildren<Camera>().GetComponent<AudioSource>();
    }
    private void SetUpEvents()
    {
        _gameManager.OnGMSetUpComplete -= SetUpAudioManager;
        _gameManager.OnGMSetUpComplete += SetUpAudioManager;
    }
    private void SetUpAudioManager()
    {
        if (_gameManager.ActiveGameState != _currentGameState )
        {
            switch (_gameManager.ActiveGameState)
            {
                case GameState.Debug:
                    StopMusic();
                    break;
                case GameState.InMenu:
                    VictoryOrDefeatSFX();
                    PlayMusic(_menuMusic);
                    _audioSourceInstance.time = 5;
                    break;
                case GameState.InGame:
                    PlayMusic(_gameMusic);
                    break;
                case GameState.InEditor:
                    PlayMusic(_editorMusic);
                    _audioSourceInstance.time = 3;
                    break;
                default:
                    StopMusic();
                    Debug.Log("MUSIC ERROR");
                    break;
            }
            _currentGameState = _gameManager.ActiveGameState;
        } 
    }

    // Music
    public void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            if ((!_audioSourceInstance.isPlaying) ||
                         (_audioSourceInstance.isPlaying &&
                          _audioSourceInstance.clip != clip))
            {
                _audioSourceInstance.Stop();

                _audioSourceInstance.clip = clip;
                _currentAudioClipLoaded = clip;

                _audioSourceInstance.Play();
                _audioClipPlaying = _audioSourceInstance.isPlaying;

                _gameManager.CurrentAudioClipLoaded = _currentAudioClipLoaded;
                _gameManager.AudioClipPlaying = _audioClipPlaying;
            }
        }
    }
    public void StopMusic()
    {
        _audioSourceInstance.Stop();

        _audioSourceInstance.clip = null;
        CurrentAudioClipLoaded = null;

        _audioClipPlaying = _audioSourceInstance.isPlaying;

        _gameManager.AudioClipPlaying = _currentAudioClipLoaded;
        _gameManager.AudioClipPlaying = _audioClipPlaying;
    }

    // SFX
    private void VictoryOrDefeatSFX()
    {
        if (SceneManager.GetActiveScene().buildIndex == 7)
        {
            if (_gameManager.Victory)
            {
                PlayVictorySFX();
            }
            else
            {
                PlayDefeatSFX();
            }
        }
    }
    public void PlaySFX(AudioClip clip)
    {
        AudioSource.PlayClipAtPoint(clip, CameraManagerScript.CMInstance.ActiveCamera.transform.position);
    }
    public void PlayGatherSFX()
    {
        PlaySFX(_reapingSFX);
    }
    public void PlayMiningSFX()
    {
        PlaySFX(_miningSFX);
    }
    public void PlayWoodCuttingSFX()
    {
        PlaySFX(_hatchetSFX);
    }
    public void PlayAttackSFX()
    {
        PlaySFX(_attackSFX);
    }
    public void PlayWaveSpawnSFX()
    {
        PlaySFX(_waveSFX);
    }
    public void PlaySmithingSFX()
    {
        PlaySFX(_smithingSFX);
    }
    public void PlaySpawnSFX()
    {
        PlaySFX(_spawnSFX);
    }
    public void PlayVictorySFX()
    {
        PlaySFX(_victorySFX);
    }
    public void PlayDefeatSFX()
    {
        PlaySFX(_defeatSFX);
    }
}

