using TMPro;
using UnityEngine;

public class Gate : MonoBehaviour
{
    [SerializeField] private int _requiredBallCount;
    private int _collectedBall;

    private TextMeshPro _text;
    private Collider _collider;
    private Vector3 _posVector;

    private void Awake()
    {
        _text = GetComponentInChildren<TextMeshPro>(true);
        _collider = GetComponent<Collider>();
        _posVector = Vector3.zero;  
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_collectedBall < _requiredBallCount)
        {
            PlayerController.Instance.Despawn(other.gameObject);
            _collectedBall++;
            _text.SetText((_requiredBallCount - _collectedBall).ToString());

            if (_collectedBall == _requiredBallCount)
            {
                var bounds = _collider.bounds;

                for (int i = 0; i < _requiredBallCount; i++)
                {
                    var spawned = PlayerController.Instance.SpawnPlayerControlledObject(RandomPointInBounds(bounds));
                    spawned.transform.localScale = Vector3.one * GetMaxAllowedSize();
                    spawned.GetComponent<Rigidbody>().AddForce(Vector3.up * 1000);
                }

                gameObject.SetActive(false);
            }
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
        if (_requiredBallCount < 10)
            return 1;
        if (_requiredBallCount < 50)
            return 0.75f;

        return 0.5f;
    }

    private void OnDisable()
    {
        _collectedBall = 0;
    }
}
