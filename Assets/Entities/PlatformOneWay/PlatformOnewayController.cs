using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformOnewayController : MonoBehaviour
{
    [SerializeField]
    public int level = 1000;
    [SerializeField]
    public GameObject limitChecker;

    public ParticleSystem dust;
    
    private float _disabledAt;
    private bool _isDisabled;

    private BoxCollider2D _collider2D;

    private void Awake()
    {
        TryGetComponent(out _collider2D);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDisabled && Time.time > _disabledAt + 0.5f)
        {
            _isDisabled = false;
            _collider2D.isTrigger = false;
            limitChecker.SetActive(true);
        }
    }

    public void TempDisabled()
    {
        _collider2D.isTrigger = true;
        _disabledAt = Time.time;
        _isDisabled = true;
        limitChecker.SetActive(false);
    }

    public void FindLimiter(int limiterLevel)
    {
        if (limiterLevel > level)
        {
            dust.Stop();
            dust.Play();
            TempDisabled();
        }
    }
}
