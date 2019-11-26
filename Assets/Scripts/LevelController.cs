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

    private void ResetFlags()
    {
        IsLevelPartBucketActive = false;
        IsLevelEndBucketActive = false;
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

    private void AttachLevelEvents()
    {
        foreach (var bucket in _levelTransform.GetComponentsInChildren<LevelPartBucket>(true))
            bucket.LevelPartBucketActivated += OnLevelPartPassed;
    }

    private void OnLevelPartPassed()
    {
        IsLevelPartBucketActive = true;
    }

    private void OnLevelPassed()
    {
        Debug.Log("Level passed!");
        LevelPassed.Invoke();
        UnloadLevel();
    }

    private void OnTransitionAnimationCompleted()
    {
        TransitionAnimationCompleted.Invoke();
    }

    private void UnloadLevel()
    {
        throw new NotImplementedException();
    }

    #endregion //Private Methods
}
