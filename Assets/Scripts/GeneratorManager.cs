using System.Collections.Generic;
using UnityEngine;

public class GeneratorManager : Singleton<GeneratorManager>
{
    public Generator Generator;
    public GameObject GeneratorObject;
    public List<ElectricBuilding> AttachedBuildings = new();

    private new void Awake()
    {
        base.Awake();
        // TODO: Load the generator from the save file and load the sprites needed
    }

    /// <summary>
    /// Adds fuel to the generator
    /// </summary>
    /// <param name="fuel">The fuel used</param>
    /// <returns>The number of items used</returns>
    public int AddFuel(ItemAmount fuel)
    {
        if (fuel.Item is not BurnableItemSO)
        {
            return 0;
        }
        if (Generator.FuelLevel + (((BurnableItemSO)fuel.Item).BurnTime * fuel.Amount) <= Generator.FuelCapacity)
        {
            Generator.FuelLevel += (((BurnableItemSO)fuel.Item).BurnTime * fuel.Amount);
            return fuel.Amount;
        }
        else
        {
            Generator.FuelLevel = Generator.FuelCapacity;
            return (int)(Generator.FuelCapacity - Generator.FuelLevel) / ((BurnableItemSO)fuel.Item).BurnTime * fuel.Amount;
        }
    }

    public void AddFuel(BurnableItemSO burnableItem)
    {
        if (Generator.FuelLevel + burnableItem.BurnTime <= Generator.FuelCapacity)
        {
            Generator.FuelLevel += burnableItem.BurnTime;
        }
        else
        {
            Generator.FuelLevel = Generator.FuelCapacity;
        }
    }

    public void UpgradeGenerator()
    {
        if (Generator.GeneratorLevel < 3)
        {
            Generator.GeneratorLevel++;
            Generator.FuelCapacity += 50f;
            Generator.FuelConsumptionRate -= 0.1f;
        }
        // TODO: Change the sprite of the generator to reflect the new level
    }

    public bool Attach(ElectricBuilding electricBuilding)
    {
        if (Generator.CurrentCapacity + electricBuilding.UsedCapacity <= Generator.MaxCapacity)
        {
            Generator.CurrentCapacity += electricBuilding.UsedCapacity;
            AttachedBuildings.Add(electricBuilding);
            return true;
        }
        return false;
    }

    public void Detach(ElectricBuilding electricBuilding)
    {
        AttachedBuildings.Remove(electricBuilding);
        Generator.CurrentCapacity -= electricBuilding.UsedCapacity;
    }

    public void Toggle()
    {
        Generator.IsOn = !Generator.IsOn;
    }
}