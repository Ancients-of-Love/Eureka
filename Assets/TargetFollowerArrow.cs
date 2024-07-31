using UnityEngine;

public class TargetFollowerArrow : MonoBehaviour
{
    public static Transform Target;
    public Transform Player;
    public GameObject Arrow;

    public Transform target;

    private void Awake()
    {
        Player = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    private void Update()
    {
        transform.position = Player.position;

        if (Target == null)
        {
            Arrow.SetActive(false);
            return;
        }

        Arrow.SetActive(true);
        transform.up = Target.position - transform.position;
    }
}