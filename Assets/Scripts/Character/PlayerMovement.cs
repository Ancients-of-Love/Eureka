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

    private Ray2D ray;

    private Transform PreviousHit = null;

    [SerializeField] private Tile PlayerTile;

    // Update is called once per frame
    private void Update()
    {
        HorizontalDirection = Input.GetAxisRaw("Horizontal");
        VerticalDirection = Input.GetAxisRaw("Vertical");
        HandleFlip(HorizontalDirection);

        var hit = Physics2D.Raycast(transform.position, transform.forward, 1f, GroundLayer).transform;
        if (hit && hit != PreviousHit)
        {
            PreviousHit = hit;
            PlayerTile = PreviousHit.GetComponent<Tile>();
        }

        if (PlayerTile.DarknessLevel > 0)
        {
            //DAMAGE
            //TIMER DAMAGE AMIKOR MÁSODPERC
        }
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            Move();
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