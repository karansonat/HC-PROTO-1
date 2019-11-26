using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class UIController : MonoBehaviour
{
    #region Fields

    public static UIController Instance { get; private set; }

    public Action PlayButtonPressed;
    public Action RestartButtonPressed;

    [SerializeField] private Button _buttonPlay;
    [SerializeField] private Button _buttonRestart;
    [SerializeField] private GameObject _labelGameOver;
    [SerializeField] private GameObject _labelLevelPassed;
    [SerializeField] private TextMeshProUGUI _textScore;

    [SerializeField] private GameObject _panelInGame;
    [SerializeField] private GameObject _panelHowToPlay;
    [SerializeField] private GameObject _panelEndGame;

    private GameObject _currentPanel;

    #endregion //Fields

    #region Unity Methods

    private void Awake()
    {
        #region Singleton

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        #endregion //Singleton
    }

    #endregion //Unity Methods

    #region Public Methods

    public void Init()
    {
        _buttonPlay.onClick.RemoveAllListeners();
        _buttonPlay.GetComponent<OnPointerDownHandler>().Attach(OnPlayButtonPressed);

        _buttonRestart.onClick.RemoveAllListeners();
        _buttonRestart.onClick.AddListener(OnRestartButtonPressed);

        UpdateScore(true);

        ShowHowToPlayScreen();
    }

    public void ShowHowToPlayScreen()
    {
        ShowPanel(_panelHowToPlay);
    }

    public void ShowInGameScreen()
    {
        ShowPanel(_panelInGame);
    }

    public void ShowEndGameScreen(bool levelPassed = false)
    {
        _labelLevelPassed.SetActive(levelPassed);
        _labelGameOver.SetActive(!levelPassed);

        ShowPanel(_panelEndGame);
    }

    public void UpdateScore(bool withoutAnim = false)
    {
        var score = GameController.Instance.GameData.Coins;
        _textScore.text = score.ToString();

        if (!withoutAnim)
        {
            _textScore.transform.DOKill(true);
            _textScore.transform.DOScale(1.1f, 0.05f).SetEase(Ease.OutCubic).onComplete += () =>
            {
                _textScore.transform.localScale = Vector3.one;
            };
        }
    }

    #endregion //Public Methods

    #region Private Methods

    private void OnPlayButtonPressed()
    {
        PlayButtonPressed?.Invoke();

        ShowPanel(_panelInGame);
    }

    private void OnRestartButtonPressed()
    {
        RestartButtonPressed?.Invoke();
    }

    private void ShowPanel(GameObject panel)
    {
        if (_currentPanel != null)
            HidePanel(_currentPanel);

        _currentPanel = panel;
        _currentPanel.SetActive(true);
    }

    private void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    #endregion //Private Methods
}
