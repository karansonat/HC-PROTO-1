using System;
using UnityEngine;
using DG.Tweening;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    private float _speed = 5f;
    public static Action<float> OnCameraRotationChange;
    public static readonly int ROTATION_LIMIT = 25;
    private const int ROTATION360_DURATION = 1;

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

        DontDestroyOnLoad(gameObject);

        #endregion
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Rotate360WithEase();
        }
    }

    #endregion //Unity Methods

    public void Init()
    {
        InputController.SwipeAction += Rotate;
        LevelController.LevelPartPassed += Rotate360WithEase;
    }

    public void Rotate(SwipeAction swipeAction)
    {
        int multiplier = swipeAction.Direction == SwipeDirection.Left ? -1 : 1;
        transform.rotation *= Quaternion.AngleAxis(swipeAction.SwipeValue * multiplier * _speed * Time.deltaTime, transform.up);

        var angles = transform.eulerAngles;
        //HACK: Dirty solution
        if (angles.y > 300f)
            angles.y = angles.y - 360;

        if (angles.y > ROTATION_LIMIT)
        {
            angles.y = ROTATION_LIMIT;
            transform.eulerAngles = angles;
        }
        else if (angles.y < -ROTATION_LIMIT)
        {
            angles.y = -ROTATION_LIMIT;
            transform.eulerAngles = angles;
        }

        OnCameraRotationChange.Invoke(angles.y);
    }

    private void Rotate360WithEase()
    {
        var angle = transform.eulerAngles.y;

        if (angle > 300f)
            angle -= 360;

        transform.DORotate(new Vector3(0, 360 - angle, 0), ROTATION360_DURATION, RotateMode.FastBeyond360)
            .SetEase(Ease.InCirc)
            .SetRelative();
    }
}