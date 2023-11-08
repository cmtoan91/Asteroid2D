using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IHealthHandler
{
    [SerializeField]
    DamageType _damageType;
    public DamageType Type => _damageType;

    [SerializeField]
    float _maxLifetime = 2f;

    float _curentLifeTime = 0;

    Movement _movement;
    public void OnTakingDamage(int damage)
    {
        PoolManager.ReleaseInstance(this);
    }

    private void Awake()
    {
        _movement = GetComponent<Movement>();
    }
    public void Init(Vector3 dir)
    {
        _curentLifeTime = _maxLifetime;
        transform.up = dir;
        _movement.Move(dir);
    }

    private void Update()
    {
        if(_curentLifeTime >= 0)
        {
            _curentLifeTime -= Time.deltaTime;
        }
        else
        {
            OnTakingDamage(0);
        }
    }
}
