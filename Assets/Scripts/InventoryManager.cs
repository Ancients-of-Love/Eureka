using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<ItemSlot> ItemSlots = new();

    /// <summary>
    /// Adds the item to the first available item slot.
    /// </summary>
    /// <param name="item">Item to add</param>
    /// <returns>Leftover item number, that did not fit in the inventory</returns>
    public int AddItem(ItemSO item)
    {
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            int leftover = ItemSlots[i].AddItem(item);
            if (leftover == 0)
            {
                return 0;
            }
            item.CurrentStack = leftover;
        }
        return item.CurrentStack;
    }

    public int RemoveItem(ItemSO item)
    {
    }
}