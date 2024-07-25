using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMobManager : MonoBehaviour
{
    public bool IsMoving = false;
    public float Speed = 0.05f;
    public bool IsSpooked = false;
    public bool IsDead = false;
    public float Alpha = 1f;

    private void FixedUpdate()
    {
        if (IsDead && Alpha <= 0)
        {
            // Destroy the game object (later make animation for it)
            Destroy(gameObject);
        }
        else if (IsDead)
        {
            // Fade out the shadow mob
            Alpha -= Time.fixedDeltaTime / 2f;
            var renderers = GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                var material = renderer.material;
                material.color = new Color(material.color.r, material.color.g, material.color.b, Alpha);
            }
            var playerTransform = GameObject.Find("Player").transform;
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, (-Speed - Time.fixedDeltaTime) / 2f);
        }
        else if (IsMoving && !IsSpooked)
        {
            // Move towards the player
            var playerTransform = GameObject.Find("Player").transform;
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, Speed + Time.fixedDeltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //TODO: Check if the player has a torch
            IsDead = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsDead = true;
        }
    }
}