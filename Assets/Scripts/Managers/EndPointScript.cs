using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Scenes
{
    Debug,
    MainMenu,
    ControlsMenu,
    Editor,
    Level_1,
    Level_2,
    Level_3,
    EndScreen
}
public class EndPointScript : MonoBehaviour
{
    [SerializeField] private Scenes _sceneToLoad;
    private SceneManagerScript _sceneManager;

    void Start()
    {
        SetUpReferences();
    }
    private void SetUpReferences()
    {
        _sceneManager = SceneManagerScript.SMInstance.GetComponent<SceneManagerScript>();
    }

    private void LoadScene()
    {
        switch (_sceneToLoad)
        {
            case Scenes.Debug:
                LoadDebugScene();
                break;

            case Scenes.MainMenu:
                LoadMainMenuScene();
                break;

            case Scenes.ControlsMenu:
                LoadControlMenuScene();
                break;

            case Scenes.Editor:
                LoadEditorScene();
                break;

            case Scenes.Level_1:
                LoadLevel1Scene();
                break;

            case Scenes.Level_2:
                LoadLevel2Scene();
                break;

            case Scenes.Level_3:
                LoadLevel3Scene();
                break;
            case Scenes.EndScreen:
                LoadEndGameScreen();
                break;
            default:
                break;
        }
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerScript>())
        {
            LoadScene();
        }
    }
}
