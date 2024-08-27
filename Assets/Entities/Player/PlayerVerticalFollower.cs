using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVerticalFollower : MonoBehaviour
{
    public Transform player;

    private void Update()
    {
        if (!player) return;
        var position = transform.position;
        position.y = Mathf.Max(0, player.position.y);
        transform.position = position;
    }
}
