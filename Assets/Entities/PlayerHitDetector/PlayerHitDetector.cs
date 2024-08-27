using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Interfaces;
using Entities.PlayerHitDetector;
using UnityEngine;

public class PlayerHitDetector : MonoBehaviour
{
    public int level = 0;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var levelEntity = other.GetComponent<ILevelEntity>();
        if (levelEntity.GetLevel() > level)
            GetComponentInParent<IPlayerHitDetectorParent>().Die();
        else
            levelEntity.Die();
    }
}
