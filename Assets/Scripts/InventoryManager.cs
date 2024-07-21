using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<ItemSlot> ItemSlots = new();
    public InventoryManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple InventoryManagers found in the scene. Destroying the new one.");
            Destroy(gameObject);
        }
    }

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

    /// <summary>
    /// Removes the itemToRemove from the inventory.
    /// </summary>
    /// <param name="itemToRemove">Item to remove</param>
    /// <returns>True if successfully removed, false if could not remove</returns>
    /// <exception cref="System.Exception">Throws exception if we removed more items than available</exception>
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

    /// <summary>
    /// Moves item from one slot to another.
    /// </summary>
    /// <param name="from">Slot to move from</param>
    /// <param name="to">Slot to move to</param>
    public void MoveItem(ItemSlot from, ItemSlot to)
    {
        if (from.Item == null)
        {
            return;
        }
        if (to.Item == null)
        {
            to.Item = from.Item;
            from.Item = null;
            return;
        }
        if (from.Item.Id == to.Item.Id)
        {
            int leftover = to.AddItem(from.Item);
            from.Item.CurrentStack = leftover;
            if (leftover == 0)
            {
                from.Item = null;
            }
        }
        else
        {
            (to.Item, from.Item) = (from.Item, to.Item);
        }
    }
}