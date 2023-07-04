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
    public void LoadDebugScene()
    {
        LoadScene("DebugScene");
    }
    public void LoadMainMenuScene()
    {
        LoadScene("MainMenu");
    }
    public void LoadControlMenuScene()
    {
        LoadScene("ControlsMenu");
    }
    public void LoadEditorScene()
    {
        LoadScene("LevelEditor");
    }
    public void LoadLevel1Scene()
    {
        LoadScene("Level_1");
        PlayerScript.PlayerInstance.ResetPlayer();
    }
    public void LoadLevel2Scene()
    {
        LoadScene("Level_2");
    }
    public void LoadLevel3Scene()
    {
        LoadScene("Level_3");
    }
    public void LoadEndGameScreen()
    {
        LoadScene("EndGameScene");
    }

}
