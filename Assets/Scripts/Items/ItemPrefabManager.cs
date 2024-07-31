using UnityEngine;

public class ItemPrefabManager : MonoBehaviour
{
    public static ItemPrefabManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("ItemSOManager instance already exists, destroying the new one.");
            Destroy(gameObject);
        }
    }

    public GameObject SpawnItemById(string id, int quantity, Vector3 location)
    {
        var prefab = Resources.Load($"Prefabs/Items/{id}");
        var Item = Instantiate(prefab, location, Quaternion.identity, null) as GameObject;
        Item.GetComponent<ItemManager>().SetQuantity(quantity);
        Item.GetComponent<ItemManager>().IsPickupable = false;
        return Item;
    }

    public void SpawnTest()
    {
        SpawnItemById("basic_torch", 1, new Vector3(0, 0, 0));
    }
}