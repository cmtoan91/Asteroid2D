using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IHealthHandler
{
    [SerializeField]
    Bullet _bulletPrefab;

    [SerializeField]
    float _fireRate = 2;

    [SerializeField]
    float _invinciblePeriod = 1f;

    float _timePerShot;
    float _current = 0;
    public DamageType Type => DamageType.Player;
    Movement _movment;
    VisualHandler _visualHandler;
    Camera _main;
    bool _isInvincible;
    Vector3 _startPos;

    public void OnTakingDamage(int damage)
    {
        if (_isInvincible) return;
        GlobalVFXManager.Instance.PlayVFX(transform.position, Vector3.one, "ShipExplosion", Quaternion.identity);
        GlobalPubSub.PublishEvent(new PlayerDieMessage());
    }

    void Start()
    {
        _main = Camera.main;
        _movment = GetComponent<Movement>();
        _visualHandler = GetComponent<VisualHandler>();
        GlobalPubSub.PublishEvent(new PlayerSpawnMessage(this));
        PoolManager.PoolGameObject(_bulletPrefab, "", 100);
        _timePerShot = 1 / _fireRate;
        _startPos = transform.position;
    }

    public void OnReinit()
    {
        StartCoroutine(Invincible(_invinciblePeriod));
        _visualHandler.Flashing(_invinciblePeriod);
        _movment.Move(Vector2.zero);
        transform.position = _startPos;
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

        if (Input.GetButton("Jump")) _movment.AddForce(transform.up);
        
        if (Input.GetButtonDown("Jump")) _visualHandler.PlayMoveVFX(true);

        if(Input.GetButtonUp("Jump")) _visualHandler.PlayMoveVFX(false);
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

    IEnumerator Invincible(float time)
    {
        _isInvincible = true;
        yield return new WaitForSeconds(time);
        _isInvincible = false;
    }
}
