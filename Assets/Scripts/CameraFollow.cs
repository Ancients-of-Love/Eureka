using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform Target;

    // Start is called before the first frame update
    private void Start()
    {
        Target = PlayerInteractManager.Instance.Player.transform;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = new Vector3(
            Mathf.Clamp(Target.position.x, -2.86f, 54.57f),
            Mathf.Clamp(Target.position.y, -14.57f, 10.9f),
            transform.position.z);
    }
}