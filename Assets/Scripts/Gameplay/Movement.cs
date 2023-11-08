using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField]
    Rigidbody2D _rb2d;

    [SerializeField]
    float _moveForce;

    [SerializeField]
    float _speed;

    public void AddForce(Vector2 direction)
    {
        _rb2d.AddForce(direction * _moveForce * Time.deltaTime);
        if(_rb2d.velocity.magnitude >= _speed)
        {
            _rb2d.velocity = _rb2d.velocity.normalized * _speed;
        }
    }

    public void Move(Vector2 direction)
    {
        _rb2d.velocity = direction * _speed;
    }

    public void Aim(Vector2 forward)
    {
        _rb2d.transform.up = forward;
    }
}
