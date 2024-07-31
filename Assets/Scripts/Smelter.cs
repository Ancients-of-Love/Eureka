using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Smelter : Building
{
    public float MaxBurnTime = 100;
    public float CurrentBurnTime = 0;
    public float MaxMoveDistance = 1;
    public ItemSO Output;
    public Timer SmeltTimer;
    public bool IsSmelting = false;

    [SerializeField]
    private GameObject UIPrefab;
    private bool IsUIActive = true;
    private GameObject ActiveUI = null;
    private SmelterUIManager SmelterUI = null;

    private Animator Animator;

    // Start is called before the first frame update
    private new void Start()
    {
        Animator = GetComponentInChildren<Animator>();
        base.Start();
        SmeltTimer = new Timer();
    }

    // Update is called once per frame
    private void Update()
    {
        if (SmelterUI == null)
        {
            return;
        }
        Animator.SetBool("IsSmelting", IsSmelting);
        if (SmelterUI.FuelSlot.Item != null)
        {
            AddFuelTime(SmelterUI.FuelSlot);
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
        if (SmelterUI.OreSlot.Item != null)
        {
            var smeltableItem = SmelterUI.OreSlot.Item as SmeltableItemSO;
            if (smeltableItem != null && smeltableItem.SmeltTime <= CurrentBurnTime && !IsSmelting)
            {
                IsSmelting = true;
                SmeltTimer.SetMaxTime(smeltableItem.SmeltTime);
            }
            if (SmeltTimer.RemainingTime <= 0 && IsSmelting)
            {
                IsSmelting = false;
                SpawnItem(smeltableItem.SmeltedItem.Id, 1);
                SmelterUI.OreSlot.RemoveItem(smeltableItem, 1);
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

    public override void InteractWithBuilding()
    {
        IsUIActive = !IsUIActive;
        if (ActiveUI == null)
        {
            ActiveUI = Instantiate(UIPrefab, PlayerInteractManager.Instance.BuildingUIPanel.transform);
            SmelterUI = ActiveUI.GetComponent<SmelterUIManager>();

            ActiveUI.SetActive(false);
        }
        if (PlayerInteractManager.Instance.CurrentActiveBuildingUI.Count > 0)
        {
            if (!PlayerInteractManager.Instance.CurrentActiveBuildingUI.Contains(ActiveUI))
            {
                foreach (var UI in PlayerInteractManager.Instance.CurrentActiveBuildingUI)
                {
                    UI.SetActive(false);
                }
                IsUIActive = true;
                PlayerInteractManager.Instance.CurrentActiveBuildingUI.Add(ActiveUI);
            }
        }
        else
        {
            PlayerInteractManager.Instance.CurrentActiveBuildingUI.Add(ActiveUI);
            IsUIActive = true;
        }
        ActiveUI.SetActive(IsUIActive);
    }
}