using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPointScript : MonoBehaviour
{
    private static SpawnPointScript _SpawnPointInstance;
    
    private GameManagerScript _gameManager;
    private PlayerScript _player;
    
    // G&S
    public static SpawnPointScript SPInstance { get { return _SpawnPointInstance; } }

    private void Awake()
    {
        SpawnPointSingleton();    
    }

    private void Start()
    {
        SetUpReferences();
        SubscribeToEvents();
    }
    private void SpawnPointSingleton()
    {
        if (_SpawnPointInstance == null)
        {
            _SpawnPointInstance = this;
        }
        else if (_SpawnPointInstance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    private void SetUpReferences()
    {
        _gameManager = GameManagerScript.GMInstance;
        _player = PlayerScript.PlayerInstance;
    }
    private void SubscribeToEvents()
    {
        _gameManager.OnGMSetUpComplete -= MoveSpawnPoint;

        _gameManager.OnGMSetUpComplete += MoveSpawnPoint;
    }

    private void MoveSpawnPoint()
    {
        Vector3 newPos = Vector3.zero;
        switch (SceneManager.GetActiveScene().buildIndex)
        {
            case 4:
                newPos = new Vector3(-1, 0, -1);
                Debug.Log("new pos: 1");
                break;
            case 5:
                newPos = new Vector3(0, 0, -1);
                Debug.Log("new pos: 2");
                break;
            case 6:
                newPos = new Vector3(0, 0, -1);
                Debug.Log("new pos: 3");
                break;
            default:
                newPos = new Vector3(10, 10, 10);
                //_player.TogglePlayerSprite(false);
                Debug.Log("MoveSP scene index default case: " + SceneManager.GetActiveScene().buildIndex);
                break;
        }
        Debug.Log("1: " + transform.position);
        transform.position = newPos;
        Debug.Log("2: " + transform.position);
        _player.SpawnPlayer(transform.position);
        Debug.Log("3: " + transform.position);
    }
}
