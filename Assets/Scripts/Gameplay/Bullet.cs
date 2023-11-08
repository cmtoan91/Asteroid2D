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
            Die();
        }
    }
    public void OnTakingDamage(int damage)
    {
        GlobalVFXManager.Instance.PlayVFX(transform.position, Vector3.one * Random.Range(0.7f, 1), "BulletExplosion", Quaternion.identity, 0.5f);
        Die();
    }
    void Die()
    {
        PoolManager.ReleaseInstance(this);
    }
}
