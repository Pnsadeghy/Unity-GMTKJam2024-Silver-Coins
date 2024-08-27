using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTrigger1DarkSide : MonoBehaviour
{
    public EndLevelManager manager;

    private Animator _animator;
    private bool _isTrigger;

    private void Awake()
    {
        TryGetComponent(out _animator);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isTrigger) return;
        if (!other.CompareTag("Player")) return;
        _animator.SetTrigger("Done");
    }

    public void OnDone()
    {
        manager.Trigger1DarkSideCall();
        Destroy(gameObject);
    }
}
