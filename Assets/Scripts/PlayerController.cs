using UnityEngine;
using Lean.Pool;

public class PlayerController
{
    #region Fields
    #region Singleton

    private static PlayerController _instance = new PlayerController();
    public static PlayerController Instance
    {
        get { return _instance; }
    }

    static PlayerController()
    {
    }

    private PlayerController()
    {
    }

    #endregion

    private LeanGameObjectPool _pool;

    private readonly string PLAYER_PREFAB_PATH = "Prefabs/Avatars/";
    private readonly Vector3 INITIAL_POSITION = new Vector3(0, 8.75f, 0);

    #endregion //Fields

    public void Init()
    {
        InitializePool();
        SpawnPlayerControlledObject(INITIAL_POSITION);
    }

    public GameObject SpawnPlayerControlledObject(Vector3 position, Transform parent = null, bool worldPositionStays = true)
    {
        return _pool.Spawn(position, Quaternion.identity, parent, worldPositionStays);
    }

    public void Despawn(GameObject go)
    {
        _pool.Despawn(go);
    }

    #region Private Methods

    private void InitializePool()
    {
        _pool = new GameObject("Pool").AddComponent<LeanGameObjectPool>();
        _pool.Prefab = GetPlayerControlledObjectPrefab();
        _pool.Preload = 200;
        _pool.PreloadAll();
    }

    private GameObject GetPlayerControlledObjectPrefab()
    {
        var prefabPath = PLAYER_PREFAB_PATH + GameController.Instance.GameData.AvatarName;
        return Resources.Load<GameObject>(prefabPath);
    }


    #endregion //Private Methods
}
