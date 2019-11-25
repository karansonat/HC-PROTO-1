using System;
using UnityEngine;

public class OnTriggerEnterHandler : MonoBehaviour
{
    [SerializeField] private LevelElement _elementType;
    private Action<Collider, LevelElement> _handler;

    public void Attach(Action<Collider, LevelElement> handler)
    {
        _handler = handler;
    }

    private void OnTriggerEnter(Collider other)
    {
        _handler?.Invoke(other, _elementType);
    }

    private void OnDisable()
    {
        _handler = null;
    }
}
