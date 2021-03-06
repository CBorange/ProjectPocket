﻿using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class NPC_QuestPanel_Objective : MonoBehaviour
{
    // UI
    public Text ContentsText;
    public Text ImpliedObjectiveText;
    public Text DetailedObjectiveText;
    public Text RewardText;

    // Buttons
    public Button Accept_Btn;
    public Button Complete_Btn;

    // Panel
    public AlertPopup AlertPopup;
    public NPC_QuestPanel QuestPanel;

    // Data
    private QuestData currnetData;
    private int npcCode;

    public void Initialize()
    {

    }

    public void OpenPanel(int npcCode)
    {
        this.npcCode = npcCode;
        RefreshPanel();
    }
    public void RefreshPanel()
    {
        DeactiveAllButton();
        ContentsText.text = string.Empty;
        ImpliedObjectiveText.text = string.Empty;
        DetailedObjectiveText.text = string.Empty;
        RewardText.text = string.Empty;
    }
    public void RefreshToAcceptable(QuestData data)
    {
        DeactiveAllButton();
        currnetData = data;
        Accept_Btn.gameObject.SetActive(true);

        // 퀘스트 설명
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < currnetData.QuestIntroduce.Length; ++i)
        {
            builder.Append(currnetData.QuestIntroduce[i]);
            builder.AppendLine();
        }
        ContentsText.text = builder.ToString();

        // 퀘스트 목표
        ImpliedObjectiveText.text = currnetData.QuestIntroduce_Implied;
        DetailedObjectiveText.text = GetQuestObjectSTR(currnetData);

        // 퀘스트 보상
        RewardText.text = GetQuestRewardSTR(currnetData);
    }
    public void RefreshToComplete(QuestData data)
    {
        currnetData = data;
        DeactiveAllButton();
        Complete_Btn.gameObject.SetActive(true);

        // 퀘스트 설명
        StringBuilder builder = new StringBuilder();
        for (int i = 0; i < currnetData.QuestCompleteContents.Length; ++i)
        {
            builder.Append(currnetData.QuestCompleteContents[i]);
            builder.AppendLine();
        }
        ContentsText.text = builder.ToString();

        // 퀘스트 목표
        ImpliedObjectiveText.text = currnetData.QuestIntroduce_Implied;
        DetailedObjectiveText.text = GetQuestObjectSTR(currnetData);

        // 퀘스트 보상
        RewardText.text = GetQuestRewardSTR(currnetData);
    }
    private string GetQuestObjectSTR(QuestData data)
    {
        StringBuilder builder = new StringBuilder();
        for (int categoryIdx = 0; categoryIdx < data.QuestCategorys.Length; ++categoryIdx)
        {
            switch (currnetData.QuestCategorys[categoryIdx])
            {
                case "Discussion":
                    builder.AppendLine("대화 : ");
                    TargetNPCData[] targetNPC = currnetData.Behaviour_Discussion.TargetNPC;
                    for (int i = 0; i < targetNPC.Length; ++i)
                    {
                        builder.AppendLine($"[{targetNPC[i].NPCName}]");
                    }
                    break;
                case "KillMonster":
                    builder.AppendLine("몬스터 사냥 : ");
                    TargetMonsterData[] targetMonster = currnetData.Behaviour_KillMonster.TargetMonster;
                    for (int i = 0; i < targetMonster.Length; ++i)
                    {
                        builder.AppendLine($"[{targetMonster[i].MonsterName} / {targetMonster[i].KillCount}마리]");
                    }
                    break;
                case "Building":
                    builder.AppendLine("건물 증축 : ");
                    TargetBuildingData[] targetBulding = currnetData.Behaviour_Building.TargetBuilding;
                    for (int i = 0; i < targetBulding.Length; ++i)
                    {
                        builder.AppendLine($"[{targetBulding[i].BuildingName} / {targetBulding[i].BuildingGrade + 1}단계]");
                    }
                    break;
                case "GetItem":
                    builder.AppendLine("아이템 획득 : ");
                    TargetItemData[] targetItem = currnetData.Behaviour_GetItem.TargetItem;
                    for (int i = 0; i < targetItem.Length; ++i)
                    {
                        builder.AppendLine($"[{targetItem[i].ItemName} / {targetItem[i].ItemCount}개]");
                    }
                    break;
            }
        }
        return builder.ToString();
    }
    private string GetQuestRewardSTR(QuestData data)
    {
        StringBuilder builder = new StringBuilder();
        for (int rewardIdx = 0; rewardIdx < data.QuestRewards.Length; ++rewardIdx)
        {
            switch(data.QuestRewards[rewardIdx])
            {
                case "GetItem":
                    RewardItem[] items = data.Reward_GetItem.RewardItems;
                    for (int i = 0; i < items.Length; ++i)
                    {
                        ItemData item = ItemDB.Instance.GetItemData(items[i].ItemCode);
                        builder.Append($"[{item.Name} / {items[i].ItemCount}개]");
                        if (i != items.Length - 1)
                            builder.Append(", ");
                    }
                    break;
                case "GetStatus":
                    RewardStatus[] statuses = data.Reward_GetStatus.RewardStatuss;
                    for (int i = 0; i < statuses.Length; ++i)
                    {
                        builder.Append($"[{statuses[i].StatusName} / {statuses[i].Amount}]");
                        if (i != statuses.Length - 1)
                            builder.Append(", ");
                    }
                    break;
            }
        }
        return builder.ToString();
    }
    private void DeactiveAllButton()
    {
        Accept_Btn.gameObject.SetActive(false);
        Complete_Btn.gameObject.SetActive(false);
    }

    // Button Method
    public void AcceptQuest()
    {
        PlayerQuest.Instance.StartQuest(npcCode, currnetData.QuestCode);
        QuestPanel.RefreshPanel();
    }
    public void CompleteQuest()
    {
        PlayerQuest.Instance.CompleteQuest(npcCode, currnetData);
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"[<color=orange>{currnetData.QuestName}</color>]");
        builder.AppendLine("퀘스트를 완료하였습니다!");
        AlertPopup.RefreshToAlert(builder.ToString());
        AlertPopup.OpenPopup(1.5f);

        QuestPanel.RefreshPanel();
    }
}
