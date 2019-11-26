using UnityEngine;
using Lean.Pool;
using System;
using System.Collections;

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
    private int _playerControlledObjectCount;

    private readonly string PLAYER_PREFAB_PATH = "Prefabs/Avatars/";
    private readonly Vector3 INITIAL_POSITION = new Vector3(0, 8.75f, 0);

    public Action NoMorePlayerControlledObject;

    #endregion //Fields

    public void Init()
    {
        InitializePool();
        SetUpInitialPlayer();
    }

    public void SetUpInitialPlayer()
    {
        SpawnPlayerControlledObject(INITIAL_POSITION);
    }

    public GameObject SpawnPlayerControlledObject(Vector3 position, Transform parent = null, bool worldPositionStays = true)
    {
        _playerControlledObjectCount++;
        return _pool.Spawn(position, Quaternion.identity, parent, worldPositionStays);
    }

    public void Despawn(GameObject go)
    {
        go.transform.localScale = Vector3.one;
        _pool.Despawn(go);
        _playerControlledObjectCount--;

        if (_playerControlledObjectCount == 0)
            GameController.Instance.StartCoroutine(WaitAndCheckForPlayerControlledObjects());
    }

    #region Private Methods

    private void InitializePool()
    {
        _pool = new GameObject("Pool").AddComponent<LeanGameObjectPool>();
        _pool.Prefab = GetPlayerControlledObjectPrefab();
        _pool.Preload = 500;
        _pool.PreloadAll();
    }

    private GameObject GetPlayerControlledObjectPrefab()
    {
        var prefabPath = PLAYER_PREFAB_PATH + GameController.Instance.GameData.AvatarName;
        return Resources.Load<GameObject>(prefabPath);
    }

    private IEnumerator WaitAndCheckForPlayerControlledObjects()
    {
        //Wait for the next frame to be sure. New objects can be spawned by gates
        yield return null;

        if (_playerControlledObjectCount == 0)
            NoMorePlayerControlledObject.Invoke();
    }

    #endregion //Private Methods
}
