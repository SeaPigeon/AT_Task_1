using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkUIScript : MonoBehaviour
{
    [Header("Variables")]
    [Header("Variables Debug Canvas")]
    [SerializeField] Sprite _debugBGSprite;
    [SerializeField] Sprite _debugTitleBGSprite;
    [SerializeField] string _debugTitleString;

    [Header("Variables Main Menu Canvas")]
    [SerializeField] Sprite _mainMenuBGSprite;
    [SerializeField] Sprite _mainMenuTitleBGSprite;
    [SerializeField] string _mainMenuTitleString;

    [Header("Variables Controls Menu Canvas")]
    [SerializeField] Sprite _controlsBGSprite;
    [SerializeField] Sprite _controlsTitleSprite;
    [SerializeField] string _controlsTitleString;

    [Header("Move")]
    [SerializeField] Sprite _wButtonSprite;
    [SerializeField] Sprite _aButtonSprite;
    [SerializeField] Sprite _sButtonSprite;
    [SerializeField] Sprite _dButtonSprite;
    [SerializeField] Sprite _stickLSprite;
    [SerializeField] string _moveString;

    [Header("RotateCam")]
    [SerializeField] Sprite _upButtonSprite;
    [SerializeField] Sprite _downButtonSprite;
    [SerializeField] Sprite _leftButtonSprite;
    [SerializeField] Sprite _rightButtonSprite;
    [SerializeField] Sprite _stickRSprite;
    [SerializeField] string _rotateCamString;

    [Header("Interact")]
    [SerializeField] Sprite _spaceBarSprite;
    [SerializeField] Sprite _southButtonSprite;
    [SerializeField] string _interactString;

    [Header("Reset Cam")]
    [SerializeField] Sprite _shiftSprite;
    [SerializeField] Sprite _westButtonSprite;
    [SerializeField] string _resetCamString;
    
    [Header("Zoom Cam")]
    [SerializeField] Sprite _qButtonSprite;
    [SerializeField] Sprite _eButtonSprite;
    [SerializeField] Sprite _lsButtonSprite;
    [SerializeField] Sprite _rsButtonSprite;
    [SerializeField] string _zoomString;

    [Header("Reset Selection")]
    [SerializeField] Sprite _eastButtonSprite;
    [SerializeField] Sprite _ctrlButtonSprite;
    [SerializeField] string _resetSelectionString;

    [Header("Variables Editor Canvas")]
    [SerializeField] Sprite _editorBGSprite;
    [SerializeField] Sprite _editorTitleBGSprite;
    [SerializeField] string _editorTitleString;

    [Header("EndGameMenu Canvas")]
    [SerializeField] Sprite _engBGSprite;
    [SerializeField] Sprite _endTitleSprite;
    [SerializeField] string _endTitleString;
    [SerializeField] Sprite _endScoreBGSprite;

    [Header("Game Canvas")]
    [SerializeField] Sprite _gameBGSprite;
    [SerializeField] Sprite _healthSprite;
    [SerializeField] Sprite _scoreSprite;

    // Score
    [SerializeField] Sprite _scoreGameSprite;
    string _scoreString;

    // Food
    [SerializeField] Sprite _foodSprite;
    string _foodString;

    // Rocks
    [SerializeField] Sprite _rocksSprite;
    string _rocksString;

    // Wood
    [SerializeField] Sprite _woodSprite;
    string _woodString;

    // Health
    [SerializeField] Sprite _healthGameSprite;
    string _healthGameString;

    // Emotions
    [SerializeField] Sprite _emotioneSprite;
    string _emotionString;

    // Agents in Game
    [SerializeField] Sprite _agentCountSprite;
    [SerializeField] string _agentCountString;

    [Header("References")]
    [Header("References Debug Canvas")]
    [Header("--------------------------------------")]
    [SerializeField] Image _debugBGImage;
    [SerializeField] Image _debugTitleBGImage;
    [SerializeField] Text _debugTitleText;

    [Header("References Main Menu Canvas")]
    [SerializeField] Image _mainMenuBGImage;
    [SerializeField] Image _mainMenuTitleImage;
    [SerializeField] Text _mainMenuTitleText;

    [Header("References Controls Menu Canvas")]
    [SerializeField] Image _controlsBGImage;
    [SerializeField] Image _controlsTitleImage;
    [SerializeField] Text _controlsTitleText;

    [Header("Move")]
    [SerializeField] Image _wButtonImage;
    [SerializeField] Image _aButtonImage;
    [SerializeField] Image _sButtonImage;
    [SerializeField] Image _dButtonImage;
    [SerializeField] Image _stickLImage;
    [SerializeField] Text _moveText;

    [Header("Rotate Camera")]
    [SerializeField] Image _upButtonImage;
    [SerializeField] Image _downButtonImage;
    [SerializeField] Image _leftButtonImage;
    [SerializeField] Image _rightButtonImage;
    [SerializeField] Image _stickRImage;
    [SerializeField] Text _rotateCamText;

    [Header("Interact")]
    [SerializeField] Image _spaceBarImage;
    [SerializeField] Image _southButtonImage;
    [SerializeField] Text _interactText;

    [Header("Reset Camera")]
    [SerializeField] Image _shiftImage;
    [SerializeField] Image _westButtonImage;
    [SerializeField] Text _resetCamText;

    [Header("Zoom Camera")]
    [SerializeField] Image _qButtonImage;
    [SerializeField] Image _eButtonImage;
    [SerializeField] Image _lsButtonImage;
    [SerializeField] Image _rsButtonImage;
    [SerializeField] Text _zoomText;

    [Header("Reset Selection")]
    [SerializeField] Image _eastButtonImage;
    [SerializeField] Image _ctrlButtonImage;
    [SerializeField] Text _resetSelText;

    [Header("References Editor Canvas")]
    [SerializeField] Image _editorBGImage;
    [SerializeField] Image _editorTitleBGImage;
    [SerializeField] Text _editorTitleText;

    [Header("References End Game Canvas")]
    [SerializeField] Image _endBGImage;
    [SerializeField] Image _endTitleBGImage;
    [SerializeField] Text _endTitleText;
    [SerializeField] Image _endScoreBGImage;
    [SerializeField] Text _endScoreBGText;

    [Header("References Player Canvas")]
    [SerializeField] Image _gameBGImage;

    // Score
    [SerializeField] Image _scoreGameImage;
    [SerializeField] Text _scoreText;

    // Food
    [SerializeField] Image _foodImage;
    [SerializeField] Text _foodText;

    // Rocks
    [SerializeField] Image _rocksImage;
    [SerializeField] Text _rocksText;

    // Wood
    [SerializeField] Image _woodImage;
    [SerializeField] Text _woodText;

    // Health
    [SerializeField] Image _healthGameImage;
    [SerializeField] Text _healthGameText;

    // Emotions
    [SerializeField] Image _emotioneImage;
    [SerializeField] Text _emotionText;

    // Agents in Game
    [SerializeField] Image _AgentCountImage;
    [SerializeField] Text _agentCountText;

    [Header("Debug")]
    [SerializeField] GameManagerScript _gameManager;

    private void Start()
    {
        SetUpReference();
        SubscribeEvents();
    }
    private void SetUpReference()
    {
        _gameManager = GameManagerScript.GMInstance;
    }
    private void SubscribeEvents()
    {
        _gameManager.OnGMSetUpComplete -= SetUpUI;
        _gameManager.OnGMSetUpComplete += SetUpUI;
    }

    // G&S
    public Text AgentCountTextUI { get { return _agentCountText; } set { _agentCountText = value; } }
    public Text ScoreTextUI { get { return _scoreText; } set { _scoreText = value; } }
    public Text FoodTextUI { get { return _foodText; } set { _foodText = value; } }
    public Text RocksTextUI { get { return _rocksText; } set { _rocksText = value; } }
    public Text WoodTextUI { get { return _woodText; } set { _woodText = value; } }
    public Text EmotionTextUI { get { return _emotionText; } set { _emotionText = value; } }
    public Text HealthTextUI { get { return _healthGameText; } set { _healthGameText = value; } }
    public Text ScoreEndScreenUI { get { return _endScoreBGText; } set { _endScoreBGText = value; } }

    // SetUpUI
    private void SetUpDebugUI()
    {
        _debugBGImage.sprite = _debugBGSprite;
        _debugTitleBGImage.sprite = _debugTitleBGSprite;
        _debugTitleText.text = _debugTitleString;
    }
    private void SetUpMainMenuUI()
    {
        _mainMenuBGImage.sprite = _mainMenuBGSprite;
        _mainMenuTitleImage.sprite = _mainMenuTitleBGSprite;
        _mainMenuTitleText.text = _mainMenuTitleString;
    }
    private void SetUpControlsUI()
    {
        _controlsBGImage.sprite = _controlsBGSprite;
        _controlsTitleImage.sprite = _controlsTitleSprite;
        _controlsTitleText.text = _controlsTitleString;

        // Move
        _wButtonImage.sprite = _wButtonSprite;
        _aButtonImage.sprite = _aButtonSprite;
        _sButtonImage.sprite = _sButtonSprite;
        _dButtonImage.sprite = _dButtonSprite;
        _stickLImage.sprite = _stickLSprite;
        _moveText.text = _moveString;

        // Rotate Cam
        _upButtonImage.sprite = _upButtonSprite;
        _downButtonImage.sprite = _downButtonSprite;
        _leftButtonImage.sprite = _leftButtonSprite;
        _rightButtonImage.sprite = _rightButtonSprite;
        _stickRImage.sprite = _stickRSprite;
        _rotateCamText.text = _rotateCamString;

        // Interact
        _spaceBarImage.sprite = _spaceBarSprite;
        _southButtonImage.sprite = _southButtonSprite;
        _interactText.text = _interactString;

        // Reset Cam
        _shiftImage.sprite = _shiftSprite;
        _westButtonImage.sprite = _westButtonSprite;
        _resetCamText.text = _resetCamString;

        // Zoom
        _qButtonImage.sprite = _qButtonSprite;
        _eButtonImage.sprite = _eButtonSprite;
        _lsButtonImage.sprite = _lsButtonSprite;
        _rsButtonImage.sprite = _rsButtonSprite;
         _zoomText.text = _zoomString;

        // Reset Selection
        _eastButtonImage.sprite = _eastButtonSprite;
        _ctrlButtonImage.sprite = _ctrlButtonSprite;
        _resetSelText.text = _resetSelectionString;

    }
    private void SetUpEditorUI()
    {
        _editorBGImage.sprite = _editorBGSprite;
        _editorTitleBGImage.sprite = _editorTitleBGSprite;
        _editorTitleText.text = _editorTitleString;
    }
    private void SetUpEndGameUI()
    {
        _endBGImage.sprite = _engBGSprite;
        _endTitleBGImage.sprite = _endTitleSprite;
        _endTitleText.text = _endTitleString;
        _endScoreBGImage.sprite = _endScoreBGSprite;
    }
    private void SetUpGameUI()
    {
        _gameBGImage.sprite = _gameBGSprite;

        // Score
        _scoreGameImage.sprite = _scoreGameSprite;
        //_scoreText.text = _scoreString;

        // Food
        _foodImage.sprite = _foodSprite;
        //_foodText.text = _foodString;

        // Rocks
        _rocksImage.sprite = _rocksSprite;
        //_rocksText.text = _rocksString;

        // Wood
        _woodImage.sprite = _woodSprite;
        //_woodText.text = _woodString;

        // Health
        _healthGameImage.sprite = _healthGameSprite;
        //_healthGameText.text = _healthGameString;

        // Emotions
        _emotioneImage.sprite = _emotioneSprite;
        //_emotionText.text = _emotionString;

        _AgentCountImage.sprite = _agentCountSprite;
    }
    private void SetUpUI()
    {
        SetUpDebugUI();
        SetUpMainMenuUI();
        SetUpControlsUI();
        SetUpEditorUI();
        SetUpEndGameUI();
        SetUpGameUI();        
    }
}
