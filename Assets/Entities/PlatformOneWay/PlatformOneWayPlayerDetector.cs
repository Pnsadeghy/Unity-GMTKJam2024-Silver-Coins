using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Interfaces;
using UnityEngine;

public class PlatformOneWayPlayerDetector : MonoBehaviour
{
    private PlatformOnewayController _parent;
    private Transform _limiter;

    private void Awake()
    {
        _parent = GetComponentInParent<PlatformOnewayController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var limiter = other.GetComponent<ILevelEntity>();
        if (limiter != null && other.transform.position.y > transform.position.y)
            _parent.FindLimiter(limiter.GetLevel());
    }
}
