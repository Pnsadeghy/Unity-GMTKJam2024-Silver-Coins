using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorKeyDetectorController : MonoBehaviour
{
    public DoorKeyController controller;
    public int level = 2;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (other.GetComponent<PlayerController>().GetLevel() < level) return;
        controller.OpenDoor();
    }
}
