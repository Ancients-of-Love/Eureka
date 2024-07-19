using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float VerticalDirection;
    [SerializeField] private float HorizontalDirection;
    [SerializeField] private float Speed = 5f;

    // Update is called once per frame
    private void Update()
    {
        HorizontalDirection = Input.GetAxisRaw("Horizontal");
        VerticalDirection = Input.GetAxisRaw("Vertical");
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(HorizontalDirection, VerticalDirection, 0f).normalized;
        controller.Move(Speed * Time.fixedDeltaTime * direction);
    }
}