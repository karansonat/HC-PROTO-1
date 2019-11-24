using UnityEngine;
using System;

public class LevelController
{
    #region Fields

    private Transform _levelTransform;
    private readonly string LEVEL_PREFAB_PATH = "Prefabs/Levels/";

    #endregion //Fields

    #region Public Methods

    public void LoadLevel(int level)
    {
        _levelTransform = GetLevelInstance(level).transform;
        SetLevelInstanceTransform();
    }

    #endregion //Public Methods

    #region Private Methods

    private GameObject LoadLevelPrefab(int level)
    {
        return Resources.Load<GameObject>(LEVEL_PREFAB_PATH + level);
    }

    private GameObject GetLevelInstance(int level)
    {
        return UnityEngine.Object.Instantiate(LoadLevelPrefab(level));
    }

    private void SetLevelInstanceTransform()
    {
        _levelTransform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    #endregion //Private Methods
}
