using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIManager : Singleton<InventoryUIManager>
{
    [SerializeField]
    private GameObject InventoryUISlotPrefab;

    [SerializeField]
    private GameObject InventoryGrid;

    public List<InventoryUISlot> InventoryUISlotList;

    [SerializeField]
    public MouseFollower MouseFollower;

    private bool SplitStack = false;
    private int SelectedItemIndex = -1;
    private ItemSO SelectedItem;
    private int SelectedItemCount = 0;
    private int HoveringSlotIndex = -1;

    private void Start()
    {
        foreach (var ItemSlot in InventoryManager.Instance.ItemSlots)
        {
            var UISlot = Instantiate(InventoryUISlotPrefab, InventoryGrid.transform).GetComponent<InventoryUISlot>();
            UISlot.SetItemSlotReference(ItemSlot);
            UISlot.OnItemClicked += HandleItemClicked;
            UISlot.OnItemDroppedOn += HandleItemDropped;
            UISlot.OnRightMouseButton += HandeItemRightClicked;
            UISlot.OnItemBeginDrag += HandleItemBeginDrag;
            UISlot.OnItemEndDrag += HandleItemEndDrag;
            UISlot.OnMouseEnter += HandleMouseEnter;
            UISlot.OnMouseExit += HandleMouseExit;
            InventoryUISlotList.Add(UISlot);
        }

        MouseFollower.Toggle(false);
    }

    private void HandleMouseExit(InventoryUISlot slot)
    {
        HoveringSlotIndex = -1;
    }

    private void HandleMouseEnter(InventoryUISlot slot)
    {
        HoveringSlotIndex = InventoryUISlotList.IndexOf(slot);
    }

    private void HandleItemEndDrag(InventoryUISlot slot)
    {
        Debug.Log(slot.name);

        MouseFollower.Toggle(false);
        SelectedItemIndex = -1;
    }

    private void HandleItemBeginDrag(InventoryUISlot slot)
    {
        MouseFollower.Toggle(true);
        if (Input.GetMouseButton(0))
        {
            MouseFollower.SetData(slot.Image.sprite, Int32.Parse(slot.UIText.text));
            SplitStack = false;
        }
        if (Input.GetMouseButton(1))
        {
            MouseFollower.SetData(slot.Image.sprite, Int32.Parse(slot.UIText.text) / 2);
            SplitStack = true;
        }

        SelectedItemIndex = InventoryUISlotList.IndexOf(slot);
        SelectedItem = InventoryManager.Instance.ItemSlots[SelectedItemIndex].Item;
        SelectedItemCount = InventoryManager.Instance.ItemSlots[SelectedItemIndex].ItemCount / 2;
    }

    private void HandleItemDropped(InventoryUISlot slot)
    {
        if (!SplitStack)
        {
            HandleSwap(SelectedItemIndex, InventoryUISlotList.IndexOf(slot));
        }
        else
        {
            HandleSplit(SelectedItemIndex, InventoryUISlotList.IndexOf(slot));
        }
        MouseFollower.Toggle(false);
        SelectedItemIndex = -1;
    }

    private void HandeItemRightClicked(InventoryUISlot slot)
    {
    }

    private void HandleItemClicked(InventoryUISlot slot)
    {
        //TODO: SOMETHING
    }

    private void HandleSwap(int indexOfBegin, int indexOfEnd)
    {
        if (indexOfBegin == indexOfEnd)
        {
            return;
        }
        var SlotA = InventoryManager.Instance.ItemSlots[indexOfBegin];
        var SlotB = InventoryManager.Instance.ItemSlots[indexOfEnd];
        InventoryManager.Instance.MoveItem(SlotA, SlotB);
    }

    private void HandleSplit(int indexOfBegin, int indexOfEnd)
    {
        var SlotA = InventoryManager.Instance.ItemSlots[indexOfBegin];
        var SlotB = InventoryManager.Instance.ItemSlots[indexOfEnd];

        if (SelectedItemCount <= 0 || SlotA.Item != SlotB.Item && SlotB.Item != null || indexOfBegin == indexOfEnd)
        {
            return;
        }

        SlotA.RemoveItem(SelectedItem, SelectedItemCount);
        SlotB.AddItem(SelectedItem, SelectedItemCount);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O) && HoveringSlotIndex != -1)
        {
            var SlotA = InventoryManager.Instance.ItemSlots[HoveringSlotIndex];
            ItemPrefabManager.Instance.SpawnItemById(SlotA.Item.Id, SlotA.ItemCount, PlayerInteractManager.Instance.Player.transform.position);
            SlotA.RemoveItem(SlotA.Item, SlotA.ItemCount);
        }
    }
}