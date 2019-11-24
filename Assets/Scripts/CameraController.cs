using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    private float _speed = 5f;
    public static Action<float> OnCameraRotationChange;
    private const int ROTATION_LIMIT = 25;

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

    #endregion //Unity Methods

    public void Init()
    {
        InputController.OnSwipeAction += Rotate;
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
            angles.y = 25;
            transform.eulerAngles = angles;
        }
        else if (angles.y < -ROTATION_LIMIT)
        {
            angles.y = -25;
            transform.eulerAngles = angles;
        }

        OnCameraRotationChange.Invoke(angles.y);
    }
}
