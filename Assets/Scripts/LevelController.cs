using UnityEngine;
using System;
using DG.Tweening;

public class LevelController
{
    #region Fields

    private Transform _levelTransform;
    private readonly string LEVEL_PREFAB_PATH = "Prefabs/Levels/";
    private const int DISTANCE_BETWEEN_PARTS = 30;

    private readonly string TAG_LEVEL_PART_GATE = "LevelPartGate";
    private readonly string TAG_LEVEL_END_GATE = "LevelEndGate";

    public static Action LevelPartPassed;
    public static Action TransitionAnimationCompleted;
    public static Action LevelPassed;
    public static Action<int> MultiplierActivated;

    #endregion //Fields

    #region Public Methods

    public void LoadLevel(int level)
    {
        _levelTransform = GetLevelInstance(level).transform;
        SetLevelInstanceTransform();
        AttachLevelEvents();
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
        foreach (var handler in _levelTransform.GetComponentsInChildren<OnTriggerEnterHandler>(true))
            handler.GetComponent<OnTriggerEnterHandler>().Attach(OnTriggerEntered);
    }

    private void OnTriggerEntered(Collider other, LevelElement elementType)
    {
        switch (elementType)
        {
            case LevelElement.LevelPartGate:
                //OnLevelPartPassed();
                break;
            case LevelElement.LevelEndGate:
                //OnLevelPassed();
                break;
            case LevelElement.Multiplier2x:
                break;
            case LevelElement.Multiplier5x:
                break;
            case LevelElement.Multipliet10x:
                break;
            case LevelElement.Multiplier100x:
                break;
            case LevelElement.Multiplier200x:
                break;
        }
    }

    private void OnLevelPartPassed()
    {
        LevelPartPassed.Invoke();
        MoveToNextPart();
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

    private void MoveToNextPart()
    {
        _levelTransform.DOMoveY(_levelTransform.position.y + DISTANCE_BETWEEN_PARTS, 1f)
            .SetEase(Ease.InCirc)
            .onComplete += OnTransitionAnimationCompleted;
    }

    #endregion //Private Methods
}
