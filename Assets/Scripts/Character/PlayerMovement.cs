using System;
using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PlayerMovement : Singleton<PlayerMovement>
{
    public CharacterController controller;
    [SerializeField] private float VerticalDirection;
    [SerializeField] private float HorizontalDirection;
    public bool CanMove = true;
    public float Speed = 5f;
    [SerializeField] private LayerMask GroundLayer;
    private Timer DamageTimer;
    private TorchDisplay Torch;

    [SerializeField] private Animator Animator;

    private Ray2D ray;

    private Transform PreviousHit = null;

    [SerializeField] private Tile PlayerTile;

    private void Start()
    {
        Torch = GetComponentInChildren<TorchDisplay>();
        Animator = GetComponentInChildren<Animator>();
        DamageTimer = new Timer(1f);
    }

    // Update is called once per frame
    private void Update()
    {
        HorizontalDirection = Input.GetAxisRaw("Horizontal");
        VerticalDirection = Input.GetAxisRaw("Vertical");
        HandleFlip(HorizontalDirection);
        Animator.SetFloat("Speed", controller.velocity.magnitude);

        var hit = Physics2D.Raycast(transform.position, transform.forward, 1f, GroundLayer).transform;
        if (hit && hit != PreviousHit)
        {
            PreviousHit = hit;
            PlayerTile = PreviousHit.GetComponent<Tile>();
        }
        Debug.Log(PlayerTile.DarknessLevel);
        if (PlayerTile.DarknessLevel > 0 && (!Torch.LightOn || !Torch.LightOnOverride))
        {
            DamageTimer.Tick(Time.deltaTime);
            if (DamageTimer.RemainingTime <= 0)
            {
                PlayerResourcesManager.Instance.Damage(2f);
                DamageTimer.ResetTimer();
            }
        }
        else if ((PlayerResourcesManager.Instance.CurrentHealth < PlayerResourcesManager.Instance.PlayerMaxHealth) && PlayerTile.DarknessLevel == 0)
        {
            DamageTimer.Tick(Time.deltaTime);
            if (DamageTimer.RemainingTime <= 0)
            {
                PlayerResourcesManager.Instance.Heal(2f);
                DamageTimer.ResetTimer();
            }
        }
        else
        {
            DamageTimer.ResetTimer();
        }
        HandleMusic();
    }

    private void FixedUpdate()
    {
        if (CanMove && !PlayerResourcesManager.Instance.IsDead)
        {
            Move();
        }
    }

    public void HandleMusic()
    {
        if (PlayerTile.DarknessLevel > 0 && Torch.LightOn)
        {
            Audio.Instance.PlayIntenseMusic();
        }
        else if (PlayerTile.DarknessLevel > 0)
        {
            Audio.Instance.PlayDamagingMusic();
        }
        else
        {
            Audio.Instance.PlayBaseMusic();
        }
    }

    private void Move()
    {
        Vector3 direction = new Vector3(HorizontalDirection, VerticalDirection, 0f).normalized;
        controller.Move(Speed * Time.fixedDeltaTime * direction);
    }

    public void HandleFlip(float horizontalDirection)
    {
        if (horizontalDirection > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (horizontalDirection < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
}