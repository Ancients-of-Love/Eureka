using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInteractManager : MonoBehaviour
{
    public static PlayerInteractManager Instance;

    private List<IBuilding> InRangeOfBuildings = new();
    private IBuilding ClosestBuilding;
    private float ClosestBuildingDistance = Mathf.Infinity;
    private GameObject Player;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogWarning("Multiple PlayerInteractManagers found in the scene. Destroying the new one.");
            Destroy(gameObject);
        }
        ClosestBuilding = null;
        Player = GameObject.Find("Player");
    }

    // Update is called once per frame
    private void Update()
    {
        if (!FindClosestBuilding())
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            ClosestBuilding.InteractWithBuilding();
        }
    }

    private bool FindClosestBuilding()
    {
        if (InRangeOfBuildings.Count == 0)
        {
            ClosestBuilding = null;
            return false;
        }
        if (InRangeOfBuildings.Count >= 2)
        {
            foreach (IBuilding building in InRangeOfBuildings)
            {
                var distance = Vector3.Distance(building.GetBuildingPosition(), Player.transform.position);
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
}