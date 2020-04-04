using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class QuestReward_GetItem
{
    public int QuestCode;
    public RewardItem[] RewardItems;
}

[System.Serializable]
public class RewardItem
{
    public string ItemType;
    public int ItemCode;
    public int ItemCount;
}
