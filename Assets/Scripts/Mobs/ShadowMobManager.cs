using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowMobManager : MonoBehaviour
{
    public bool IsMoving = false;
    public float Speed = 0.05f;
    public bool IsDead = false;
    public float Alpha = 1f;
    public bool IsPlayerReachable = false;
    public float LifeTime = 10f;

    private void Update()
    {
        // TODO: Check if the player is reachable (is on Tile with more than 0 darkness)
        // TODO: Use the LifeTime variable to destroy the mob after a certain amount of time (when timer is implemented)
    }

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
        else if (IsMoving && IsPlayerReachable)
        {
            // Move towards the player
            var playerTransform = GameObject.Find("Player").transform;
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, Speed + Time.fixedDeltaTime);
        }
        else if (IsMoving && !IsPlayerReachable)
        {
            // Add a timer to delay changes in direction
            // Randomly move around
            var randomX = Random.Range(-1f, 1f);
            var randomY = Random.Range(-1f, 1f);
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x + randomX, transform.position.y + randomY, transform.position.z), Speed + Time.fixedDeltaTime);
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