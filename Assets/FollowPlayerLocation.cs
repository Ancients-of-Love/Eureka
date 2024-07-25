using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class FollowPlayerLocation : MonoBehaviour
{
    public VisualEffect Shadow;
    public SphereCollider SphereCollider;

    private void Start()
    {
        if (SphereCollider == null)
        {
            var PlayerLight = GameObject.FindGameObjectWithTag("PlayerLightArea");
            SphereCollider = PlayerLight.GetComponent<SphereCollider>();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Shadow != null)
        {
            Shadow.SetVector3("SphereCenter", SphereCollider.transform.position);
            Shadow.SetFloat("SphereRadius", SphereCollider.radius);
        }
    }
}