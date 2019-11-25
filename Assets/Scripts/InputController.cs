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

    public static Action<SwipeAction>  SwipeAction;

    #endregion //Events

    #region Fields

    private SwipeDetector _swipeDetector;
    private SwipeAction _swipeAction;
    private bool _enabled;

    #endregion //Fields

    public void Init()
    {
        _swipeDetector = new SwipeDetector();
        _swipeDetector.OnSwipe += SwipeDetector_OnSwipe;

        _swipeAction = new SwipeAction();

        //TODO: Remove this. Input should be enabled from game controller.
        EnableInput();
    }

    public void EnableInput()
    {
        _enabled = true;
    }

    public void DisableInput()
    {
        _enabled = false;
    }

    private void SwipeDetector_OnSwipe(SwipeData obj)
    {
        _swipeAction.Direction = obj.Direction;
        _swipeAction.SwipeValue = Vector2.Distance(obj.EndPosition, obj.StartPosition);
        SwipeAction(_swipeAction);
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
        if (_enabled)
         _swipeDetector.Update();
    }

    #endregion //IMonoNotification Interface
}
