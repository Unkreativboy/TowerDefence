using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour, IDamageable
{


    public event EventHandler OnDamaged;
    public event EventHandler OnDied;

    [SerializeField] private int healthAmountMax = 100;
    private int healthAmount;

    
    private void Awake()
    {
        healthAmount = healthAmountMax;
    }
    public void Damage(int amount)
    {
        healthAmount -= amount;
        healthAmount = Mathf.Clamp(healthAmount, 0, healthAmountMax);
        
        OnDamaged?.Invoke(this, EventArgs.Empty);
        if (IsDead())
        {
            OnDied?.Invoke(this, EventArgs.Empty);
        }

    }
    public bool IsHealthFull()
    {
        return healthAmount == healthAmountMax;
    }
    public bool IsDead()
    {
        return healthAmount == 0;
    }
    public int GetHealthAmount()
    {
        return healthAmount;
    }
    public float GetHealthAmountNormalized()
    {
        return (float)healthAmount / healthAmountMax;
    }
    public void SetHealthAmountMax(int maxAmount, bool updateHealthAmount)
    {
        healthAmountMax = maxAmount;
        if(updateHealthAmount)
        {
            healthAmount = healthAmountMax;
        }

    }

}
