using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthHandler 
{
    public DamageType Type { get; }
    public void OnTakingDamage(int damage);
}

public enum DamageType { Player, Enemy}