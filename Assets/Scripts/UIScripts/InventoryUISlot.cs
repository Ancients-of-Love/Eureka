using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUISlot : MonoBehaviour, IPointerClickHandler, IDropHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    public Image TransformImage;

    [SerializeField]
    public Image Image;

    [SerializeField]
    public TMPro.TextMeshProUGUI UIText;

    [SerializeField]
    public ItemSlot ItemSlotReference;

    private bool empty = true;
    private bool hovering = false;

    private Color CurrentColor;
    private Color HoverColor = Color.gray;

    public event Action<InventoryUISlot> OnItemClicked, OnItemBeginDrag, OnItemEndDrag, OnItemDroppedOn, OnRightMouseButton, OnDragExited, OnItemHoverHandler, OnMouseEnter, OnMouseExit;

    public void SetAmountText(string amount)
    {
        UIText.text = amount;
    }

    public void SetSpriteOfSlot(Sprite sprite)
    {
        Image.sprite = sprite;
    }

    public void SetItemSlotReference(ItemSlot itemSlot)
    {
        ItemSlotReference = itemSlot;
    }

    private void Awake()
    {
        TransformImage = GetComponent<Image>();
        CurrentColor = TransformImage.color;
    }

    private void Update()
    {
        if (ItemSlotReference != null)
        {
            if (ItemSlotReference.Item != null)
            {
                Image.enabled = true;
                Image.sprite = ItemSlotReference.Item.Sprite;
                UIText.text = ItemSlotReference.ItemCount.ToString();
                empty = false;
            }
            else
            {
                Image.sprite = null;
                UIText.text = "";
                Image.enabled = false;
                empty = true;
            }
        }
    }

    public void OnClicked()
    {
        if (empty)
        {
            return;
        }
        OnItemClicked?.Invoke(this);
    }

    public void OnPointerClick(PointerEventData pointerData)
    {
        if (empty)
        {
            return;
        }
        if (pointerData.button == PointerEventData.InputButton.Right)
        {
            OnRightMouseButton?.Invoke(this);
        }
        else
        {
            OnItemClicked?.Invoke(this);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (empty)
        {
            return;
        }
        OnItemBeginDrag?.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        OnItemEndDrag?.Invoke(this);
    }

    public void OnDrop(PointerEventData eventData)
    {
        OnItemDroppedOn?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //LEAVE EMPTY
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        TransformImage.color = HoverColor;
        OnMouseEnter?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TransformImage.color = CurrentColor;
        OnMouseExit?.Invoke(this);
    }
}