using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class EndBossController : MonoBehaviour
{
    public float speed = 5f;
    private bool _isMoving;
    private bool _isRunning;
    private Animator _animator;
    private CinemachineImpulseSource _cinemachineImpulse;
    public AudioSource footstep;

    private void Awake()
    {
        TryGetComponent(out _animator);
        TryGetComponent(out _cinemachineImpulse);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isMoving)
            transform.position += transform.right * (speed * Time.deltaTime);
    }

    public void StartMoving()
    {
        _isMoving = true;
        _animator.SetBool("Moving", true);
    }

    public void StartRunning()
    {
        transform.rotation = Quaternion.Euler(0, 180f, 0);
        _animator.SetBool("Moving", true);
        _isMoving = true;
    }

    public void Stop()
    {
        _isMoving = false;
        _animator.SetBool("Moving", false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        other.GetComponent<PlayerController>().Die();
    }

    public void Shake()
    {
        _cinemachineImpulse.GenerateImpulseWithForce(0.5f);
        footstep.Play();
    }
}
