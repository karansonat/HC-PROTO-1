using UnityEngine;

public class PlayerControlledObject : MonoBehaviour
{
    #region Fields

    private Rigidbody _rigidbody;
    private float _cameraAngle;

    private readonly float MAX_VELOCITY = 15f;

    #endregion

    #region Unity Methods

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        CameraController.OnCameraRotationChange += OnCameraRotationChanged;
    }

    private void FixedUpdate()
    {
        UpdateVelocity();
    }

    #endregion

    #region Private Methods

    private void UpdateVelocity()
    {
        var velocity = Vector3.right * (_cameraAngle / CameraController.ROTATION_LIMIT) * MAX_VELOCITY;
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;
    }

    private void OnCameraRotationChanged(float angle)
    {
        _cameraAngle = angle;
    }

    #endregion
}
