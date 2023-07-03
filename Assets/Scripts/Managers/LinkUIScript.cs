using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LinkUIScript : MonoBehaviour
{
    [Header("Debug Canvas")]
    [SerializeField] Image _debugBG;
    [SerializeField] Image _debugTitleBG;
    [SerializeField] Text _debugTitleText;

    [SerializeField] Sprite _debugBGImage;
    [SerializeField] Sprite _debugTitleImage;
    [SerializeField] string _debugTitleString;

    [Header("MainMenu Canvas")]
    [SerializeField] Image _mainMenuBG;
    [SerializeField] Image _mainMenuTitle;
    [SerializeField] Text _mainMenuTitleText;

    [SerializeField] Sprite _mainMenuBGImage;
    [SerializeField] Sprite _mainMenuTitleImage;
    [SerializeField] string _mainMenuTitleString;

    [Header("ControlsMenu Canvas")]
    [SerializeField] Image _controlsBG;
    [SerializeField] Image _controlsTitle;
    [SerializeField] Text _controlsTitleText;

    [SerializeField] Image _wButtonImage;
    [SerializeField] Image _aButtonImage;
    [SerializeField] Image _sButtonImage;
    [SerializeField] Image _dButtonImage;
    [SerializeField] Image _stickImage;
    [SerializeField] Image _dpadImage;
    [SerializeField] Text _MoveText;

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

    [SerializeField] Sprite _controlsBGImage;
    [SerializeField] Sprite _controlsTitleImage;
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

    [Header("Game Canvas")]
    [SerializeField] Image _gameBG;

    [SerializeField] Image _healthImage;
    [SerializeField] Text _healthText;
    [SerializeField] Image _ammoImage;
    [SerializeField] Text _ammoText;
    [SerializeField] Image _scoreImage;
    [SerializeField] Text _scoreText;
    [SerializeField] Image _weaponImage;
    [SerializeField] Text _weaponText;

    [SerializeField] Sprite _gameBGImage;

    [SerializeField] Sprite _healthSprite;
    [SerializeField] Sprite _ammoSprite;
    [SerializeField] Sprite _scoreSprite;
    [SerializeField] Sprite _weaponSprite;

    [Header("Editor Canvas")]
    [SerializeField] Image _editorBG;
    [SerializeField] Image _editorTitle;
    [SerializeField] Text _editorTitleText;

    [SerializeField] Sprite _editorBGImage;
    [SerializeField] Sprite _editorTitleImage;
    [SerializeField] string _editorTitleString;

    [Header("EndGameMenu Canvas")]
    [SerializeField] Image _endBG;
    [SerializeField] Image _endTitle;
    [SerializeField] Text _endTitleText;
    [SerializeField] Image _endScoreBG;
    [SerializeField] Text _endScoreBGText;

    [SerializeField] Sprite _engBGImage;
    [SerializeField] Sprite _endTitleImage;
    [SerializeField] string _endTitleString;
    [SerializeField] Sprite _endScoreBGSprite;

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
    private void SetUpUI()
    {
        // Debug
        _debugBG.sprite = _debugBGImage;
        _debugTitleBG.sprite = _debugTitleImage;
        _debugTitleText.text = _debugTitleString;

        // Main Menu
        _mainMenuBG.sprite = _mainMenuBGImage;
        _mainMenuTitle.sprite = _mainMenuTitleImage;
        _mainMenuTitleText.text = _mainMenuTitleString;

        // ControlsMenu
        _controlsBG.sprite = _controlsBGImage;
        _controlsTitle.sprite = _controlsTitleImage;
        _controlsTitleText.text = _controlsTitleString;

        _wButtonImage.sprite = _wButtonSprite;
        _aButtonImage.sprite = _aButtonSprite;
        _sButtonImage.sprite = _sButtonSprite;
        _dButtonImage.sprite = _dButtonSprite;
        _stickImage.sprite = _stickSprite;
        _dpadImage.sprite = _dpadSprite;
        _MoveText.text = _moveString;

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

        // Editor Menu
        _editorBG.sprite = _editorBGImage;
        _editorTitle.sprite = _editorTitleImage;
        _editorTitleText.text = _editorTitleString;

        // End Screen
        _endBG.sprite = _engBGImage;
        _endTitle.sprite = _endTitleImage;
        _endTitleText.text = _endTitleString;
        _endScoreBG.sprite = _endScoreBGSprite;

        // Game
        _gameBG.sprite = _gameBGImage;

        _healthImage.sprite = _healthSprite;
        _ammoImage.sprite = _ammoSprite;
        _scoreImage.sprite = _scoreSprite;
    }

    public Text HealthTextUI { get { return _healthText;} set { _healthText = value; } }
    public Text AmmoTextUI { get { return _ammoText; } set { _ammoText = value; } }
    public Text ScoreTextUI { get { return _scoreText; } set { _scoreText = value; } }
    public Text WeaponTextUI { get { return _weaponText; } set { _weaponText = value; } }
    public Text ScoreEndScreenUI { get { return _endScoreBGText; } set { _endScoreBGText = value; } }
    public Image ActiveWeaponUI { get { return _weaponImage; } set { _weaponImage = value; } }

}
