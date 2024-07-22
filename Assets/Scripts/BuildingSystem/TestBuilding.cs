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
        Debug.Log($"�n a {transform.name} �p�let vagyok gecih");
    }

    public override Vector3 GetBuildingPosition()
    {
        return transform.position;
    }
}