using System.Collections.Generic;
using UnityEditor;
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
        Object prefab = AssetDatabase.LoadAssetAtPath($"Assets/Prefabs/Items/{id}.prefab", typeof(GameObject));
        return Instantiate(prefab, location, Quaternion.identity, null) as GameObject;
    }
}