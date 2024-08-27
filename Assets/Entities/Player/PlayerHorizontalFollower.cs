using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHorizontalFollower : MonoBehaviour
{
    public Transform player;

    private void Update()
    {
        if (!player) return;
        var position = transform.position;
        position.x = Mathf.Max(0, player.position.x);
        transform.position = position;
    }
}
