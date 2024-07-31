using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class ElectricBuilding : Building
{
    public bool IsAttached;
    public bool IsPowered;
    public int UsedCapacity;

    [SerializeField]
    private GameObject UIPrefab;
    private bool IsUIActive = true;

    private Light2D Light2D;

    private bool IsSwitchedOff = false;
    private GameObject ActiveUI = null;

    private ElectricalBuildingUIManager ElectricalUI = null;

    private new void Start()
    {
        base.Start();

        Light2D = GetComponentInChildren<Light2D>();
    }

    private void Update()
    {
        if (IsSwitchedOff)
        { return; }
        if (IsAttached)
        {
            var IsGeneratorOn = GeneratorManager.Instance.Generator.IsOn;
            IsPowered = IsGeneratorOn;
            Light2D.gameObject.SetActive(IsGeneratorOn);
        }
        else
        {
            IsPowered = false;
            Light2D.gameObject.SetActive(false);
        }
    }

    public override void InteractWithBuilding()
    {
        IsUIActive = !IsUIActive;
        if (ActiveUI == null)
        {
            ActiveUI = Instantiate(UIPrefab, PlayerInteractManager.Instance.BuildingUIPanel.transform);
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
            }
        }
        else
        {
            PlayerInteractManager.Instance.CurrentActiveBuildingUI.Add(ActiveUI);
            IsUIActive = true;
        }
        ActiveUI.SetActive(IsUIActive);
    }

    private void TogglePowerOff(ToggleSwitchScript script)
    {
        IsSwitchedOff = true;
        Light2D.gameObject.SetActive(false);
    }

    private void TogglePowerOn(ToggleSwitchScript script)
    {
        IsSwitchedOff = false;
        Light2D.gameObject.SetActive(true);
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
            Attach();
            if (!TargetFollowerArrow.Target.TryGetComponent<Generator>(out var generator))
            {
                TargetFollowerArrow.Target.GetComponent<ElectricBuilding>().Attach();
            }
            TargetFollowerArrow.Target = null;
        }
    }

    public virtual void Attach()
    {
        if (!IsAttached)
        {
            if (GeneratorManager.Instance.Attach(this))
            {
                IsAttached = true;
                return;
            }
            else
            {
                Debug.Log("Not enough capacity to attach " + Name);
            }
        }
        else
        {
            Debug.Log(Name + " is already attached to the generator");
        }
    }

    public virtual void Detach()
    {
        if (IsAttached)
        {
            GeneratorManager.Instance.Detach(this);
            IsAttached = false;
        }
        else
        {
            Debug.Log(Name + " is not attached to the generator");
        }
    }

    private void OnDestroy()
    {
        ElectricalUI.TogglePower.ToggleOn -= TogglePowerOn;
        ElectricalUI.TogglePower.ToggleOff -= TogglePowerOff;
        Destroy(ActiveUI);
    }
}