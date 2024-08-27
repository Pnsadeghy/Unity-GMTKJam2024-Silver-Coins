using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Entities.Fruit;
using Entities.Interfaces;
using Entities.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IFruitEater, ILevelEntity
{
    [Header("Movement Parameters")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    
    public LayerMask groundLayer;
    public Transform groundChecker;
    
    [Header("Coyote Time Parameters")]
    public float coyoteTime = 0.2f;
    private float _coyoteTimeCounter;
    public float jumpBufferTime = 0.1f;
    private float _jumpBufferCounter;

    [Header("State Management")] 
    public int defaultLevel = 0;
    private bool _isGrounded;
    private Rigidbody2D _rb;
    private Animator _animator;
    private CapsuleCollider2D _collider;
    private int _level;
    
    [Header("Screen shakes")]
    public CinemachineImpulseSource convertShake;
    public CinemachineImpulseSource landShake;

    [Header("Sound")] 
    public AudioSource powerUpSound;
    public AudioSource land1Sound;
    public AudioSource land2Sound;
    public AudioSource land3Sound;
    public AudioSource JumpSound;
    
    private enum State { Idle, Running, Jumping, Falling }
    private State _currentState;

    private bool _isFacingRight = true;
    private bool _canJump = false;
    private bool _wasOnGround = false;
    private float _lastGroundY;
    private float _moveInput = 0;
    private bool _isDie = false;

    private List<PlayerLevelData> _levelData;

    public bool OnGround() => _wasOnGround;
    public bool IsJumping() => _rb && _rb.velocity.y > 0;

    private bool _isFalling;

    private void Awake()
    {
        TryGetComponent(out _rb);
        TryGetComponent(out _animator);
        TryGetComponent(out _collider);
    }

    private void Start()
    {
        _levelData = new List<PlayerLevelData>
        {
            new PlayerLevelData()
            {
                Scale = 1f,
                Gravity = 2f
            },
            new PlayerLevelData()
            {
                Scale = 1.5f,
                Gravity = 2f
            },
            new PlayerLevelData()
            {
                Scale = 2.3f,
                Gravity = 3.5f
            },
            new PlayerLevelData()
            {
                Scale = 4f,
                Gravity = 3.5f
            },
            new PlayerLevelData()
            {
                Scale = 5f,
                Gravity = 3.5f
            },
            new PlayerLevelData()
            {
                Scale = 6f,
                Gravity = 3.5f
            },
            new PlayerLevelData()
            {
                Scale = 7f,
                Gravity = 3.5f
            },
            new PlayerLevelData()
            {
                Scale = 8f,
                Gravity = 3.5f
            }
        };

        _lastGroundY = transform.position.y;
        _currentState = State.Idle;
        SetLevel(defaultLevel);

        _wasOnGround = Physics2D.OverlapCircle(groundChecker.position, 0.1f, groundLayer);
    }
    
    private void Update()
    {
        HandleState();
        if (!_isDie)
            HandleInput();
        HandleFlip();
    }
    
    private void HandleState()
    {
        switch (_currentState)
        {
            case State.Idle:
                if (!_isGrounded)
                    ChangeState(State.Falling);
                else if (!_moveInput.Equals(0))
                    ChangeState(State.Running);
                break;

            case State.Running:
                if (!_isGrounded)
                    ChangeState(State.Falling);
                else if (_moveInput.Equals(0))
                    ChangeState(State.Idle);
                break;

            case State.Jumping:
                if (_rb && _rb.velocity.y < 0)
                    ChangeState(State.Falling);
                break;

            case State.Falling:
                if (_isGrounded)
                    ChangeState(State.Idle);
                break;
        }
    }

    private void ChangeState(State newState)
    {
        switch (newState)
        {
            case State.Idle:
                _animator.SetTrigger("Idle");
                break;
            case State.Running:
                _animator.SetTrigger("Run");
                break;
            case State.Falling:
                _animator.SetTrigger("Fall");
                break;
            case State.Jumping:
                _animator.SetTrigger("Jump");
                break;
        }
        
        _currentState = newState;
    }
    
    private void HandleInput()
    {
        _moveInput = Input.GetAxisRaw("Horizontal");
        _rb.velocity = new Vector2(_moveInput * moveSpeed, _rb.velocity.y);

        // Coyote Time
        if (_isGrounded)
        {
            _coyoteTimeCounter = coyoteTime;
        }
        else
        {
            _coyoteTimeCounter -= Time.deltaTime;
        }

        var jumpRequested = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.Z);

        if (jumpRequested && Input.GetAxisRaw("Vertical") < 0)
        {
            var collider = Physics2D.OverlapCircle(groundChecker.position, 0.1f, groundLayer);
            if (collider != null)
            {
                PlatformOnewayController _platformOnewayController = null;
                collider.TryGetComponent(out _platformOnewayController);
                if (_platformOnewayController)
                {
                    _platformOnewayController.TempDisabled();
                    jumpRequested = false;
                }
            }
        }
        
        if (jumpRequested)
        {
            _jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        if (_jumpBufferCounter > 0f && _coyoteTimeCounter > 0f && _canJump)
        {
            Jump();
            _jumpBufferCounter = 0f;
        }
        
        /*if (Input.GetKeyDown(KeyCode.W))
            SetLevel(_level + 1);
        if (Input.GetKeyDown(KeyCode.S))
            SetLevel(_level - 1);*/
    }
    
    private void Jump()
    {
        JumpSound.Play();
        _rb.velocity = new Vector2(_rb.velocity.x, jumpForce);
        ChangeState(State.Jumping);
        _canJump = false;
    }
    
    private void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapCircle(groundChecker.position, 0.1f, groundLayer);

        if (!_isGrounded)
            _isFalling = !IsJumping();
        
        if (!_isGrounded.Equals(_wasOnGround))
        {
            _wasOnGround = _isGrounded;
            if (_isGrounded)
            {
                _canJump = true;
                if (_level > 0 && _lastGroundY - transform.position.y > 3f)
                {
                    landShake.GenerateImpulseWithForce(_level);
                
                    switch (_level)
                    {
                        case 0:
                            land1Sound.Play();
                            break;
                        case 1:
                            land2Sound.Play();
                            break;
                        case 2:
                            land3Sound.Play();
                            break;
                    }
                }
            }
            else
            {
                _lastGroundY = transform.position.y;
            }
        }
    }
    
    private void HandleFlip()
    {
        if (!_rb) return;
        if (_rb.velocity.x.Equals(0) || _isFacingRight.Equals(_rb.velocity.x > 0)) return;
        _isFacingRight = !_isFacingRight;
        transform.rotation = Quaternion.Euler(0, _isFacingRight ? 0 : 180f, 0);
    }

    private void SetLevel(int newLevel)
    {
        _level = newLevel;
        transform.localScale = Vector3.one * _levelData[_level].Scale;
        _rb.gravityScale = _levelData[_level].Gravity;
    }

    private void ConvertShake()
    {
        convertShake.GenerateImpulseWithForce(0.5f);
    }

    public void Eat()
    {
        SetLevel(_level + 1);
        ConvertShake();
        powerUpSound.Play();
    }

    public int GetLevel()
    {
        return this._level;
    }

    public void Hit()
    {
        if (_level.Equals(0))
            Die();
        else
        {
            LevelManager.Instance.EnemyDie();
            convertShake.GenerateImpulseWithForce(0.5f);
            SetLevel(_level - 1);
        }
    }

    public void Die()
    {
        if (_isDie) return;
        LevelManager.Instance.EnemyDie();
        convertShake.GenerateImpulseWithForce(0.5f);
        Destroy(_rb);
        Destroy(_collider);
        _isDie = true;
        _animator.SetTrigger("Die");
    }

    public void OnDisabled()
    {
        _moveInput = 0f;
        _rb.velocity = Vector2.zero;
        _isDie = true;
    }

    public void OnEnabled()
    {
        _isDie = false;
    }

    public void OnDie()
    {
        LevelManager.Instance.ResetScene(1f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Killer"))
        {
            Die();
        }
    }
}
