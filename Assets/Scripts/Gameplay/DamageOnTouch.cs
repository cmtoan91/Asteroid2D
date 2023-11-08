using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
    [SerializeField]
    DamageType _toDamage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IHealthHandler healthHandler = collision.gameObject.GetComponent<IHealthHandler>();
        if (healthHandler == null || healthHandler.Type != _toDamage) return;
        healthHandler.OnTakingDamage(1);
    }
}
