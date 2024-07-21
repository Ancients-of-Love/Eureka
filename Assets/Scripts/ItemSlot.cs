using UnityEngine;

public class ItemSlot : MonoBehaviour
{
#nullable enable
    public ItemSO? Item;

    /// <summary>
    /// Adds the itemToRemove to the itemToRemove slot.
    /// </summary>
    /// <param name="item">The itemToRemove to add</param>
    /// <returns>Number of leftover items</returns>
    public int AddItem(ItemSO item)
    {
        if (Item == null)
        {
            Item = item;
            return 0;
        }
        else if (Item.Id == item.Id)
        {
            if (Item.IsStackable && Item.CurrentStack < Item.MaxStack)
            {
                int futureStack = Item.CurrentStack + item.CurrentStack;
                if (futureStack > Item.MaxStack)
                {
                    Item.CurrentStack = Item.MaxStack;
                    return futureStack - Item.MaxStack;
                }
            }
        }
        return item.CurrentStack;
    }

    /// <summary>
    /// Removes the itemToRemove from the slot.
    /// </summary>
    /// <param name="itemToRemove">Item to remove</param>
    /// <param name="removeIfMoreThanCurrentStack">Flag if we want to remove items even if it is more than the current stack</param>
    /// <returns>Number of items removed</returns>
    public int RemoveItem(ItemSO itemToRemove, bool removeIfMoreThanCurrentStack = false)
    {
        if (Item == null || (itemToRemove.CurrentStack > Item.CurrentStack && !removeIfMoreThanCurrentStack))
        {
            return 0;
        }
        else if (Item.Id == itemToRemove.Id)
        {
            int removed = 0;
            if (Item.IsStackable)
            {
                if (Item.CurrentStack <= itemToRemove.CurrentStack)
                {
                    removed = Item.CurrentStack;
                    Item = null;
                }
                else
                {
                    removed = itemToRemove.CurrentStack;
                    Item.CurrentStack -= itemToRemove.CurrentStack;
                }
            }
            else
            {
                Item = null;
                removed = 1;
            }
            return removed;
        }
        return 0;
    }

#nullable disable
}