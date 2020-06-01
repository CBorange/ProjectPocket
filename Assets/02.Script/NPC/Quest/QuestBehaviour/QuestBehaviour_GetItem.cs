using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestBehaviour_GetItem
{
    public int QuestCode;
    public TargetItemData[] TargetItem;
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
        for (int i = 0; i < TargetItem.Length; ++i)
        {
            InventoryItem needItem = PlayerInventory.Instance.GetItem(itemCode);
            if (needItem != null)
            {
                if (needItem.ItemCount <= TargetItem[i].ItemCount &&
                    needItem.OriginalItemData.ItemCode == TargetItem[i].ItemCode)   
                {
                    QuestNoticePopup.Instance.PrintNotice_GetItem(QuestCode, itemCode, needItem.ItemCount, TargetItem[i].ItemCount);
                    return;
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

