using UnityEngine;

public class GameController : MonoBehaviour
{
    #region Fields

    public static GameController Instance { get; private set; }

    private LevelController _levelController;
    private PlayerController _playerController;

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

    #endregion //Unity Methods

    #region Private Methods

    private void Init()
    {
        InitializeLevelController();
    }

    private void InitializeLevelController()
    {
        _levelController = new LevelController();
    }

    private void InitializeInputController()
    {

    }

    private void InitializePlayerController()
    {

    }

    #endregion //Private Methods
}
