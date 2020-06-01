using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBuilder : MonoBehaviour
{
    // Data
    public int BuildingCode;
    private BuildingStatus currentBuildingStatus;
    private BuildingData currentData;
    private BuildingController currentBuilding;

    private void Start()
    {
        ConstructBuilding(BuildingCode);
    }
    public void CreateNewBuilding(int buildingCode)
    {
        PlayerBuilding.Instance.RemoveBuildingEffects();
        PlayerBuilding.Instance.ApplyBuildingEffects();
        PlayerQuest.Instance.UpdateBuildingQuest(buildingCode);
        ConstructBuilding(buildingCode);
        PayCost();
    }
    private void ConstructBuilding(int buildingCode)
    {
        if (currentBuilding != null)
        {
            Destroy(currentBuilding.gameObject);
        }

        currentBuildingStatus = PlayerBuilding.Instance.GetBuildingStatus(buildingCode);
        currentData = BuildingDB.Instance.GetBuildingData(buildingCode);

        GameObject prefab = AssetBundleCacher.Instance.LoadAndGetAsset("object", $"Building_Grade_{currentBuildingStatus.Grade}") as GameObject;
        currentBuilding = Instantiate(prefab, transform).GetComponent<BuildingController>();

        BuildingInfo currentGradeInfo = currentData.StatsByGrade[currentBuildingStatus.Grade];
        currentBuilding.transform.localPosition = currentGradeInfo.BuildingPosition;
        currentBuilding.transform.localRotation = Quaternion.Euler(currentGradeInfo.BuildingRotation);
        currentBuilding.Initialize(currentData);
    }
    private void PayCost()
    {
        int grade = currentBuildingStatus.Grade;
        PlayerStat.Instance.RemoveGold(currentData.StatsByGrade[grade].RequiredGold);

        BuildingCost[] costs = currentData.StatsByGrade[grade].ConstructionCost;
        for (int i = 0; i < costs.Length; ++i)
        {
            PlayerInventory.Instance.RemoveItemFromInventory(costs[i].NeedItem, costs[i].NeedItemCount);
        }
    }
}
