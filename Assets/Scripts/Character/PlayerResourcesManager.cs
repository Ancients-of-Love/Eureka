using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class PlayerResourcesManager : Singleton<PlayerResourcesManager>, IDamageable
{
    [SerializeField]
    [Min(1f)]
    public float PlayerMaxHealth;

    public float CurrentHealth;

    [SerializeField]
    private Slider Slider;

    private Timer RespawnTimer;

    public bool IsDead = false;

    private void Start()
    {
        CurrentHealth = PlayerMaxHealth;
        Slider.value = CurrentHealth / PlayerMaxHealth * 100;
        RespawnTimer = new Timer(3f);
    }

    private void Update()
    {
        if (IsDead)
        {
            RespawnTimer.Tick(Time.deltaTime);
            if (RespawnTimer.RemainingTime <= 0)
            {
                Respawn();
            }
        }
    }

    public void Respawn()
    {
        PlayerMovement.Instance.controller.enabled = false;
        IsDead = false;
        CurrentHealth = PlayerMaxHealth;
        Slider.value = CurrentHealth / PlayerMaxHealth * 100;
        var player = GameObject.FindGameObjectWithTag("Player");
        player.transform.position = new Vector3(0, 0, 0);
        RespawnTimer.ResetTimer();
        PlayerMovement.Instance.controller.enabled = true;
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
        IsDead = true;
        //THIS IS A MANAGER DUMMY, YOU DON'T DESTROY THIS
    }
}