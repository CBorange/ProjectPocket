using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureBuilder : MonoBehaviour
{
    // Data
    public int BuildingCode;
    private BuildingStatus currentBuildingStatus;
    private BuildingData currentData;
    private GameObject currentBuildingObj;

    private void Start()
    {
        CreateBuilding(BuildingCode);
    }
    public void CreateBuilding(int buildingCode)
    {
        if (currentBuildingObj != null)
            Destroy(currentBuildingObj);

        currentBuildingStatus = PlayerBuilding.Instance.GetBuildingStatus(buildingCode);
        currentData = BuildingDB.Instance.GetBuildingData(buildingCode);

        GameObject prefab = Resources.Load<GameObject>($"Object/Structure/Building/Building_Grade_{currentBuildingStatus.Grade}");
        currentBuildingObj = Instantiate(prefab, transform);

        BuildingInfo currentGradeInfo = currentData.StatsByGrade[currentBuildingStatus.Grade];
        currentBuildingObj.transform.localPosition = currentGradeInfo.BuildingPosition;
        currentBuildingObj.transform.localRotation = Quaternion.Euler(currentGradeInfo.BuildingRotation);
        currentBuildingObj.GetComponent<BuildingController>().Initialize(currentData);
    }
}
