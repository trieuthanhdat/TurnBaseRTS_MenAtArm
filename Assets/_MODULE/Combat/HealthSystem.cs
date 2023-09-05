using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler onDead;
    public event EventHandler onDamage;
    private const float HEALTH_MAX = 100;
    [SerializeField] private float health = HEALTH_MAX;
    public void Damage(int amount)
    {
        health -= amount;
        onDamage?.Invoke(this, EventArgs.Empty);
        if(health < 0)
            health = 0;
        if(health == 0)
            Die();
    }

    private void Die()
    {
        onDead?.Invoke(this, EventArgs.Empty);
    }
    public float GetHealthProportion()
    {
        return health/ HEALTH_MAX;
    }

}
