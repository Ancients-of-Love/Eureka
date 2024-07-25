using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] private List<ItemSlot> ItemSlots = new();
    public static InventoryManager Instance;

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
    /// <param name="addItemCount">Number of items to add</param>
    /// <returns>Leftover itemToRemove number, that did not fit in the inventory</returns>
    public int AddItem(ItemSO item, int addItemCount)
    {
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            int leftover = ItemSlots[i].AddItem(item, addItemCount);
            if (leftover == 0)
            {
                return 0;
            }
            addItemCount = leftover;
        }
        return addItemCount;
    }

    /// <summary>
    /// Removes the itemToRemove from the inventory.
    /// </summary>
    /// <param name="itemToRemove">Item to remove</param>
    /// <param name="removeItemCount">Number of items to remove</param>
    /// <returns>True if successfully removed, false if could not remove</returns>
    /// <exception cref="System.Exception">Throws exception if we removed more items than available</exception>
    public bool RemoveItem(ItemSO itemToRemove, int removeItemCount)
    {
        var numberOfItems = ItemSlots.Where(slot => slot.Item != null && slot.Item.Id == itemToRemove.Id).Sum(slot => slot.ItemCount);
        if (numberOfItems < removeItemCount)
        {
            return false;
        }
        for (int i = 0; i < ItemSlots.Count; i++)
        {
            var removed = ItemSlots[i].RemoveItem(itemToRemove, removeItemCount, true);
            removeItemCount -= removed;
            if (removed > 0 && removeItemCount == 0)
            {
                return true;
            }
        }
        if (removeItemCount < 0)
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
            int leftover = to.AddItem(from.Item, from.ItemCount);
            from.ItemCount = leftover;
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

    /// <summary>
    /// Checks if the inventory has enough items.
    /// </summary>
    /// <param name="items">Items to check</param>
    /// <returns>True if it has enough items, false if anything's missing.</returns>
    public bool HasEnoughItems(List<ItemAmount> items)
    {
        foreach (var item in items)
        {
            var numberOfItems = ItemSlots.Where(slot => slot.Item != null && slot.Item.Id == item.Item.Id).Sum(slot => slot.ItemCount);
            if (numberOfItems < item.Amount)
            {
                return false;
            }
        }
        return true;
    }

    public bool IsInventoryFull(List<ItemAmount> resultItem)
    {
        if (resultItem.Count >= ItemSlots.Count(x => x.Item == null))
        {
            return true;
        }
        return false;
    }
}