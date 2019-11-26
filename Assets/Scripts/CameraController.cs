using System;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    #region Fields

    public static CameraController Instance { get; private set; }

    private float _speed = 20f;
    public static Action<float> OnCameraRotationChange;
    public static readonly int ROTATION_LIMIT = 25;
    private const int ROTATION360_DURATION = 1;
    private Vector3 _targetAngles;

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

        #endregion
    }

    #endregion //Unity Methods

    #region Public Methods

    public void Init()
    {
        InputController.SwipeAction += Rotate;
    }

    public void Rotate(SwipeAction swipeAction)
    {
        int multiplier = swipeAction.Direction == SwipeDirection.Left ? -1 : 1;
        var targetRot = transform.rotation * Quaternion.AngleAxis(swipeAction.SwipeValue * multiplier * _speed * Time.deltaTime, transform.up);

        _targetAngles = targetRot.eulerAngles;
        //HACK: Dirty solution
        if (_targetAngles.y > 300f)
            _targetAngles.y = _targetAngles.y - 360;

        if (_targetAngles.y > ROTATION_LIMIT)
        {
            _targetAngles.y = ROTATION_LIMIT;
            transform.eulerAngles = _targetAngles;
        }
        else if (_targetAngles.y < -ROTATION_LIMIT)
        {
            _targetAngles.y = -ROTATION_LIMIT;
            transform.eulerAngles = _targetAngles;
        }

        transform.DOKill();
        transform.DORotate(_targetAngles, 0.1f);

        OnCameraRotationChange.Invoke(_targetAngles.y);
    }

    public void MoveToNextPart()
    {
        Rotate360WithEase();
    }

    public void Reset()
    {
        transform.eulerAngles = Vector3.zero;
    }

    #endregion //Public Methods

    #region Private Methods

    private void Rotate360WithEase()
    {
        var angle = transform.eulerAngles.y;

        if (angle > 300f)
            angle -= 360;

        transform.DORotate(new Vector3(0, 360 - angle, 0), ROTATION360_DURATION, RotateMode.FastBeyond360)
            .SetEase(Ease.InCirc)
            .SetRelative();
    }

    #endregion //Private Methods
}