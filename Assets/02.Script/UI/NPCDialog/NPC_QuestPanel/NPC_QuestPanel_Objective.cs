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

    // Data
    private QuestData currnetData;

    public void Initialize()
    {

    }

    public void OpenPanel()
    {
        DeactiveAllButton();
        ImpliedObjectiveText.text = "퀘스트 목표";
        DetailedObjectiveText.text = "내용";
        RewardText.text = "내용";
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
                    TargetNPCData[] targetNPC = currnetData.Behaviour_Discussion.TargetNPC;
                    for (int i = 0; i < targetNPC.Length; ++i)
                    {
                        if (i == targetNPC.Length)
                            builder.Append($"[{targetNPC[i].NPCName}]");
                        else
                            builder.Append($"[{targetNPC[i].NPCName}], ");
                    }
                    builder.Append(" 와 대화하세요.");
                    break;
                case "KillMonster":
                    builder.Append("몬스터 사냥 : ");
                    TargetMonsterData[] targetMonster = currnetData.Behaviour_KillMonster.TargetMonster;
                    for (int i = 0; i < targetMonster.Length; ++i)
                    {
                        if (i == targetMonster.Length)
                            builder.Append($"[{targetMonster[i].MonsterName} / {targetMonster[i].KillCount}마리]");
                        else
                            builder.Append($"[{targetMonster[i].MonsterName} / {targetMonster[i].KillCount}마리], ");
                    }
                    builder.Append(" 사냥하세요.");
                    break;
                case "Building":
                    builder.Append("건물 증축 : ");
                    TargetBuildingData[] targetBulding = currnetData.Behaviour_Building.TargetBuilding;
                    for (int i = 0; i < targetBulding.Length; ++i)
                    {
                        if (i == targetBulding.Length)
                            builder.Append($"[{targetBulding[i].BuildingName} / {targetBulding[i].BuildingGrade}단계]");
                        else
                            builder.Append($"[{targetBulding[i].BuildingName} / {targetBulding[i].BuildingGrade}단계], ");
                    }
                    builder.Append(" 까지 업그레이드 하세요.");
                    break;
                case "ItemGet":
                    builder.Append("아이템 획득 : ");
                    TargetItemData[] targetItem = currnetData.Behaviour_GetItem.TargetItem;
                    for (int i = 0; i < targetItem.Length; ++i)
                    {
                        if (i == targetItem.Length)
                            builder.Append($"[{targetItem[i].ItemName} / {targetItem[i].ItemCount}개]");
                        else
                            builder.Append($"[{targetItem[i].ItemName} / {targetItem[i].ItemCount}개], ");
                    }
                    builder.Append(" 아이템을 획득하세요.");
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
                        if (i == items.Length)
                            builder.Append($"[{item.Name} / {items[i].ItemCount}개]");
                        else
                            builder.Append($"[{item.Name} / {items[i].ItemCount}개], ");
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
        PlayerQuest.Instance.StartQuest(currnetData.QuestCode);
    }
    public void CompleteQuest()
    {
        PlayerQuest.Instance.CompleteQuest(currnetData.QuestCode);
    }
}
