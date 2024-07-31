using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightActivator : MonoBehaviour
{
    [SerializeField]
    private Animator Animator;

    private void Awake()
    {
        Animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        Animator.SetBool("IsOn", true);
    }
}