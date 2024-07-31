using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject InventoryUISlotPrefab;

    [SerializeField]
    private GameObject InventoryContentHolder;

    [SerializeField]
    private List<InventoryUISlot> InventoryUISlots;

    [SerializeField]
    private InventoryUISlot GeneratorUISlot;

    [SerializeField]
    public ItemSlot GeneratorItemInventorySlot;

    [SerializeField]
    private MouseFollower MouseFollower;

    [SerializeField]
    private bool SplitStack = false;

    private ItemSlot SelectedItemSlot;
    private ItemSO SelectedItem;
    private int SelectedItemCount = 0;
    private int HoveringSlotIndex = -1;

    private void Awake()
    {
        GeneratorUISlot.SetItemSlotReference(GeneratorItemInventorySlot);
        GeneratorUISlot.OnItemDroppedOn += HandleItemDropped;
        GeneratorUISlot.OnItemBeginDrag += HandleItemBeginDrag;
        GeneratorUISlot.OnItemEndDrag += HandleItemEndDrag;
        GeneratorUISlot.OnMouseEnter += HandleMouseEnter;
        GeneratorUISlot.OnMouseExit += HandleMouseExit;
    }

    private void OnEnable()
    {
        foreach (var slot in InventoryManager.Instance.ItemSlots)
        {
            if (slot.Item is BurnableItemSO)
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
        MouseFollower.Toggle(false);
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

        MouseFollower.Toggle(false);
        SelectedItemSlot = null;
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

        if (slot == GeneratorUISlot)
        {
            SelectedItemSlot = slot.ItemSlotReference;
            SelectedItem = GeneratorItemInventorySlot.Item;
            SelectedItemCount = SplitStack ? GeneratorItemInventorySlot.ItemCount : GeneratorItemInventorySlot.ItemCount;
        }

        if (InventoryUISlots.Contains(slot))
        {
            SelectedItemSlot = slot.ItemSlotReference;
            SelectedItem = SelectedItemSlot.Item;
            SelectedItemCount = SplitStack ? SelectedItemSlot.ItemCount / 2 : SelectedItemSlot.ItemCount;
        }
    }

    private void HandleItemDropped(InventoryUISlot slot)
    {
        var localSlot = new ItemSlot();
        if (slot == GeneratorUISlot)
        {
            localSlot = GeneratorUISlot.ItemSlotReference;
        }
        if (InventoryUISlots.Contains(slot))
        {
            localSlot = slot.ItemSlotReference;
        }

        if (!SplitStack)
        {
            HandleSwap(SelectedItemSlot, localSlot);
        }
        else
        {
            HandleSplit(SelectedItemSlot, localSlot);
        }
        MouseFollower.Toggle(false);
    }

    private void HandleSwap(ItemSlot beginSlot, ItemSlot endSlot)
    {
        if (beginSlot == endSlot)
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
        if (SelectedItemCount <= 0 || SlotA.Item != SlotB.Item && SlotB.Item != null || beginSlot == endSlot)
        {
            return;
        }

        SlotA.RemoveItem(SelectedItem, SelectedItemCount);
        SlotB.AddItem(SelectedItem, SelectedItemCount);
    }
}