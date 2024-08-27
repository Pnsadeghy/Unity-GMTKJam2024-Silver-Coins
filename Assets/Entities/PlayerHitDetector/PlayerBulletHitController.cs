using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBulletHitController : MonoBehaviour
{
    public float speed;
    public int level;

    private void Update()
    {
        transform.position += transform.right * (Time.deltaTime * -speed);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.GetComponent<PlayerController>();
            if (player.GetLevel() > level)
                player.Hit();
            else
                player.Die();
            Destroy(gameObject);
        }

        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}