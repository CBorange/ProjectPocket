using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestBehaviour_Building
{
    public int QuestCode;
    public TargetBuildingData[] TargetBuilding;
}

[System.Serializable]
public class TargetBuildingData
{
    public int BuildingCode;
    public string BuildingName;
    public int BuildingGrade;
}