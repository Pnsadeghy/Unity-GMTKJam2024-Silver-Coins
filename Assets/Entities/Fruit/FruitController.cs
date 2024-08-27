using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Fruit;
using UnityEngine;

public class FruitController : MonoBehaviour
{
    private Animator _animator;
    private CircleCollider2D _collider;

    private void Start()
    {
        TryGetComponent(out _animator);
        TryGetComponent(out _collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IFruitEater fruitEater = null;
        other.TryGetComponent(out fruitEater);
        if (fruitEater == null) return;
        Destroy(_collider);
        fruitEater.Eat();
        _animator.Play("Use");
    }

    public void OnFinish()
    {
        Destroy(gameObject);
    }
}
