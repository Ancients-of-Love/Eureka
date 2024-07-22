using UnityEngine;

public class ItemSlot : MonoBehaviour
{
#nullable enable
    public ItemSO? Item;
    public int ItemCount = 0;

    /// <summary>
    /// Adds the itemToRemove to the itemToRemove slot.
    /// </summary>
    /// <param name="item">The itemToRemove to add</param>
    /// <param name="addItemCount">Number of items to add</param>
    /// <returns>Number of leftover items</returns>
    public int AddItem(ItemSO item, int addItemCount)
    {
        if (Item == null)
        {
            Item = item;
            ItemCount = addItemCount;
            return 0;
        }
        else if (Item.Id == item.Id)
        {
            if (Item.IsStackable && addItemCount < Item.MaxStack)
            {
                int futureStack = ItemCount + addItemCount;
                if (futureStack > Item.MaxStack)
                {
                    ItemCount = Item.MaxStack;
                    return futureStack - Item.MaxStack;
                }
                else
                {
                    ItemCount = futureStack;
                    return 0;
                }
            }
        }
        return addItemCount;
    }

    /// <summary>
    /// Removes the itemToRemove from the slot.
    /// </summary>
    /// <param name="itemToRemove">Item to remove</param>
    /// <param name="removeItemCount">Number of items to remove</param>
    /// <param name="removeIfMoreThanCurrentStack">Flag if we want to remove items even if it is more than the current stack</param>
    /// <returns>Number of items removed</returns>
    public int RemoveItem(ItemSO itemToRemove, int removeItemCount, bool removeIfMoreThanCurrentStack = false)
    {
        if (Item == null || (removeItemCount > ItemCount && !removeIfMoreThanCurrentStack))
        {
            return 0;
        }
        else if (Item.Id == itemToRemove.Id)
        {
            int removed = 0;
            if (Item.IsStackable)
            {
                if (ItemCount <= removeItemCount)
                {
                    removed = ItemCount;
                    Item = null;
                }
                else
                {
                    removed = removeItemCount;
                    ItemCount -= removeItemCount;
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