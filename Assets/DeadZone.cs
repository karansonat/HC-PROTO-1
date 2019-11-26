using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerController.Instance.Despawn(other.gameObject);
        Debug.Log("Ball Destroyed");
    }
}
