using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multiplier : MonoBehaviour
{
    private HashSet<int> _spawnedInstances;
    [SerializeField] private int _multiplier;

    private Collider _collider;
    private Vector3 _posVector;

    private void Awake()
    {
        _spawnedInstances = new HashSet<int>();
        _collider = GetComponent<Collider>();
        _posVector = Vector3.zero;
    }

    private void OnTriggerEnter(Collider other)
    {
        var currentSize = other.transform.localScale.x;
        if (_spawnedInstances.Contains(other.gameObject.GetInstanceID()))
            return;

        var maxAllowedSize = GetMaxAllowedSize();
        var newSize = currentSize > maxAllowedSize ? maxAllowedSize : currentSize;

        PlayerController.Instance.Despawn(other.gameObject);
        StartCoroutine(SpawnRoutine(newSize));
    }

    private IEnumerator SpawnRoutine(float ballSize)
    {
        var bounds = _collider.bounds;
        for (int i = 0; i < _multiplier; i++)
        {
            var spawned = PlayerController.Instance.SpawnPlayerControlledObject(RandomPointInBounds(bounds));
            _spawnedInstances.Add(spawned.GetInstanceID());
            spawned.transform.localScale = Vector3.one * ballSize;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public Vector3 RandomPointInBounds(Bounds bounds)
    {
        _posVector.x = Random.Range(bounds.min.x, bounds.max.x);
        _posVector.y = Random.Range(bounds.min.y, bounds.max.y);
        return _posVector;
    }

    private float GetMaxAllowedSize()
    {
        switch (_multiplier)
        {
            case 10:
            case 25:
                return 0.75f;
            case 50:
            case 100:
            case 200:
                return 0.50f;
            default:
                return 1;
        }
    }

    private void OnDisable()
    {
        _spawnedInstances.Clear();
    }
}
