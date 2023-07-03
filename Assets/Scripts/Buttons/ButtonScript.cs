using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ButtonScript : MonoBehaviour
{
    private InputManagerScript _inputManager;
    private SceneManagerScript _sceneManager;
    
    private void Start()
    {
        SetUpReferences();
    }

    // Methods
    private void SetUpReferences()
    {
        _sceneManager = SceneManagerScript.SMInstance.GetComponent<SceneManagerScript>();
        _inputManager = InputManagerScript.IMInstance.GetComponent<InputManagerScript>();
    }
    
    // Public Functions
    public void ActivateUIInputMap()
    {
        _inputManager.OnInputMapEvent(_inputManager.InputMap.UI);
    }
    public void ActivateGameInputMap()
    {
        _inputManager.OnInputMapEvent(_inputManager.InputMap.Game);
    }
    
    public void LoadDebugScene()
    {
        _sceneManager.LoadScene("DebugScene");
    }
    public void LoadMainMenuScene()
    {
        _sceneManager.LoadScene("MainMenu");
    }
    public void LoadControlMenuScene()
    {
        _sceneManager.LoadScene("ControlsMenu");
    }
    public void LoadEditorScene()
    {
        _sceneManager.LoadScene("LevelEditor");
    }
    public void LoadLevel1Scene()
    {
        _sceneManager.LoadScene("Level_1");
        PlayerScript.PlayerInstance.ResetPlayer();
    }
    public void LoadLevel2Scene()
    {
        _sceneManager.LoadScene("Level_2");
    }
    public void LoadLevel3Scene()
    {
        _sceneManager.LoadScene("Level_3");
    }
    public void LoadEndGameScreen()
    {
        _sceneManager.LoadScene("EndGameScene");
    }
}
