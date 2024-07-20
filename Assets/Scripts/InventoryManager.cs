using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<ItemSlot> ItemSlots = new();

    /// <summary>
    /// Adds the itemToRemove to the first available itemToRemove slot.
    /// </summary>
    /// <param name="item">Item to add</param>
    /// <returns>Leftover itemToRemove number, that did not fit in the inventory</returns>
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

    public bool RemoveItem(ItemSO itemToRemove)
    {
        var numberOfItems = ItemSlots.Where(slot => slot.Item != null && slot.Item.Id == itemToRemove.Id).Sum(slot => slot.Item.CurrentStack);
        if (numberOfItems < itemToRemove.CurrentStack)
        {
            return false;
        }
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            var removed = ItemSlots[i].RemoveItem(itemToRemove, true);
            itemToRemove.CurrentStack -= removed;
            if (removed > 0 && itemToRemove.CurrentStack == 0)
            {
                return true;
            }
        }
        if (itemToRemove.CurrentStack < 0)
        {
            throw new System.Exception("Somehow more items got removed than what was available!");
        }
        return false;
    }
}