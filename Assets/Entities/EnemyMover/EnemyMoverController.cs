using System;
using System.Collections;
using System.Collections.Generic;
using Entities.PlayerAreaDetecter;
using Entities.PlayerHitDetector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class EnemyMoverController : MonoBehaviour, IPlayerAreaDetecterUser, IPlayerHitDetectorParent
{
    public Transform groundChecker;
    public Transform frontChecker;
    public LayerMask groundLayer;
    public LayerMask frontLayer;
    public float moveSpeed = 5f;
    public int level;
    public AudioSource heySound;
    public AudioSource ohSound;
    public Light2D light;
    
    private Vector3 _target;
    private bool _isMoving = false;
    private int _direction = 1;
    private bool _listIsOn;

    private Animator _animator;
    private Rigidbody2D _rigidbody2D;

    enum State
    {
        Wait,
        Run,
        Scape,
        Die
    }

    private State _currentState;

    private bool _isDie => _currentState == State.Die;
    private bool _isScapping => _currentState == State.Scape;
    private bool _isHeySoundPlayed = false;
    private bool _isOhSoundPlayed = false;

    private void Awake()
    {
        TryGetComponent(out _animator);
        TryGetComponent(out _rigidbody2D);
        _currentState = State.Wait;
    }

    private void Start()
    {
        _target = transform.position;
    }

    private void Update()
    {
        if (_isDie) return;

        if (_listIsOn && light.intensity < 1f)
            light.intensity = Mathf.Lerp(light.intensity, 1, Time.deltaTime * 10f);
        
        if (!_isMoving) return;
        if (IsReached()
            || !Physics2D.OverlapCircle(groundChecker.position, 0.1f, groundLayer)
            || Physics2D.OverlapCircle(frontChecker.position, 0.1f, frontLayer))
        {
            _isMoving = false;
            _animator.Play("Idle");
            _rigidbody2D.velocity = Vector2.zero;
            ChangeState(State.Wait);
        }
        else
        {
            _animator.Play("Run");
            _rigidbody2D.velocity = new Vector2(_direction * moveSpeed, _rigidbody2D.velocity.y);
        }
    }

    public void SetFollow(Vector3 position, int playerLevel)
    {
        if (_isDie) return;

        if (!_listIsOn && light != null)
            _listIsOn = true;
        
        _target = position;
        _target.y = transform.position.y;
        var isScapping = playerLevel > level;
        
        ChangeState(isScapping ? State.Scape : State.Run);
        
        _isMoving = true;
        _direction = _target.x > transform.position.x ? 1 : -1;
        if (isScapping)
            _direction *= -1;
        transform.rotation = Quaternion.Euler(0, _direction.Equals(1) ? 0 : 180f, 0);
    }

    private void ChangeState(State state)
    {
        if (_currentState.Equals(state)) return;

        switch (state)
        {
            case State.Scape:
                if (_isOhSoundPlayed) break;
                _isOhSoundPlayed = true;
                ohSound.Play();
                break;
            case State.Run:
                if (_isHeySoundPlayed) break;
                _isHeySoundPlayed = true;
                heySound.Play();
                break;
        }
        
        _currentState = state;
    }

    public void FollowerGone()
    {
        if (_isDie) return;
        if (!_isScapping) return;
        ChangeState(State.Wait);
        _isMoving = false;
        _animator.Play("Idle");
    }

    private bool IsReached()
    {
        if (_isScapping) return false;
        if (transform.position.x.Equals(_target.x)) return true;
        if (_direction.Equals(1)) return transform.position.x > _target.x;
        return transform.position.x < _target.x;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundChecker.position, 0.1f);
        Gizmos.DrawWireSphere(frontChecker.position, 0.1f);
    }

    public void Die()
    {
        if (_isDie) return;
        LevelManager.Instance.EnemyDie();
        ChangeState(State.Die);
        _animator.Play("Die");
        Destroy(_rigidbody2D);
    }

    public void OnDie()
    {
        Destroy(gameObject);
    }
}
