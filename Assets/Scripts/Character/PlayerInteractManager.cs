using NaughtyAttributes;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractManager : Singleton<PlayerInteractManager>
{
    private List<IBuilding> InRangeOfBuildings = new();
    public IBuilding ClosestBuilding;
    private float ClosestBuildingDistance = Mathf.Infinity;
    public GameObject Player;

    [SerializeField]
    private GameObject InventoryUIPanel;

    [SerializeField]
    public GameObject BuildingUIPanel;

    [SerializeField]
    private GameObject PromptUI;

    public ElectricBuilding SelectedAttachingBuilding;
    public ElectricalBuildingUIManager CurrentActiveBuildingUI;

    [BoxGroup("PromptPrefabs")]
    [SerializeField]
    private GameObject EUsePrefab;

    private GameObject EUseObject = null;

    [BoxGroup("PromptPrefabs")]
    [SerializeField]
    private GameObject FUsePrefab;
    private GameObject FUseObject = null;

    [BoxGroup("PromptPrefabs")]
    [SerializeField]
    private GameObject LMBUsePrefab;
    private GameObject LMBUseObject = null;

    [BoxGroup("PromptPrefabs")]
    [SerializeField]
    private GameObject DELUsePrefab;
    private GameObject DELUseObject = null;

    private List<GameObject> Prompts = new List<GameObject>();

    private bool IsInventoryOpen = false;

    private void Start()
    {
        ClosestBuilding = null;
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryUIPanel.SetActive(IsInventoryOpen = !IsInventoryOpen);
        }

        HandleBuildingInteractions();
    }

    private bool FindClosestBuilding()
    {
        if (InRangeOfBuildings.Count == 0)
        {
            ClosestBuilding = null;
            ClosestBuildingDistance = Mathf.Infinity;
            return false;
        }
        if (InRangeOfBuildings.Count >= 2)
        {
            foreach (IBuilding building in InRangeOfBuildings)
            {
                var distance = Vector3.Distance(building.GetBuildingPosition(), Player.transform.position);

                Debug.Log(distance);
                if (distance < ClosestBuildingDistance)
                {
                    ClosestBuildingDistance = distance;
                    ClosestBuilding = building;
                }
            }
        }
        else
        {
            ClosestBuilding = InRangeOfBuildings[0];
        }
        return true;
    }

    public void AddNearBuilding(IBuilding building)
    {
        InRangeOfBuildings.Add(building);
    }

    public void RemoveNearBuilding(IBuilding building)
    {
        InRangeOfBuildings.Remove(building);
    }

    private void HandleBuildingInteractions()
    {
        if (!FindClosestBuilding())
        {
            foreach (GameObject prompt in Prompts)
            {
                Destroy(prompt);
                EUseObject = null;
                FUseObject = null;
                LMBUseObject = null;
                DELUseObject = null;
                Prompts = new();
            }
            if (CurrentActiveBuildingUI != null)
            {
                CurrentActiveBuildingUI.gameObject.SetActive(false);
                CurrentActiveBuildingUI = null;
            }
            CurrentActiveBuildingUI = null;
            return;
        }
        if (!ClosestBuilding.IsActive)
        {
            return;
        }

        if (EUseObject == null)
        {
            Prompts.Add(EUseObject = Instantiate(EUsePrefab, PromptUI.transform));
        }

        if (DELUseObject == null)
        {
            Prompts.Add(DELUseObject = Instantiate(DELUsePrefab, PromptUI.transform));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ClosestBuilding.InteractWithBuilding();
        }
        if (Input.GetKeyDown(KeyCode.Delete))
        {
            InRangeOfBuildings.Remove(ClosestBuilding);
            ClosestBuilding.DestroyBuilding();
            ClosestBuilding = null;
        }
    }
}