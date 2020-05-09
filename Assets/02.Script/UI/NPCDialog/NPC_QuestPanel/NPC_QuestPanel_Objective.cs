using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class NPC_QuestPanel_Objective : MonoBehaviour
{
    // Quest Objective UI
    public Text ContentsText;
    public Text ImpliedObjectiveText;
    public Text DetailedObjectiveText;
    public Text RewardText;

    // Buttons
    public Button Accept_Btn;
    public Button Complete_Btn;

    // Panel
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
                    builder.Append("대화 : ");
                    builder.AppendLine();
                    TargetNPCData[] targetNPC = currnetData.Behaviour_Discussion.TargetNPC;
                    for (int i = 0; i < targetNPC.Length; ++i)
                    {
                        builder.Append($"[{targetNPC[i].NPCName}]");
                        if (i < targetNPC.Length - 1)
                            builder.AppendLine();
                    }
                    break;
                case "KillMonster":
                    builder.Append("몬스터 사냥 : ");
                    builder.AppendLine();
                    TargetMonsterData[] targetMonster = currnetData.Behaviour_KillMonster.TargetMonster;
                    for (int i = 0; i < targetMonster.Length; ++i)
                    {
                        builder.Append($"[{targetMonster[i].MonsterName} / {targetMonster[i].KillCount}마리]");
                        if (i < targetMonster.Length - 1)
                            builder.AppendLine();
                    }
                    break;
                case "Building":
                    builder.Append("건물 증축 : ");
                    builder.AppendLine();
                    TargetBuildingData[] targetBulding = currnetData.Behaviour_Building.TargetBuilding;
                    for (int i = 0; i < targetBulding.Length; ++i)
                    {
                        builder.Append($"[{targetBulding[i].BuildingName} / {targetBulding[i].BuildingGrade + 1}단계]");
                        if (i < targetBulding.Length - 1)
                            builder.AppendLine();
                    }
                    break;
                case "GetItem":
                    builder.Append("아이템 획득 : ");
                    builder.AppendLine();
                    TargetItemData[] targetItem = currnetData.Behaviour_GetItem.TargetItem;
                    for (int i = 0; i < targetItem.Length; ++i)
                    {
                        builder.Append($"[{targetItem[i].ItemName} / {targetItem[i].ItemCount}개]");
                        if (i < targetItem.Length - 1)
                            builder.AppendLine();
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
        QuestPanel.RefreshPanel();
    }
}
