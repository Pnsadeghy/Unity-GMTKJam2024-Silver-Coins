using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour
{
    private Animator _animator;
    private bool _get;

    private void Awake()
    {
        TryGetComponent(out _animator);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_get) return;
        if (!other.CompareTag("Player")) return;
        _get = true;
        LevelManager.Instance.GetCoin();
        _animator.Play("Get");
    }

    public void OnEnd()
    {
        Destroy(gameObject);
    }
}
