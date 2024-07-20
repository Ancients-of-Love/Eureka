using UnityEngine;

public class ItemSlot : MonoBehaviour
{
#nullable enable
    public ItemSO? Item;

    /// <summary>
    /// Adds the item to the item slot.
    /// </summary>
    /// <param name="item">The item to add</param>
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

#nullable disable
}