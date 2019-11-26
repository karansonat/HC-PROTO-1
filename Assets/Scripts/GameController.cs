using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    #region Fields

    public static GameController Instance { get; private set; }

    private LevelController _levelController;
    private InputController _inputController;
    public GameData GameData { get; private set; }

    private readonly string GAME_DATA_PATH = "GameData/TestGameData";

    #endregion //Fields

    #region Unity Methods

    private void Awake()
    {
        Application.targetFrameRate = 60;

        #region Singleton

        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        #endregion
    }

    private void Start()
    {
        Init();
    }

    private void FixedUpdate()
    {
    }

    private void Update()
    {
        if (_inputController != null)
            (_inputController as IMonoNotification).Update();
    }

    #endregion //Unity Methods

    public void IncreaseCoinsByAmount(int amount)
    {
        GameData.Coins += amount;
        UIController.Instance.UpdateScore();
    }

    #region Private Methods

    private void Init()
    {
        LoadGameData();
        CreateControllers();
        InitializeControllers();
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        LevelController.TransitionAnimationCompleted += OnLevelTransitionCompleted;
        LevelController.LevelPassed += OnLevelPassed;

        PlayerController.Instance.NoMorePlayerControlledObject += OnNoMorePlayerControlledObject;

        UIController.Instance.PlayButtonPressed += OnPlayButtonPressed;
        UIController.Instance.RestartButtonPressed += OnRestartButtonPressed;

    }

    private void OnRestartButtonPressed()
    {
        UIController.Instance.ShowHowToPlayScreen();
        CameraController.Instance.Reset();
        _levelController.LoadLevel(GameData.Level);
        PlayerController.Instance.SetUpInitialPlayer();
    }

    private void OnPlayButtonPressed()
    {
        _inputController.EnableInput();
    }

    private void OnNoMorePlayerControlledObject()
    {
        switch (CheckGameEndConditions())
        {
            case GameState.LevelPartPassed:
                OnLevelPartPassed();
                break;
            case GameState.LevelPassed:
                OnLevelPassed();
                break;
            case GameState.GameOver:
                OnGameOver();
                break;
        }
    }

    private void OnGameOver()
    {
        UIController.Instance.ShowEndGameScreen();
        _inputController.DisableInput();
    }

    private GameState CheckGameEndConditions()
    {
        if (_levelController.IsLevelEndBucketActive)
            return GameState.LevelPassed;

        if (_levelController.IsLevelPartBucketActive)
            return GameState.LevelPartPassed;

            return GameState.GameOver;
    }

    private void LoadGameData()
    {
        GameData = Resources.Load<GameData>(GAME_DATA_PATH);
    }

    private void CreateControllers()
    {
        _levelController = new LevelController();
        _inputController = new InputController();
    }

    private void InitializeControllers()
    {
        //TODO: Level Controller also need Init() func.
        _levelController.LoadLevel(GameData.Level);
        _inputController.Init();
        PlayerController.Instance.Init();
        CameraController.Instance.Init();
        UIController.Instance.Init();
    }

    private void OnLevelPartPassed()
    {
        _inputController.DisableInput();
        _levelController.MoveToNextPart();
        CameraController.Instance.MoveToNextPart();
    }

    private void OnLevelTransitionCompleted()
    {
        _inputController.EnableInput();
        PlayerController.Instance.SetUpInitialPlayer();
    }

    private void OnLevelPassed()
    {
        UIController.Instance.ShowEndGameScreen(true);
    }

    #endregion //Private Methods
}
