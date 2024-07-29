using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [SerializeField]
    private Canvas Canvas;

    [SerializeField]
    private InventoryUISlot InventoryUISlot;

    private void Awake()
    {
        Canvas = transform.GetComponentInParent<Canvas>();
        InventoryUISlot = GetComponentInChildren<InventoryUISlot>();
    }

    public void SetData(Sprite sprite, int quantity)
    {
        InventoryUISlot.SetAmountText(quantity.ToString());
        InventoryUISlot.SetSpriteOfSlot(sprite);
    }

    private void Update()
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)Canvas.transform, Input.mousePosition, Canvas.worldCamera, out position);
        transform.position = Canvas.transform.TransformPoint(position);
    }

    public void Toggle(bool toggle)
    {
        gameObject.SetActive(toggle);
    }
}