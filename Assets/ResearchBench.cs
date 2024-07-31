using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchBench : Building
{
    private bool IsUIActive = true;

    public override void InteractWithBuilding()
    {
        IsUIActive = !IsUIActive;
        if (PlayerInteractManager.Instance.CurrentActiveBuildingUI.Count > 0)
        {
            if (!PlayerInteractManager.Instance.CurrentActiveBuildingUI.Contains(BlueprintManager.Instance.ResearchUI))
            {
                foreach (var UI in PlayerInteractManager.Instance.CurrentActiveBuildingUI)
                {
                    UI.SetActive(false);
                }
                IsUIActive = true;
                PlayerInteractManager.Instance.CurrentActiveBuildingUI.Add(BlueprintManager.Instance.ResearchUI);
            }
        }
        else
        {
            PlayerInteractManager.Instance.CurrentActiveBuildingUI.Add(BlueprintManager.Instance.ResearchUI);
            IsUIActive = true;
        }
        BlueprintManager.Instance.ResearchUI.SetActive(IsUIActive);
    }
}