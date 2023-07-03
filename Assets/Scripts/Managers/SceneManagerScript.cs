        using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class SceneManagerScript : MonoBehaviour
{
    Scene _activeScene;
    
    private static SceneManagerScript _SceneManagerInstance;

    // Main
    private void Awake()
    {
        SceneManagerSingleton();
    }

    // Getters & Setters
    public static SceneManagerScript SMInstance { get { return _SceneManagerInstance; } }
    public Scene ActiveScene { get { return _activeScene; } set { _activeScene = value; } }

    // Methods
    private void SceneManagerSingleton()
    {
        if (_SceneManagerInstance == null)
        {
            _SceneManagerInstance = this;
        }
        else if (_SceneManagerInstance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

}
