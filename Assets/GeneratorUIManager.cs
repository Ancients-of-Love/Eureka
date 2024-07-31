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
    private ItemSlot GeneratorItemInventorySlot;

    [SerializeField]
    private MouseFollower MouseFollower;

    private bool SplitStack = false;
    private int SelectedItemIndex = -1;
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
        foreach (var item in InventoryUISlots)
        {
            Destroy(item);
        }
        InventoryUISlots.Clear();
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
        }

        MouseFollower.Toggle(false);
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

        SelectedItemIndex = InventoryUISlots.IndexOf(slot);
        SelectedItem = InventoryManager.Instance.ItemSlots[SelectedItemIndex].Item;
        SelectedItemCount = InventoryManager.Instance.ItemSlots[SelectedItemIndex].ItemCount / 2;
    }

    private void HandleItemDropped(InventoryUISlot slot)
    {
        /*if (!SplitStack)
        {
            HandleSwap(SelectedItemIndex, InventoryUISlots.IndexOf(slot));
        }
        else
        {
            HandleSplit(SelectedItemIndex, InventoryUISlots.IndexOf(slot));
        }
        MouseFollower.Toggle(false);
        SelectedItemIndex = -1;*/
    }

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}