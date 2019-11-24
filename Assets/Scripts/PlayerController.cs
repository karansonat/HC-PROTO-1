using System;
using UnityEngine;

public class PlayerController : IMonoNotification
{
    #region Fields

    private Transform _playerTransform;
    private Rigidbody _rigidbody;

    private float _levelAngle;

    private readonly string PLAYER_PREFAB_PATH = "Prefabs/Avatars/";
    private readonly Vector3 INITIAL_TRANSFORM = new Vector3(0, 8.75f, 0);
    private readonly float MAX_VELOCITY = 5f;

    #endregion //Fields

    public void Init(string avatarName)
    {
        CameraController.OnCameraRotationChange += OnLevelRotationChanged;

        _playerTransform = GetPlayerInstance(avatarName).transform;
        _rigidbody = _playerTransform.GetComponent<Rigidbody>();
        _playerTransform.SetPositionAndRotation(INITIAL_TRANSFORM, Quaternion.identity);
    }

    #region Private Methods

    private GameObject LoadPlayerPrefab(string avatarName)
    {
        return Resources.Load<GameObject>(PLAYER_PREFAB_PATH + avatarName);
    }

    private GameObject GetPlayerInstance(string avatarName)
    {
        return UnityEngine.Object.Instantiate(LoadPlayerPrefab(avatarName));
    }

    private void UpdateVelocity()
    {
        var velocity = Vector3.right * (_levelAngle / 25f) * MAX_VELOCITY;
        velocity.y = _rigidbody.velocity.y;
        _rigidbody.velocity = velocity;
    }

    private void OnLevelRotationChanged(float angle)
    {
        _levelAngle = angle;
    }

    void IMonoNotification.FixedUpdate()
    {
        UpdateVelocity();
    }

    void IMonoNotification.Update()
    {
        throw new NotImplementedException();
    }

    void IMonoNotification.LateUpdate()
    {
        throw new NotImplementedException();
    }

    #endregion //Private Methods
}
