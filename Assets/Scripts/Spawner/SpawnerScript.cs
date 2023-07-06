using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public enum SpawnerState
{
    Spawning,
    Waiting
}
public class SpawnerScript : MonoBehaviour
{
    [Header("Spawner Variables")]
    [SerializeField] private int _MAX_NUMBER_OF_AGENTS = 5;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] float _spawnTime = 10;

    [Header("Debug")]
    [SerializeField] private SpawnerState _state;
    [SerializeField] private GameManagerScript _gameManager;
    [SerializeField] private int _currentAgentsInScene;
    private bool _isWaiting = false;
    private float _waitTimer;

    private Action CountCheckEvent;
    void Start()
    {
        SetReferences();
        SetUpSpawner();
        StartCoroutine(SpawnAgent());
    }
    private void Update()
    {
        switch (_state)
        {
            case SpawnerState.Waiting:
                if (_currentAgentsInScene < _MAX_NUMBER_OF_AGENTS)
                {
                    _state = SpawnerState.Spawning;
                    StartCoroutine(SpawnAgent());
                }
                break;

            default:
                Debug.Log("Spawner Error");
                break;
        }
    }
    private void SetReferences()
    {
        _gameManager = GameManagerScript.GMInstance;
    }

    private void SetUpSpawner()
    {
        _currentAgentsInScene = _gameManager.AgentsInGame.Count;
        _state = SpawnerState.Spawning;
    }

    private IEnumerator SpawnAgent()
    {
        while (_currentAgentsInScene < _MAX_NUMBER_OF_AGENTS)
        {
            yield return new WaitForSeconds(_spawnTime);

            Instantiate(_prefab, _spawnPoint.position, _spawnPoint.rotation);
            _currentAgentsInScene++;
            Debug.Log(_currentAgentsInScene);
        }

        _state = SpawnerState.Waiting;
    }

}
