using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Generator : Building
{
    public bool IsOn = false;
    public float FuelCapacity = 1000f;
    public float FuelLevel = 1000f;
    public float FuelConsumptionRate = 1f;
    public int GeneratorLevel = 1;
    public int MaxCapacity = 100;
    public int CurrentCapacity = 0;

    public Slider Slider;
    public GameObject GeneratorUI;
    private GameObject ActiveUI = null;
    private bool IsUIActive = false;
    private ElectricalBuildingUIManager ElectricalUI = null;

    [SerializeField]
    private GameObject GeneratorManagerUI;

    private List<Tile> CurrentTiles = new();

    public override void InteractWithBuilding()
    {
        IsUIActive = !IsUIActive;
        if (ActiveUI == null)
        {
            ActiveUI = Instantiate(GeneratorUI, PlayerInteractManager.Instance.BuildingUIPanel.transform);
            ElectricalUI = ActiveUI.GetComponent<ElectricalBuildingUIManager>();
            ElectricalUI.TogglePower.ToggleOn += TogglePowerOn;
            ElectricalUI.TogglePower.ToggleOff += TogglePowerOff;
            ElectricalUI.ToggleConnect.onClick.AddListener(HandleAttach);

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
                PlayerInteractManager.Instance.CurrentActiveBuildingUI.Add(GeneratorManagerUI);
            }
        }
        else
        {
            PlayerInteractManager.Instance.CurrentActiveBuildingUI.Add(ActiveUI);
            PlayerInteractManager.Instance.CurrentActiveBuildingUI.Add(GeneratorManagerUI);
            IsUIActive = true;
        }

        GeneratorManagerUI.SetActive(IsUIActive);
        ActiveUI.SetActive(IsUIActive);
    }

    private void TogglePowerOff(ToggleSwitchScript script)
    {
        IsOn = true;
    }

    private void TogglePowerOn(ToggleSwitchScript script)
    {
        IsOn = false;
    }

    private void HandleAttach()
    {
        if (TargetFollowerArrow.Target == null)
        {
            TargetFollowerArrow.Target = transform;
        }
        else if (TargetFollowerArrow.Target == transform)
        {
            TargetFollowerArrow.Target = null;
        }

        if (TargetFollowerArrow.Target != null && TargetFollowerArrow.Target != transform)
        {
            var other = TargetFollowerArrow.Target.GetComponent<ElectricBuilding>();
            other.Attach();
            TargetFollowerArrow.Target = null;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (CurrentTiles.Count == 0)
        {
            CurrentTiles = GridManager.Instance.GetTileOrNeighboursBySize(transform.position, BuildingSO.TileOccupationSize);
            foreach (Tile tile in CurrentTiles)
            {
                tile.IsOccupied = true;
            }
        }

        if (IsOn)
        {
            FuelLevel -= FuelConsumptionRate * Time.deltaTime;
            if (FuelLevel <= 0)
            {
                IsOn = false;
                FuelLevel = 0;
            }
            //GeneratorManagerUI.
        }
        if (CurrentCapacity > MaxCapacity)
        {
            IsOn = false;
        }

        Slider.value = FuelLevel / FuelCapacity * 100;
    }
}