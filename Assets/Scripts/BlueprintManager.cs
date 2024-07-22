using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlueprintManager : MonoBehaviour
{
    public static BlueprintManager Instance;
    public List<BlueprintSO> Blueprints;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("BlueprintManager instance already exists, destroying the new one.");
            Destroy(gameObject);
        }
    }

    public void ResearchItem(ItemSO item)
    {
        item.IsResearched = true;
        foreach (var blueprint in Blueprints)
        {
            if (blueprint.RequiredItems.All(itemAmount => itemAmount.Item.IsResearched))
            {
                blueprint.IsUnlocked = true;
            }
        }
    }

    public bool CanCraft(BlueprintSO blueprint)
    {
        return InventoryManager.Instance.HasEnoughItems(blueprint.RequiredItems);
    }

    public void Craft(BlueprintSO blueprint)
    {
        if (!CanCraft(blueprint))
        {
            Debug.LogWarning("Cannot craft the item. Missing required items.");
            return;
        }

        if (!blueprint.IsUnlocked)
        {
            Debug.LogWarning("Cannot craft the item. Blueprint is not unlocked.");
            return;
        }

        if (InventoryManager.Instance.IsInventoryFull(blueprint.ResultItem))
        {
            Debug.LogWarning("Cannot craft the item. Inventory is full.");
            return;
        }

        foreach (var itemAmount in blueprint.RequiredItems)
        {
            InventoryManager.Instance.RemoveItem(itemAmount.Item, itemAmount.Amount);
        }

        foreach (var itemAmount in blueprint.ResultItem)
        {
            InventoryManager.Instance.AddItem(itemAmount.Item, itemAmount.Amount);
        }
    }
}