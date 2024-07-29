using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResourcesManager : Singleton<PlayerResourcesManager>, IDamageable
{
    [SerializeField]
    [Min(1f)]
    public float PlayerMaxHealth;

    public float CurrentHealth { get; private set; }

    [SerializeField]
    private Slider Slider;

    private void Start()
    {
        CurrentHealth = PlayerMaxHealth;
        Slider.value = CurrentHealth / PlayerMaxHealth * 100;
    }

    public void Damage(float damageAmount)
    {
        CurrentHealth -= damageAmount;
        Slider.value = CurrentHealth / PlayerMaxHealth * 100;
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float healAmount)
    {
        CurrentHealth += healAmount;
        Slider.value = CurrentHealth / PlayerMaxHealth * 100;
        if (CurrentHealth > PlayerMaxHealth)
        {
            CurrentHealth = PlayerMaxHealth;
        }
    }

    public void Die()
    {
        //THIS IS A MANAGER DUMMY, YOU DON'T DESTROY THIS
    }
}