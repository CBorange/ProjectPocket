using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestReward_GetStatus
{
    public int QuestCode;
    public RewardStatus[] RewardStatuss;
}

[System.Serializable]
public class RewardStatus
{
    public string StatusType;
    public string StatusName;
    public int Amount;
}
