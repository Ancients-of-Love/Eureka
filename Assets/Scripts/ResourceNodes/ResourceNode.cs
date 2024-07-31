using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ResourceNode : MonoBehaviour
{
    public ResourceNodeSO ResourceNodeData;
    public int CurrentHealth;
    public int MaxHealth;
    public float MaxMoveDistance = 5f;
    public List<ItemAmount> RemainingDropTable;
    public bool IsPlayerClose = false;

    private void Awake()
    {
        CurrentHealth = ResourceNodeData.Health;
        MaxHealth = ResourceNodeData.Health;
        RemainingDropTable = ResourceNodeData.DropTable.ToList();
    }

    public void TakeDamage(int amount)
    {
        var randomTreshold = Random.Range(0, CurrentHealth);
        CurrentHealth -= amount;
        if (CurrentHealth <= 0)
        {
            Die();
        }
        else if (CurrentHealth <= randomTreshold)
        {
            // Drop random loot from loottable
            var dropIndex = Random.Range(0, RemainingDropTable.Count);
            var drop = RemainingDropTable[dropIndex];
            SpawnItem(drop.Item.Id, 1);
            drop.Amount--;
            RemainingDropTable[dropIndex] = drop;
            if (RemainingDropTable[dropIndex].Amount <= 0)
            {
                RemainingDropTable.Remove(drop);
            }
        }
    }

    private void SpawnItem(string id, int count)
    {
        var spawnedItem = ItemPrefabManager.Instance.SpawnItemById(id, count, transform.position);
        var newPosition = new Vector3(transform.position.x + Random.Range(-MaxMoveDistance, MaxMoveDistance), transform.position.y + Random.Range(-MaxMoveDistance, MaxMoveDistance), transform.position.z);
        var itemManager = spawnedItem.GetComponentInChildren<ItemManager>();
        itemManager.OnSpawn(newPosition);
    }

    public void Die()
    {
        try
        {
            foreach (var drop in RemainingDropTable)
            {
                SpawnItem(drop.Item.Id, drop.Amount);
            }
        }
        catch (System.Exception e)
        {
            throw e;
        }
        finally
        {
            PlayerInteractManager.Instance.RemoveNearResource(this);
            Destroy(gameObject);
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerClose = true;
            PlayerInteractManager.Instance.AddNearResource(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerClose = false;
            PlayerInteractManager.Instance.RemoveNearResource(this);
        }
    }
}