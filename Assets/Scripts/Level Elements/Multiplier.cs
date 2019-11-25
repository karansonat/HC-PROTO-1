using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier : MonoBehaviour
{
    private HashSet<int> _spawnedInstances;
    [SerializeField] private int _multiplier;

    private void Awake()
    {
        _spawnedInstances = new HashSet<int>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_spawnedInstances.Contains(other.gameObject.GetInstanceID()))
            return;

        PlayerController.Instance.Despawn(other.gameObject);
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        for (int i = 0; i < _multiplier; i++)
        {
            var spawned = PlayerController.Instance.SpawnPlayerControlledObject(transform.position);
            _spawnedInstances.Add(spawned.GetInstanceID());
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void OnDisable()
    {
        _spawnedInstances.Clear();
    }
}
