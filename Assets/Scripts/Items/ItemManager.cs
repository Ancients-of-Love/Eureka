using UnityEngine;

public class ItemManager : MonoBehaviour, IPickupable
{
    public ItemSO Item;
    public int Quantity;
    public float Speed = 0.5f;
    public float PickUpDistance = 0.1f;
    public bool Move;
    public bool IsPickupable = true;

    private SpriteRenderer SpriteRenderer;
    private TMPro.TextMeshProUGUI UITextMeshPro;

    private void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        UITextMeshPro = GetComponentInChildren<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (UITextMeshPro != null)
        {
            UITextMeshPro.text = Quantity.ToString();
        }
        if (Vector3.Distance(transform.position, GameObject.FindWithTag("Player").transform.position) < PickUpDistance && IsPickupable)
        {
            OnPickup();
        }
    }

    private void FixedUpdate()
    {
        if (Move && IsPickupable)
        {
            transform.position = Vector3.MoveTowards(transform.position, GameObject.FindWithTag("Player").transform.position, Speed * Time.fixedDeltaTime);
            Speed += 0.1f;
        }
    }

    public void SetQuantity(int quantity)
    {
        Quantity = quantity;
    }

    public int OnPickup()
    {
        var itemsLeftOver = InventoryManager.Instance.AddItem(Item, Quantity);
        if (itemsLeftOver > 0)
        {
            Quantity = itemsLeftOver;
            return itemsLeftOver;
        }
        Destroy(gameObject);
        return 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Move = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Move = false;
            Speed = 0.5f;
        }
        if (!IsPickupable)
        {
            IsPickupable = true;
        }
    }
}