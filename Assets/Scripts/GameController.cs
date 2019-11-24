using System;
using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Fields

    public static GameController Instance { get; private set; }

    private LevelController _levelController;
    private PlayerController _playerController;
    private InputController _inputController;
    private GameData _game;

    private readonly string GAME_DATA_PATH = "GameData/TestGameData";

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

        DontDestroyOnLoad(gameObject);

        #endregion
    }

    private void Start()
    {
        Init();   
    }

    private void FixedUpdate()
    {
        if (_playerController != null)
            (_playerController as IMonoNotification).FixedUpdate();
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
        InitializeInputController();
        InitializeLevelController();
        InitializePlayerController();
        CameraController.Instance.Init();
    }

    private void InitializeLevelController()
    {
        _levelController = new LevelController();
        _levelController.LoadLevel(_game.Level);
    }

    private void InitializeInputController()
    {
        _inputController = new InputController();
        _inputController.Init();
    }

    private void InitializePlayerController()
    {
        _playerController = new PlayerController();
        _playerController.Init(_game.AvatarName);
    }

    private void LoadGameData()
    {
        _game = Resources.Load<GameData>(GAME_DATA_PATH);
    }

    #endregion //Private Methods
}
