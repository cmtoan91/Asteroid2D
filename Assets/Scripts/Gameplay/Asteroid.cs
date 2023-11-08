using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour, IHealthHandler
{
    [SerializeField]
    int _health;

    [SerializeField]
    int _level;

    Movement _movement;
    public DamageType Type => DamageType.Enemy;

    private void Awake()
    {
        _movement = GetComponent<Movement>();
    }

    public void Init(int level, float speedModifier, Vector3 direction)
    {
        GlobalPubSub.PublishEvent(new AsteroidSpawnMessage());
        _health = _level = level;
        transform.localScale = Vector3.one * _level;
        _movement.Move(direction * speedModifier);
        transform.up = direction;
    }

    public void OnTakingDamage(int damage)
    {
        _health--;
        if(_health <= 0)
        {
            OnDie();
        }
    }

    void OnDie()
    {
        _level--;
        if(_level > 0)
        {
            for (int i = 0; i < 2; i++)
            {   
                float angle = Random.Range(-90f, 90f);
                Vector3 dir = Quaternion.AngleAxis(angle, Vector3.forward) * transform.up;
                GameManager.Instance.SpawnAsteroid(transform.position, _level, Mathf.Sqrt(4f/(float)_level), dir);
            }
        }
        GlobalPubSub.PublishEvent(new AsteroidDieMessage(_level + 1));
        PoolManager.ReleaseInstance(this);
    }
}
