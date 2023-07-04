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

    [SerializeField] Sprite _wButtonSprite;
    [SerializeField] Sprite _aButtonSprite;
    [SerializeField] Sprite _sButtonSprite;
    [SerializeField] Sprite _dButtonSprite;
    [SerializeField] Sprite _stickSprite;
    [SerializeField] Sprite _dpadSprite;
    [SerializeField] string _moveString;

    [SerializeField] Sprite _spaceBarSprite;
    [SerializeField] Sprite _southButtonSprite;
    [SerializeField] string _fireString;

    [SerializeField] Sprite _shiftSprite;
    [SerializeField] Sprite _westButtonSprite;
    [SerializeField] string _strafeString;

    [SerializeField] Sprite _qButtonSprite;
    [SerializeField] Sprite _eButtonSprite;
    [SerializeField] Sprite _lsButtonSprite;
    [SerializeField] Sprite _rsButtonSprite;
    [SerializeField] string _weaponSwapString;

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

    [SerializeField] Image _wButtonImage;
    [SerializeField] Image _aButtonImage;
    [SerializeField] Image _sButtonImage;
    [SerializeField] Image _dButtonImage;
    [SerializeField] Image _stickImage;
    [SerializeField] Image _dpadImage;
    [SerializeField] Text _moveText;

    [SerializeField] Image _spaceBarImage;
    [SerializeField] Image _southButtonImage;
    [SerializeField] Text _fireText;

    [SerializeField] Image _shiftImage;
    [SerializeField] Image _westImage;
    [SerializeField] Text _strafeText;

    [SerializeField] Image _qButtonImage;
    [SerializeField] Image _eButtonImage;
    [SerializeField] Image _lsButtonImage;
    [SerializeField] Image _rsButtonImage;
    [SerializeField] Text _weaponSwapText;

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
    [SerializeField] Image _healthImage;
    [SerializeField] Text _healthText;
    [SerializeField] Image _scoreImage;
    [SerializeField] Text _scoreText;

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
    public Text HealthTextUI { get { return _healthText; } set { _healthText = value; } }
    public Text ScoreTextUI { get { return _scoreText; } set { _scoreText = value; } }
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

        _wButtonImage.sprite = _wButtonSprite;
        _aButtonImage.sprite = _aButtonSprite;
        _sButtonImage.sprite = _sButtonSprite;
        _dButtonImage.sprite = _dButtonSprite;
        _stickImage.sprite = _stickSprite;
        _dpadImage.sprite = _dpadSprite;
        _moveText.text = _moveString;

        _spaceBarImage.sprite = _spaceBarSprite;
        _southButtonImage.sprite = _southButtonSprite;
        _fireText.text = _fireString;

        _shiftImage.sprite = _shiftSprite;
        _westImage.sprite = _westButtonSprite;
        _strafeText.text = _strafeString;

        _qButtonImage.sprite = _qButtonSprite;
        _eButtonImage.sprite = _eButtonSprite;
        _lsButtonImage.sprite = _lsButtonSprite;
        _rsButtonImage.sprite = _rsButtonSprite;
        _weaponSwapText.text = _weaponSwapString;
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

        _healthImage.sprite = _healthSprite;
        _scoreImage.sprite = _scoreSprite;
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
