using System;
using System.Collections;
using System.Collections.Generic;
using Entities.PlayerAreaDetecter;
using UnityEngine;

public class PlayerAreaDetecter : MonoBehaviour
{
    private PlayerController _player = null;

    private void Update()
    {
        if (!_player) return;
        var children = GetComponentsInChildren<IPlayerAreaDetecterUser>();
        foreach (var child in children)
            child.SetFollow(_player.transform.position, _player.GetLevel());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        _player = other.GetComponent<PlayerController>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!_player) return;
        if (!other.transform.Equals(_player.transform)) return;
        _player = null;
        foreach (var child in GetComponentsInChildren<IPlayerAreaDetecterUser>())
            child.FollowerGone();
    }
}
