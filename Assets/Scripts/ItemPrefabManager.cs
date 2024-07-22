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

    public GameObject SpawnItemById(string id, Vector3 location)
    {
        var prefab = Resources.Load($"Prefabs/Items/{id}");
        return Instantiate(prefab, location, Quaternion.identity, null) as GameObject;
    }

    public void SpawnTest()
    {
        SpawnItemById("basic_torch", new Vector3(0, 0, 0));
    }
}