using UnityEngine;

public class Generator : Building
{
    public bool IsOn = false;
    public float FuelCapacity = 1000f;
    public float FuelLevel = 1000f;
    public float FuelConsumptionRate = 1f;
    public int GeneratorLevel = 1;
    public int MaxCapacity = 100;
    public int CurrentCapacity = 0;

    public override void InteractWithBuilding()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (IsOn)
        {
            FuelLevel -= FuelConsumptionRate * Time.deltaTime;
            if (FuelLevel <= 0)
            {
                IsOn = false;
                FuelLevel = 0;
            }
        }
        if (CurrentCapacity > MaxCapacity)
        {
            IsOn = false;
        }
    }
}