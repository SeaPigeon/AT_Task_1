using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManagerScript : MonoBehaviour
{
    [Header("Playlists")]
    [SerializeField] private AudioClip[] _gameMusic;
    [SerializeField] private AudioClip[] _gameSFX;

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
    public AudioClip[] GameMusicList {get { return _gameMusic; } }
    public AudioClip[] GameSFXList { get { return _gameSFX; } }
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

                    PlayMusic(0);
                    _audioSourceInstance.time = 5;

                    break;
                case GameState.InGame:
                    StopMusic();
                    break;
                case GameState.InEditor:
                    PlayMusic(1);
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
    public void PlayMusic(int clipIndex)
    {
        if ((!_audioSourceInstance.isPlaying) || 
             (_audioSourceInstance.isPlaying &&
              _audioSourceInstance.clip != _gameMusic[clipIndex]))
        {
            _audioSourceInstance.Stop();

            _audioSourceInstance.clip = _gameMusic[clipIndex];
            _currentAudioClipLoaded = _gameMusic[clipIndex];

            _audioSourceInstance.Play();
            _audioClipPlaying = _audioSourceInstance.isPlaying;

            _gameManager.CurrentAudioClipLoaded = _currentAudioClipLoaded;
            _gameManager.AudioClipPlaying = _audioClipPlaying;
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
    public void PlaySFX(int clipIndex)
    {
        AudioSource.PlayClipAtPoint(_gameSFX[clipIndex], CameraManagerScript.CMInstance.ActiveCamera.transform.position);
    }
    public void PlayHealSFX()
    {
        AudioSource.PlayClipAtPoint(_gameSFX[0], CameraManagerScript.CMInstance.ActiveCamera.transform.position);
    }
    public void PlayPlayerHurtSFX()
    {
        AudioSource.PlayClipAtPoint(_gameSFX[1], CameraManagerScript.CMInstance.ActiveCamera.transform.position);
    }
    public void PlayWeaponBSFX()
    {
        AudioSource.PlayClipAtPoint(_gameSFX[2], CameraManagerScript.CMInstance.ActiveCamera.transform.position);
    }
    public void PlayWeapon2SFX()
    {
        AudioSource.PlayClipAtPoint(_gameSFX[3], CameraManagerScript.CMInstance.ActiveCamera.transform.position);
    }
    public void PlayWeapon3SFX()
    {
        AudioSource.PlayClipAtPoint(_gameSFX[4], CameraManagerScript.CMInstance.ActiveCamera.transform.position);
    }
    public void PlayReloadSFX()
    {
        AudioSource.PlayClipAtPoint(_gameSFX[5], CameraManagerScript.CMInstance.ActiveCamera.transform.position);
    }
    public void PlayEnemyAtkSFX()
    {
        AudioSource.PlayClipAtPoint(_gameSFX[6], CameraManagerScript.CMInstance.ActiveCamera.transform.position);
    }
    public void PlayEnemyBaseTakeDamageSFX()
    {
        AudioSource.PlayClipAtPoint(_gameSFX[7], CameraManagerScript.CMInstance.ActiveCamera.transform.position);
    }
    public void PlayEnemyStrongTakeDamageSFX()
    {
        AudioSource.PlayClipAtPoint(_gameSFX[8], CameraManagerScript.CMInstance.ActiveCamera.transform.position);
    }
    public void PlayBossTakeDamageSFX()
    {
        AudioSource.PlayClipAtPoint(_gameSFX[9], CameraManagerScript.CMInstance.ActiveCamera.transform.position);
    }
    public void PlayDoorSFX()
    {
        AudioSource.PlayClipAtPoint(_gameSFX[10], CameraManagerScript.CMInstance.ActiveCamera.transform.position);
    }
    public void PlayKeyCardSFX()
    {
        AudioSource.PlayClipAtPoint(_gameSFX[11], CameraManagerScript.CMInstance.ActiveCamera.transform.position);
    }
    public void PlayVictorySFX()
    {
        AudioSource.PlayClipAtPoint(_gameSFX[12], CameraManagerScript.CMInstance.ActiveCamera.transform.position);
    }
    public void PlayDefeatSFX()
    {
        AudioSource.PlayClipAtPoint(_gameSFX[13], CameraManagerScript.CMInstance.ActiveCamera.transform.position);
    }
}

