using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingInfo
{
    public string Introduce;
    public StatAdditional[] BuildingStats;
    public int RequiredGold;
    public BuildingCost[] ConstructionCost;
    public Vector3 BuildingPosition;
    public Vector3 BuildingRotation;
}
[System.Serializable]
public class BuildingCost
{
    public int NeedItem;
    public int NeedItemCount;
}
[System.Serializable]
public class BuildingData
{
    public int BuildingCode;
    public string BuildingName;
    public BuildingInfo[] StatsByGrade;
}
