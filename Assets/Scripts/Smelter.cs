using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Smelter : MonoBehaviour
{
    public ItemSlot InputSlot;
    public ItemSlot FuelSlot;
    public float MaxBurnTime = 100;
    public float CurrentBurnTime = 0;
    public float MaxMoveDistance = 1;
    public ItemSO Output;
    public Timer SmeltTimer;
    public bool IsSmelting = false;

    // Start is called before the first frame update
    private void Start()
    {
        SmeltTimer = new Timer();
    }

    // Update is called once per frame
    private void Update()
    {
        if (FuelSlot.Item != null)
        {
            AddFuelTime(FuelSlot);
        }
        if (CurrentBurnTime > 0)
        {
            Smelt();
        }
        if (IsSmelting)
        {
            SmeltTimer.Tick(Time.deltaTime);
        }
        if (CurrentBurnTime > 0 && IsSmelting)
        {
            CurrentBurnTime -= Time.deltaTime;
        }
    }

    private void Smelt()
    {
        if (InputSlot.Item != null)
        {
            var smeltableItem = InputSlot.Item as SmeltableItemSO;
            if (smeltableItem != null && smeltableItem.SmeltTime <= CurrentBurnTime)
            {
                IsSmelting = true;
                SmeltTimer.SetMaxTime(smeltableItem.SmeltTime);
            }
            if (SmeltTimer.RemainingTime <= 0 && IsSmelting)
            {
                IsSmelting = false;
                SpawnItem(Output.Id, 1);
                InputSlot.RemoveItem(smeltableItem, 1);
            }
        }
        else
        {
            IsSmelting = false;
        }
    }

    private void SpawnItem(string id, int count)
    {
        var spawnedItem = ItemPrefabManager.Instance.SpawnItemById(id, count, transform.position);
        var newPosition = new Vector3(transform.position.x + Random.Range(-MaxMoveDistance, MaxMoveDistance), transform.position.y + Random.Range(-MaxMoveDistance, MaxMoveDistance), transform.position.z);
        var itemManager = spawnedItem.GetComponentInChildren<ItemManager>();
        itemManager.OnSpawn(newPosition);
    }

    private void AddFuelTime(ItemSlot burnableItemSlot)
    {
        var burnableItem = burnableItemSlot.Item as BurnableItemSO;
        var maxItemCount = (int)((MaxBurnTime - CurrentBurnTime) / burnableItem.BurnTime);
        if (maxItemCount > 0)
        {
            if (maxItemCount <= burnableItemSlot.ItemCount)
            {
                burnableItemSlot.RemoveItem(burnableItem, maxItemCount);
                CurrentBurnTime += burnableItem.BurnTime * maxItemCount;
            }
            else
            {
                burnableItemSlot.RemoveItem(burnableItem, burnableItemSlot.ItemCount);
                CurrentBurnTime += burnableItem.BurnTime * burnableItemSlot.ItemCount;
            }
        }
    }
}