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

    public int RemoveItem(ItemSO itemToRemove, bool removeFromMultiple = false)
    {
        if (Item == null || (itemToRemove.CurrentStack > Item.CurrentStack && !removeFromMultiple))
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