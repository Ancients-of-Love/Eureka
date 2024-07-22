using UnityEngine;

public class TestBuilding : Building
{
    private new void Start()
    {
        base.Start();
    }

    private void Update()
    {
    }

    public override void InteractWithBuilding()
    {
        Debug.Log($"Én a {transform.name} épület vagyok gecih");
    }

    public override Vector3 GetBuildingPosition()
    {
        return transform.position;
    }
}