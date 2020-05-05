using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestBehaviour_Building
{
    public int QuestCode;
    public TargetBuildingData[] TargetBuilding;
    public bool GetHasCompletedAllBuildingConstruct()
    {
        int constructComleteCount = 0;
        for (int i = 0; i < TargetBuilding.Length; ++i)
        {
            BuildingStatus currentBuilding = PlayerBuilding.Instance.GetBuildingStatus(TargetBuilding[i].BuildingCode);
            if (currentBuilding.Grade >= TargetBuilding[i].BuildingGrade)
                constructComleteCount += 1;
        }
        if (constructComleteCount == TargetBuilding.Length)
            return true;
        else
            return false;
    }
}

[System.Serializable]
public class TargetBuildingData
{
    public int BuildingCode;
    public string BuildingName;
    public int BuildingGrade;
}