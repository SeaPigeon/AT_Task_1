using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManagerScript : MonoBehaviour
{
    [SerializeField] private ParticleSystem[] _particlesList;

    private static ParticleManagerScript _ParticleManagerInstance;

    // G&S 
    public ParticleManagerScript PMInstance { get { return _ParticleManagerInstance; } }
    public ParticleSystem[] ParticlesList { get { return _particlesList; } }

    void Awake()
    {
        ParticleManagerSingleton();
    }

    private void ParticleManagerSingleton()
    {
        if (_ParticleManagerInstance == null)
        {
            _ParticleManagerInstance = this;
        }
        else if (_ParticleManagerInstance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void PlayPE(Vector3 position, int particleIndex)
    {
        ParticleSystem instance;
        instance = Instantiate(_particlesList[particleIndex], position, Quaternion.identity);
        instance.Play();
        Destroy(instance.gameObject, instance.main.duration);
    }

    public void PlayShootPE(Vector3 position)
    {
        PlayPE(position, 0);
    }
    public void PlayTakeDamageMonsterPE(Vector3 position)
    {
        PlayPE(position, 0);
    }
    /*public void PlayShootPE(Vector3 position)
    {
        _particlesList[0].Play();
    }
    public void PlayShootPE(Vector3 position)
    {
        _particlesList[0].Play();
    }*/

}
