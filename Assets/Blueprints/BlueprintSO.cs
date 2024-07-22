using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemAmount
{
    public ItemSO Item;
    public int Amount;
}

[CreateAssetMenu(fileName = "Blueprint", menuName = "Blueprints/Blueprint", order = 1)]
public class BlueprintSO : ScriptableObject, IUnlockable
{
    public void Unlock() => IsUnlocked = true;

    public void Lock() => IsUnlocked = false;

    public bool IsUnlocked = false;
    public List<ItemAmount> RequiredItems;
    public List<ItemAmount> ResultItem;
    public float CraftingTime = 100; // 2 seconds when using FixedUpdate
}