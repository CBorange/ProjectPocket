using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestBehaviour_GetItem : QuestUpdater
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
    public TargetItemData[] TargetItem;

    public void Initialize()
    {
        questObservers = new List<QuestObserver>();
    }
    public bool GetHasCompletedAllItemGet()
    {
        int getCompleteCount = 0;
        for (int i = 0; i < TargetItem.Length; ++i)
        {
            InventoryItem targetInInventory = PlayerInventory.Instance.GetItem(TargetItem[i].ItemCode);
            if (targetInInventory != null)
            {
                if (targetInInventory.ItemCount >= TargetItem[i].ItemCount)
                    getCompleteCount += 1;
            }                
        }
        if (getCompleteCount == TargetItem.Length)
            return true;
        else
            return false;
    }
    public void NoticeQuestProgress(int itemCode)
    {
        for (int itemIdx = 0; itemIdx < TargetItem.Length; ++itemIdx)
        {
            InventoryItem needItem = PlayerInventory.Instance.GetItem(itemCode);
            if (needItem != null)
            {
                if (needItem.ItemCount <= TargetItem[itemIdx].ItemCount &&
                    needItem.OriginalItemData.ItemCode == TargetItem[itemIdx].ItemCode)   
                {
                    for (int i = 0; i < questObservers.Count; ++i)
                        questObservers[i].Update_GetItem(QuestCode, itemCode, needItem.ItemCount, TargetItem[itemIdx].ItemCount);
                }
            }
        }
    }
}


[System.Serializable]
public class TargetItemData
{
    public int ItemCode;
    public string ItemName;
    public int ItemCount;
}

