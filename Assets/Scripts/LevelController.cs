using UnityEngine;

public class LevelController
{
    #region Fields

    private readonly string LEVEL_PREFAB_PATH = "Prefabs/Levels/";

    #endregion //Fields

    #region Public Methods

    public void LoadLevel(int level)
    {
        var levelInstance = GetLevelInstance(level);
        SetLevelInstanceTransform(levelInstance.transform);
    }

    #endregion //Public Methods

    #region Private Methods

    private GameObject LoadLevelPrefab(int level)
    {
        return Resources.Load<GameObject>(LEVEL_PREFAB_PATH + level);
    }

    private GameObject GetLevelInstance(int level)
    {
        return Object.Instantiate(LoadLevelPrefab(level));
    }

    private void SetLevelInstanceTransform(Transform levelTransform)
    {
        levelTransform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    #endregion //Private Methods
}
