using System;
using UnityEngine;

public class LevelPartBucket : MonoBehaviour
{
    public Action LevelPartBucketActivated;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController.Instance.Despawn(other.gameObject);
        GameController.Instance.IncreaseCoinsByAmount(1);

        if (LevelPartBucketActivated != null)
            LevelPartBucketActivated.Invoke();
    }
}
