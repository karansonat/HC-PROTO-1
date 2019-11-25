using System;
using UnityEngine;

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

        DontDestroyOnLoad(gameObject);

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
        LevelController.LevelPartPassed += OnLevelPartPassed;
        LevelController.TransitionAnimationCompleted += OnLevelTransitionCompleted;
        LevelController.LevelPassed += OnLevelPassed;
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
    }

    private void OnLevelPartPassed()
    {
        _inputController.DisableInput();
        //_playerController.Disable();
    }

    private void OnLevelTransitionCompleted()
    {
        _inputController.EnableInput();
        //_playerController.Reset();
    }

    private void OnLevelPassed()
    {
    }

    #endregion //Private Methods
}
