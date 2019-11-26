using UnityEngine;
using System;
using DG.Tweening;

public class LevelController
{
    #region Fields

    private Transform _levelTransform;
    private readonly string LEVEL_PREFAB_PATH = "Prefabs/Levels/";
    private const int DISTANCE_BETWEEN_PARTS = 30;
    public bool IsLevelPartBucketActive { get; private set; }
    public bool IsLevelEndBucketActive { get; private set; }

    private readonly string TAG_LEVEL_PART_GATE = "LevelPartGate";
    private readonly string TAG_LEVEL_END_GATE = "LevelEndGate";

    public static Action TransitionAnimationCompleted;
    public static Action LevelPassed;
    public static Action<int> MultiplierActivated;

    #endregion //Fields

    #region Public Methods

    public void LoadLevel(int level)
    {
        UnloadLevel();

        IsLevelPartBucketActive = false;
        IsLevelEndBucketActive = false;
        _levelTransform = GetLevelInstance(level).transform;
        SetLevelInstanceTransform();
        AttachLevelEvents();
    }

    public void MoveToNextPart()
    {
        _levelTransform.DOMoveY(_levelTransform.position.y + DISTANCE_BETWEEN_PARTS, 1f)
            .SetEase(Ease.InCirc)
            .onComplete += OnTransitionAnimationCompleted;

        ResetFlags();
    }

    #endregion //Public Methods

    #region Private Methods

    private void UnloadLevel()
    {
        if (_levelTransform != null)
        {
            UnityEngine.Object.Destroy(_levelTransform.gameObject);
            _levelTransform = null;
        }
    }

    private void ResetFlags()
    {
        IsLevelPartBucketActive = false;
        IsLevelEndBucketActive = false;
    }

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

    private void AttachLevelEvents()
    {
        foreach (var bucket in _levelTransform.GetComponentsInChildren<Bucket>(true))
            bucket.BucketActivated += BucketActivated;
    }

    private void BucketActivated(BucketType type)
    {
        if (type == BucketType.LevelPart)
        {
            IsLevelPartBucketActive = true;
        }
        else if (type == BucketType.LevelEnd)
        {
            IsLevelEndBucketActive = true;
        }
    }

    private void OnLevelPassed()
    {
        Debug.Log("Level passed!");
        LevelPassed.Invoke();
    }

    private void OnTransitionAnimationCompleted()
    {
        TransitionAnimationCompleted.Invoke();
    }

    #endregion //Private Methods
}
