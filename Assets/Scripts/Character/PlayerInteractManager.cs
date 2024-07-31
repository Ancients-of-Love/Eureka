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
    public List<ResourceNode> InRangeOfResourceNodes = new();
    public ResourceNode ClosestResourceNode;
    public float MiningSpeed = 0.8f;
    public float MiningDistance = 1f;
    private bool IsFPressed = false;
    private Timer MiningTimer;
    public GameObject Player;

    [SerializeField]
    private GameObject InventoryUIPanel;

    [SerializeField]
    public GameObject BuildingUIPanel;

    [SerializeField]
    public GameObject PlayerLeftHand;

    [SerializeField]
    private GameObject PromptUI;

    public ElectricBuilding SelectedAttachingBuilding;
    public List<GameObject> CurrentActiveBuildingUI = new();

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

    private List<GameObject> Prompts = new();

    private bool IsInventoryOpen = false;

    private void Start()
    {
        ClosestBuilding = null;
        Player = GameObject.Find("Player");
        MiningTimer = new Timer();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            InventoryUIPanel.SetActive(IsInventoryOpen = !IsInventoryOpen);
        }

        HandleBuildingInteractions();
        HandleResourceNodesInteractions();
    }

    #region Building Interactions

    private bool FindClosestBuilding()
    {
        if (InRangeOfBuildings.Count == 0)
        {
            ClosestBuilding = null;
            return false;
        }
        if (InRangeOfBuildings.Count >= 2)
        {
            float closestBuildingDistance = Mathf.Infinity;
            foreach (IBuilding building in InRangeOfBuildings)
            {
                var distance = Vector3.Distance(building.GetBuildingPosition(), Player.transform.position);

                Debug.Log(distance);
                if (distance < closestBuildingDistance)
                {
                    closestBuildingDistance = distance;
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
            if (CurrentActiveBuildingUI.Count > 0)
            {
                foreach (GameObject UI in CurrentActiveBuildingUI)
                {
                    UI.SetActive(false);
                }
            }
            CurrentActiveBuildingUI.Clear();
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

    #endregion Building Interactions

    #region Node Interactions

    private bool FindClosestNode()
    {
        if (InRangeOfResourceNodes.Count == 0)
        {
            ClosestResourceNode = null;
            return false;
        }
        if (InRangeOfResourceNodes.Count >= 2)
        {
            float closestDistance = Mathf.Infinity;
            foreach (ResourceNode resourceNode in InRangeOfResourceNodes)
            {
                var distance = Vector3.Distance(resourceNode.GetPosition(), Player.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    ClosestResourceNode = resourceNode;
                }
            }
        }
        else
        {
            ClosestResourceNode = InRangeOfResourceNodes[0];
        }
        return true;
    }

    public void AddNearResource(ResourceNode resourceNode)
    {
        InRangeOfResourceNodes.Add(resourceNode);
    }

    public void RemoveNearResource(ResourceNode resourceNode)
    {
        InRangeOfResourceNodes.Remove(resourceNode);
    }

    private void HandleResourceNodesInteractions()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            IsFPressed = true;
            MiningTimer.SetMaxTime(MiningSpeed);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            PlayerMovement.Instance.CanMove = true;
            IsFPressed = false;
            Player.GetComponentInChildren<Animator>().SetBool("IsMining", false);
            PlayerLeftHand.SetActive(false);
        }

        if (!FindClosestNode())
        {
            foreach (GameObject prompt in Prompts)
            {
                Destroy(prompt);
                FUseObject = null;
                Prompts = new();
            }
            PlayerMovement.Instance.CanMove = true;

            return;
        }

        if (EUseObject == null)
        {
            Prompts.Add(FUseObject = Instantiate(FUsePrefab, PromptUI.transform));
        }

        if (IsFPressed)
        {
            PlayerMovement.Instance.CanMove = false;

            var distance = Vector3.Distance(ClosestResourceNode.GetPosition(), Player.transform.position);
            var direction = (ClosestResourceNode.GetPosition() - Player.transform.position).normalized;

            if (distance > MiningDistance)
            {
                PlayerMovement.Instance.controller.Move(PlayerMovement.Instance.Speed * Time.deltaTime * direction);
                PlayerMovement.Instance.HandleFlip(direction.x);
            }
            else
            {
                Player.GetComponentInChildren<Animator>().SetBool("IsMining", IsFPressed);
                PlayerLeftHand.SetActive(true);
                MiningTimer.Tick(Time.deltaTime);
                if (MiningTimer.RemainingTime <= 0)
                {
                    ClosestResourceNode.TakeDamage(1);
                    MiningTimer.ResetTimer();
                }
            }
        }
    }

    #endregion Node Interactions
}