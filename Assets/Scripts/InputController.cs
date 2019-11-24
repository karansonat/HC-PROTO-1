using System;
using UnityEngine;

public struct SwipeAction
{
    public SwipeDirection Direction;
    public float SwipeValue;
}

public class InputController : IMonoNotification
{
    #region Events

    public static Action<SwipeAction> OnSwipeAction;

    #endregion //Events

    #region Fields

    private SwipeDetector _swipeDetector;
    private SwipeAction _swipeAction;

    #endregion //Fields

    public void Init()
    {
        _swipeDetector = new SwipeDetector();
        _swipeDetector.OnSwipe += SwipeDetector_OnSwipe;

        _swipeAction = new SwipeAction();
    }

    private void SwipeDetector_OnSwipe(SwipeData obj)
    {
        _swipeAction.Direction = obj.Direction;
        _swipeAction.SwipeValue = Vector2.Distance(obj.EndPosition, obj.StartPosition);
        OnSwipeAction(_swipeAction);
    }

    #region IMonoNotification Interface

    void IMonoNotification.FixedUpdate()
    {
    }

    void IMonoNotification.LateUpdate()
    {
    }

    void IMonoNotification.Update()
    {
        _swipeDetector.Update();
    }

    #endregion //IMonoNotification Interface
}
