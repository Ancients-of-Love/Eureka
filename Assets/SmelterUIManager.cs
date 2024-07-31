using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmelterUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject InventoryUISlotPrefab;

    [SerializeField]
    private GameObject InventoryContentHolder;

    [SerializeField]
    private List<InventoryUISlot> InventoryUISlots;

    [SerializeField]
    public InventoryUISlot OreUISlot;
    public ItemSlot OreSlot;

    [SerializeField]
    public InventoryUISlot FuelUISlot;
    public ItemSlot FuelSlot;

    [SerializeField]
    private bool SplitStack = false;

    private ItemSlot SelectedItemSlot;
    private ItemSO SelectedItem;
    private int SelectedItemCount = 0;
    private int HoveringSlotIndex = -1;

    private void Awake()
    {
        OreUISlot.SetItemSlotReference(OreSlot);
        OreUISlot.OnItemDroppedOn += HandleItemDropped;
        OreUISlot.OnItemBeginDrag += HandleItemBeginDrag;
        OreUISlot.OnItemEndDrag += HandleItemEndDrag;
        OreUISlot.OnMouseEnter += HandleMouseEnter;
        OreUISlot.OnMouseExit += HandleMouseExit;

        FuelUISlot.SetItemSlotReference(FuelSlot);
        FuelUISlot.OnItemDroppedOn += HandleItemDropped;
        FuelUISlot.OnItemBeginDrag += HandleItemBeginDrag;
        FuelUISlot.OnItemEndDrag += HandleItemEndDrag;
        FuelUISlot.OnMouseEnter += HandleMouseEnter;
        FuelUISlot.OnMouseExit += HandleMouseExit;
    }

    private void OnEnable()
    {
        foreach (var slot in InventoryManager.Instance.ItemSlots)
        {
            if (slot.Item is BurnableItemSO || slot.Item is SmeltableItemSO)
            {
                var UISlot = Instantiate(InventoryUISlotPrefab, InventoryContentHolder.transform).GetComponent<InventoryUISlot>();
                UISlot.SetItemSlotReference(slot);
                UISlot.OnItemDroppedOn += HandleItemDropped;
                UISlot.OnItemBeginDrag += HandleItemBeginDrag;
                UISlot.OnItemEndDrag += HandleItemEndDrag;
                UISlot.OnMouseEnter += HandleMouseEnter;
                UISlot.OnMouseExit += HandleMouseExit;
                InventoryUISlots.Add(UISlot);
            }
            if (InventoryUISlots.Count >= 4)
            {
                break;
            }
        }

        if (InventoryUISlots.Count == 1)
        {
            var slot = InventoryManager.Instance.ItemSlots.Find(e => e.Item == null);
            if (slot != null)
            {
                var UISlot = Instantiate(InventoryUISlotPrefab, InventoryContentHolder.transform).GetComponent<InventoryUISlot>();
                UISlot.SetItemSlotReference(slot);
                UISlot.OnItemDroppedOn += HandleItemDropped;
                UISlot.OnItemBeginDrag += HandleItemBeginDrag;
                UISlot.OnItemEndDrag += HandleItemEndDrag;
                UISlot.OnMouseEnter += HandleMouseEnter;
                UISlot.OnMouseExit += HandleMouseExit;
                InventoryUISlots.Add(UISlot);
            }
        }
        MouseFollower.Instance.Toggle(false);
    }

    private void OnDisable()
    {
        foreach (var item in InventoryUISlots)
        {
            Destroy(item.gameObject);
        }
        InventoryUISlots.Clear();
    }

    private void HandleMouseExit(InventoryUISlot slot)
    {
        HoveringSlotIndex = -1;
    }

    private void HandleMouseEnter(InventoryUISlot slot)
    {
        HoveringSlotIndex = InventoryUISlots.IndexOf(slot);
    }

    private void HandleItemEndDrag(InventoryUISlot slot)
    {
        Debug.Log(slot.name);

        MouseFollower.Instance.Toggle(false);
        SelectedItemSlot = null;
    }

    private void HandleItemBeginDrag(InventoryUISlot slot)
    {
        MouseFollower.Instance.Toggle(true);
        if (Input.GetMouseButton(0))
        {
            MouseFollower.Instance.SetData(slot.Image.sprite, Int32.Parse(slot.UIText.text));
            SplitStack = false;
        }
        if (Input.GetMouseButton(1))
        {
            MouseFollower.Instance.SetData(slot.Image.sprite, Int32.Parse(slot.UIText.text) / 2);
            SplitStack = true;
        }

        SelectedItemSlot = slot.ItemSlotReference;
        SelectedItem = SelectedItemSlot.Item;
        SelectedItemCount = SplitStack ? SelectedItemSlot.ItemCount / 2 : SelectedItemSlot.ItemCount;
    }

    private void HandleItemDropped(InventoryUISlot slot)
    {
        var localSlot = slot.ItemSlotReference;
        if (!SplitStack)
        {
            HandleSwap(SelectedItemSlot, localSlot);
        }
        else
        {
            HandleSplit(SelectedItemSlot, localSlot);
        }
        MouseFollower.Instance.Toggle(false);
    }

    private void HandleSwap(ItemSlot beginSlot, ItemSlot endSlot)
    {
        if (beginSlot == endSlot || (endSlot == FuelSlot && !(beginSlot.Item.GetType() == typeof(BurnableItemSO))) || (endSlot == OreSlot && !(beginSlot.Item.GetType() == typeof(SmeltableItemSO))))
        {
            return;
        }
        Debug.Log("HandleSwap!");
        var SlotA = beginSlot;
        var SlotB = endSlot;

        InventoryManager.Instance.MoveItem(SlotA, SlotB);
    }

    private void HandleSplit(ItemSlot beginSlot, ItemSlot endSlot)
    {
        var SlotA = beginSlot;
        var SlotB = endSlot;
        Debug.Log("HandleSplit!");
        if (SelectedItemCount <= 0 || SlotA.Item != SlotB.Item && SlotB.Item != null || beginSlot == endSlot || (endSlot == FuelSlot && !(beginSlot.Item.GetType() == typeof(BurnableItemSO))) || (endSlot == OreSlot && !(beginSlot.Item.GetType() == typeof(SmeltableItemSO))))
        {
            return;
        }

        SlotA.RemoveItem(SelectedItem, SelectedItemCount);
        SlotB.AddItem(SelectedItem, SelectedItemCount);
    }
}