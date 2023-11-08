using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHealthHandler
{
    [SerializeField]
    Bullet _bulletPrefab;

    [SerializeField]
    float _fireRate = 2;
    float _timePerShot;
    float _current = 0;
    public DamageType Type => DamageType.Player;
    Movement _movment;
    Camera _main;

    public void OnTakingDamage(int damage)
    {
        GlobalPubSub.PublishEvent(new PlayerDieMessage());
    }

    void Start()
    {
        _main = Camera.main;
        _movment = GetComponent<Movement>();
        GlobalPubSub.PublishEvent(new PlayerSpawnMessage(this));
        PoolManager.PoolGameObject(_bulletPrefab, "", 100);
        _timePerShot = 1 / _fireRate;
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if(_current > 0) 
            {
                _current -= Time.deltaTime;
            }
            else
            {
                _current = _timePerShot;
                Fire();
            }
        }
        float v = Input.GetAxis("Vertical");

        if (v > 0)
        {
            _movment.AddForce(transform.up);
        }
        Vector3 dir = (_main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        dir.z = 0;
        dir = dir.normalized;
        _movment.Aim(dir);
    }

    void Fire()
    {
        Bullet bullet = PoolManager.GetInstance<Bullet>(_bulletPrefab);
        bullet.transform.position = transform.position;
        bullet.Init(transform.up);
    }
}
