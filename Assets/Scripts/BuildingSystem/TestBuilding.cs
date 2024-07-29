using UnityEngine;

public class TestBuilding : Building
{
    [SerializeField]
    private float Cooldown = 0f;

    private Timer Timer;

    private new void Start()
    {
        base.Start();
        Timer = new(Cooldown);
    }

    public override void InteractWithBuilding()
    {
        if (!IsActive)
        {
            return;
        }
        Debug.Log($"Én a {transform.name} épület vagyok");
    }
}