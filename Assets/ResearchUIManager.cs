using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResearchUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject InventoryUISlotPrefab;

    [SerializeField]
    private GameObject InventoryContentHolder;

    [SerializeField]
    private List<InventoryUISlot> InventoryUISlots;

    [SerializeField]
    private InventoryUISlot ResearchUISlot;

    [SerializeField]
    public ItemSlot ResearchItemInventorySlot;

    [SerializeField]
    private MouseFollower MouseFollower;

    [SerializeField]
    private bool SplitStack = false;

    [SerializeField]
    private Button Button;

    private ItemSlot SelectedItemSlot;
    private ItemSO SelectedItem;
    private int SelectedItemCount = 0;
    private int HoveringSlotIndex = -1;

    private void Awake()
    {
        ResearchUISlot.SetItemSlotReference(ResearchItemInventorySlot);
        ResearchUISlot.OnItemDroppedOn += HandleItemDropped;
        ResearchUISlot.OnItemBeginDrag += HandleItemBeginDrag;
        ResearchUISlot.OnItemEndDrag += HandleItemEndDrag;
        ResearchUISlot.OnMouseEnter += HandleMouseEnter;
        ResearchUISlot.OnMouseExit += HandleMouseExit;

        Button.onClick.AddListener(Research);
    }

    private void OnEnable()
    {
        foreach (var slot in InventoryManager.Instance.ItemSlots)
        {
            if (slot.Item != null && !slot.Item.IsResearched)
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

        var emptySlot = InventoryManager.Instance.ItemSlots.Find(e => e.Item == null);
        if (emptySlot != null)
        {
            var UISlot = Instantiate(InventoryUISlotPrefab, InventoryContentHolder.transform).GetComponent<InventoryUISlot>();
            UISlot.SetItemSlotReference(emptySlot);
            UISlot.OnItemDroppedOn += HandleItemDropped;
            UISlot.OnItemBeginDrag += HandleItemBeginDrag;
            UISlot.OnItemEndDrag += HandleItemEndDrag;
            UISlot.OnMouseEnter += HandleMouseEnter;
            UISlot.OnMouseExit += HandleMouseExit;
            InventoryUISlots.Add(UISlot);
        }

        MouseFollower.Toggle(false);
    }

    private void Research()
    {
        var item = ResearchItemInventorySlot.Item;
        BlueprintManager.Instance.ResearchItem(item);
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
        }

        SelectedItemSlot = slot.ItemSlotReference;
        SelectedItem = SelectedItemSlot.Item;
        SelectedItemCount = SelectedItemSlot.ItemCount;
    }

    private void HandleItemDropped(InventoryUISlot slot)
    {
        var localSlot = new ItemSlot();
        if (slot == ResearchUISlot)
        {
            localSlot = ResearchUISlot.ItemSlotReference;
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