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
            if (blueprint.RequiredItems.Any(x => x.Item.Id.Equals(item.Id)))
            {
                blueprint.IsUnlocked = true;
                // TODO: Add a notification that the blueprint is unlocked
                // TODO: Find a better solution
            }
        }
    }
}