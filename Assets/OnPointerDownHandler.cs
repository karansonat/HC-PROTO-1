using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnPointerDownHandler : MonoBehaviour, IPointerDownHandler
{
    private Action _pointerDownCallback;

    public void Attach(Action callback)
    {
        _pointerDownCallback += callback;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        _pointerDownCallback?.Invoke();
    }
}
