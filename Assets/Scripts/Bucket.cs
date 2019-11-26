using System;
using UnityEngine;

public enum BucketType
{
    LevelPart,
    LevelEnd
}

public class Bucket : MonoBehaviour
{
    [SerializeField] private BucketType _type;
    public Action<BucketType> BucketActivated;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController.Instance.Despawn(other.gameObject);
        GameController.Instance.IncreaseCoinsByAmount(1);

        if (BucketActivated != null)
            BucketActivated.Invoke(_type);
    }
}
