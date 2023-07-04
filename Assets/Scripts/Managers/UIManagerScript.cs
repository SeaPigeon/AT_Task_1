using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class UIManagerScript : MonoBehaviour
{
    [Header("UI Variables")]
    [SerializeField] GameObject[] _canvasList;
    
    [Header("Debug")]
    [SerializeField] GameObject _activeCanvas;
    [SerializeField] private GameManagerScript _gameManager;
    [SerializeField] private EventSystem _eventSystem;

    private static UIManagerScript _UIManagerInstance = null;

    void Awake()
    {
        UIManagerSingleton();    
    }
    private void Start()
    {
        SetUpReferences();
        SubscribeToEvents();
    }

    // Getter & Setters
    public static UIManagerScript UIMInstance { get { return _UIManagerInstance; } }
    public GameObject ActiveCanvas { get { return _activeCanvas; } set { _activeCanvas = value; } }
   
   // Methods
    private void UIManagerSingleton()
    {
        if (_UIManagerInstance == null)
        {
            _UIManagerInstance = this;
        }
        else if (_UIManagerInstance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }   
    private void SetUpReferences() 
    {
        _gameManager = GameManagerScript.GMInstance;
        _eventSystem = gameObject.GetComponentInChildren<EventSystem>();
    }
    private void SubscribeToEvents()
    {
        _gameManager.OnGMSetUpComplete -= SetUpStartingCanvas;
        _gameManager.OnGMSetUpComplete += SetUpStartingCanvas;
    }
    
    public void SetUpStartingCanvas()
    {
        foreach (GameObject item in _canvasList)
        {
            item.SetActive(false);
        }
 
        if (_gameManager.SceneLoadedIndex == 0)
        {
            LoadCanvas(0);
        }
        else if (_gameManager.SceneLoadedIndex == 1)
        {
            LoadCanvas(1);
        }
        else if (_gameManager.SceneLoadedIndex == 2)
        {
            LoadCanvas(2);
        }
        else if (_gameManager.SceneLoadedIndex == 3)
        {
            LoadCanvas(3);
        }
        else if (_gameManager.SceneLoadedIndex == 4 ||
                 _gameManager.SceneLoadedIndex == 5 ||
                 _gameManager.SceneLoadedIndex == 6)
        {
            LoadCanvas(4);
        }
        else if (_gameManager.SceneLoadedIndex == 7)
        {
            LoadCanvas(5);
        }
        //Debug.Log("UI Manager Canvas Loaded from GMEvent: " + _activeCanvas.name);
    }
    public void SetUpEventSystem()
    {
        _eventSystem.SetSelectedGameObject(ActiveCanvas.GetComponent<RectTransform>().GetChild(1).gameObject);
        Debug.Log("call setup" + ActiveCanvas.GetComponent<RectTransform>().GetChild(1).gameObject.name);
    }   

    // Public Methods
    public void LoadCanvas(int canvasIndex)
    {
        if (ActiveCanvas != null) 
        {
            ActiveCanvas.SetActive(false);
        }
        _canvasList[canvasIndex].SetActive(true);
        ActiveCanvas = _canvasList[canvasIndex];
        SetUpEventSystem();
        _gameManager.ActiveCanvas = ActiveCanvas;
    }

}
