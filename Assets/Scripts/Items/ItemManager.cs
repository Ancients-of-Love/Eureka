using UnityEngine;

public class ItemManager : MonoBehaviour, IPickupable
{
    public ItemSO Item;
    public int Quantity;
    public float Speed = 0.5f;

    public float DropSpeed = 1f;
    public float PickUpDistance = 0.1f;

    public bool Move;
    public bool IsPickupable = true;
    public bool IsRecentlySpawned = true;
    public Vector3 GoalPosition;

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
        if (IsRecentlySpawned)
        {
            transform.position = Vector3.MoveTowards(transform.position, GoalPosition, DropSpeed * Time.deltaTime);
            if (transform.position == GoalPosition)
            {
                IsRecentlySpawned = false;
                IsPickupable = true;
            }
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

    public void OnSpawn(Vector3 newPosition)
    {
        GoalPosition = newPosition;
        IsPickupable = false;
        IsRecentlySpawned = true;
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