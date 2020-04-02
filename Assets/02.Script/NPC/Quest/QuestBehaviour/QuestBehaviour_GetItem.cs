using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestBehaviour_GetItem
{
    public int QuestCode;
    public TargetItemData[] TargetItem;
}


[System.Serializable]
public class TargetItemData
{
    public int ItemCode;
    public string ItemName;
    public int ItemCount;
}

