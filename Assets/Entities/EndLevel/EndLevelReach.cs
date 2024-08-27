using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelReach : MonoBehaviour
{
    public EndLevelManager manager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        manager.ReachEnd();
        Destroy(gameObject);
    }
}
