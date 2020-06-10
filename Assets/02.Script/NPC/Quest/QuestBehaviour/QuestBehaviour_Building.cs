using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestBehaviour_Building : QuestUpdater
{
    #region Observer
    private List<QuestObserver> questObservers;
    public void AddObserver(QuestObserver observer)
    {
        questObservers.Add(observer);
    }
    public void DeleteObserver(QuestObserver observer)
    {
        questObservers.Remove(observer);
    }
    #endregion

    public int QuestCode;
    public TargetBuildingData[] TargetBuilding;

    public void Initialize()
    {
        questObservers = new List<QuestObserver>();
    }
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
    public void NoticeQuestProgress(int buildingCode)
    {
        for (int buildIdx = 0; buildIdx < TargetBuilding.Length; ++buildIdx)
        {
            if (TargetBuilding[buildIdx].BuildingCode == buildingCode)
            {
                BuildingStatus currentBuilding = PlayerBuilding.Instance.GetBuildingStatus(TargetBuilding[buildIdx].BuildingCode);
                if (currentBuilding.Grade <= TargetBuilding[buildIdx].BuildingGrade)
                {
                    for (int i = 0; i < questObservers.Count; ++i)
                        questObservers[i].Update_Building(QuestCode, buildingCode, currentBuilding.Grade, TargetBuilding[buildIdx].BuildingGrade);
                }
            }
        }
    }
}

[System.Serializable]
public class TargetBuildingData
{
    public int BuildingCode;
    public string BuildingName;
    public int BuildingGrade;
}