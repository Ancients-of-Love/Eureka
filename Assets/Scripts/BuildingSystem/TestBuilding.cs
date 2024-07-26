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
        Debug.Log($"�n a {transform.name} �p�let vagyok");
    }

    public override Vector3 GetBuildingPosition()
    {
        return transform.position;
    }
}