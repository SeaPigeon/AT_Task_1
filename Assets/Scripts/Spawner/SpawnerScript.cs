using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawnerScript : MonoBehaviour
{
    [Header("Spawner Variables")]
    [SerializeField] private int _MAX_NUMBER_OF_AGENTS = 5;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] float _spawnTime = 10;

    [Header("Debug")]
    [SerializeField] private GameManagerScript _gameManager;
    [SerializeField] private int _currentAgentsInScene;
    void Start()
    {
        SetReferences();
        SetUpSpawner();
        StartCoroutine(SpawnAgent());
    }

    private void SetReferences()
    {
        _gameManager = GameManagerScript.GMInstance;
    }

    private void SetUpSpawner()
    {
        _currentAgentsInScene = _gameManager.AgentsInGame.Count;
    }

    private IEnumerator SpawnAgent()
    {

        /*while (true)
        {
            if (_currentAgentsInScene < _MAX_NUMBER_OF_AGENTS)
            {
                Instantiate(_prefab, _spawnPoint.position, _spawnPoint.rotation);
                _currentAgentsInScene = _gameManager.AgentsInGame.Count;
                Debug.Log(_gameManager.AgentsInGame.Count);
            }

            yield return new WaitForSeconds(_spawnTime);

            if (_currentAgentsInScene < _MAX_NUMBER_OF_AGENTS)
            {
                StartCoroutine(SpawnAgent());
                yield break;
            }
        }*/
        while (true)
        {
            if (_currentAgentsInScene < _MAX_NUMBER_OF_AGENTS)
            {
                Instantiate(_prefab, _spawnPoint.position, _spawnPoint.rotation);
                _currentAgentsInScene = _gameManager.AgentsInGame.Count;
                Debug.Log(_gameManager.AgentsInGame.Count);
                yield return new WaitForSeconds(_spawnTime);
            }
            else
            {
                // Check if the agent count falls below the maximum
                while (_currentAgentsInScene >= _MAX_NUMBER_OF_AGENTS)
                {
                    _currentAgentsInScene = _gameManager.AgentsInGame.Count;
                }
            }
        }
    }
    
}
