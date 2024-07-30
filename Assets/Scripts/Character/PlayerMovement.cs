using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float VerticalDirection;
    [SerializeField] private float HorizontalDirection;
    [SerializeField] private float Speed = 5f;
    [SerializeField] private LayerMask GroundLayer;

    private Ray2D ray;

    private Transform PreviousHit = null;

    [SerializeField] private Tile PlayerTile;

    // Update is called once per frame
    private void Update()
    {
        HorizontalDirection = Input.GetAxisRaw("Horizontal");
        VerticalDirection = Input.GetAxisRaw("Vertical");
        HandleFlip();

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
        Vector3 direction = new Vector3(HorizontalDirection, VerticalDirection, 0f).normalized;
        controller.Move(Speed * Time.fixedDeltaTime * direction);
    }

    private void HandleFlip()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
    }
}