using System;
using System.Collections;
using System.Collections.Generic;
using Entities.PlayerHitDetector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TrunkController : MonoBehaviour, IPlayerHitDetectorParent
{
    public Transform shootPoint;
    public GameObject bullet;
    public float timeout;
    public AudioSource shootSound;
    public Light2D light;
    public AudioSource appearSound;
    
    private Animator _animator;
    private bool _isDead;
    private float _lastShoot;
    private bool _isActive;
    private bool _lightIsOn;

    private void Awake()
    {
        TryGetComponent(out _animator);
    }

    private void Update()
    {
        if (_isDead) return;
        if (_isActive && Time.time > _lastShoot + timeout)
            Shoot();
        if (_lightIsOn && light.intensity < 1f)
            light.intensity = Mathf.Lerp(light.intensity, 1f, Time.deltaTime * 20f);
    }

    public void OnShoot()
    {
        if (_isDead) return;
        shootSound.Play();
        Instantiate(bullet, shootPoint.position, transform.rotation);
    }

    public void OnDie()
    {
        Destroy(gameObject);
    }

    public void Shoot()
    {
        if (_isDead) return;
        _animator.Play("Shoot");
        _lastShoot = Time.time;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_isDead) return;
        if (!other.CompareTag("Player")) return;
        _isActive = true;
        if (!_lightIsOn && light != null)
        {
            _lightIsOn = true;
            appearSound.Play();
        }
        Shoot();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (_isDead) return;
        if (!other.CompareTag("Player")) return;
        _isActive = false;
    }

    public void Die()
    {
        if (_isDead) return;
        LevelManager.Instance.EnemyDie();
        _isDead = true;
        _animator.Play("Hit");
    }
}
