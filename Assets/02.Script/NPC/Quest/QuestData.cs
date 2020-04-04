using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestData
{
    public string QuestName;
    public int[] PrecedentQuests;
    public int StartNPC;
    public int EndNPC;
    public int QuestCode;
    public string[] QuestCategorys;
    public string QuestIntroduce_Implied;
    public string[] QuestIntroduce;
    public string[] QuestCompleteContents;
    public string[] QuestRewards;

    // Behaviour
    public QuestBehaviour_Discussion Behaviour_Discussion;
    public QuestBehaviour_Building Behaviour_Building;
    public QuestBehaviour_KillMonster Behaviour_KillMonster;
    public QuestBehaviour_GetItem Behaviour_GetItem;

    // Reward
    public QuestReward_GetItem Reward_GetItem;

    public QuestData() { }
    public QuestData(QuestBehaviour_Discussion discussion, QuestBehaviour_Building building, QuestBehaviour_KillMonster killMonster, QuestBehaviour_GetItem getItem)
    {
        this.Behaviour_Discussion = discussion;
        this.Behaviour_Building = building;
        this.Behaviour_KillMonster = killMonster;
        this.Behaviour_GetItem = getItem;
    }
    
}
