using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class UnityEventOnEnableDisable : MonoBehaviour
{
    [SerializeField] private bool triggerOnEnable = true;
    [SerializeField] private bool triggerOnDisable = true;
    [SerializeField] private bool triggerOnDestroy = true;
    [ShowIf("triggerOnEnable")]
    public UnityEvent onEnableEvent;
    [ShowIf("triggerOnDisable")]
    public UnityEvent onDisableEvent;
    [ShowIf("triggerOnDestroy")]
    public UnityEvent onDestroyEvent;
    private void OnEnable()
    {
        if (triggerOnEnable)
        {
            onEnableEvent.Invoke();
        }
    }

    private void OnDisable()
    {
        if (triggerOnDisable)
        {
            onDisableEvent.Invoke();
        }
    }

    private void OnDestroy()
    {
        if (triggerOnDestroy)
        {
            onDestroyEvent.Invoke();
        }
    }
}
