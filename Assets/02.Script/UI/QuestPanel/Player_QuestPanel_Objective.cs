using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Player_QuestPanel_Objective : MonoBehaviour
{
    // UI
    public Text ContentsText;
    public Text ObjectiveTitleText;
    public Text ObjectiveContentsText;
    public Text RewardContentsText;

    public void Initialize()
    {
    }
    public void OpenPanel()
    {

    }
    public void ClosePanel()
    {

    }
    public void RefreshToInProgress(int questCode)
    {
        StringBuilder builder = new StringBuilder();
        QuestData data = QuestDB.Instance.GetQuestData(questCode);

        // QuestContents
        for (int i = 0; i < data.QuestIntroduce.Length; ++i)
        {
            builder.Append(data.QuestIntroduce[i]);
            builder.AppendLine();
        }
        ContentsText.text = builder.ToString();

        // QuestObjective
        ObjectiveTitleText.text = "퀘스트 진행도";
        builder = new StringBuilder();
        for (int categoryIdx = 0; categoryIdx < data.QuestCategorys.Length; ++categoryIdx)
        {
            switch(data.QuestCategorys[categoryIdx])
            {
                case "Discussion":
                    builder.Append("대화 : ");
                    builder.AppendLine();
                    DiscussionProgressInfo[] discussions = PlayerQuest.Instance.GetDetailedDiscussionProgresses(data.QuestCode);
                    for (int i = 0; i < discussions.Length; ++i)
                    {
                        string npcName = NpcDB.Instance.GetNPCData(discussions[i].TargetNPC).Name;
                        string talkDone = UIText_Util.Instance.BooleanToQuestIsDoneSTR(discussions[i].TalkCompleted);
                        builder.Append($"[{npcName} : {talkDone}]");
                        if (i < discussions.Length - 1)
                            builder.Append(", ");
                    }
                    break;
                case "KillMonster":
                    builder.Append("몬스터 사냥 : ");
                    builder.AppendLine();
                    KillMonsterProgressInfo[] killMonsters = PlayerQuest.Instance.GetDetailedKillMonsterProgresses(data.QuestCode);
                    for (int i = 0; i < killMonsters.Length; ++i)
                    {
                        string mobName = MonsterDB.Instance.GetMonsterData(killMonsters[i].TargetMonster).MonsterKorName;
                        builder.Append($"[{mobName} : {killMonsters[i].CurrentKillCount} / {killMonsters[i].GoalKillCount}]");
                        if (i < killMonsters.Length - 1)
                            builder.Append(",");
                    }
                    break;
                case "Building":
                    break;
                case "ItemGet":
                    break;
            }
        }
        ObjectiveContentsText.text = builder.ToString();

        // Reward
        RewardContentsText.text = GetQuestRewardSTR(data);
    }
    public void RefreshToCompleted(int questCode)
    {
        StringBuilder builder = new StringBuilder();
        QuestData data = QuestDB.Instance.GetQuestData(questCode);

        // QuestContents
        builder.Append("<color=#FFED72>퀘스트 본문 :</color>");
        builder.AppendLine();
        for (int i = 0; i < data.QuestIntroduce.Length; ++i)
        {
            builder.Append(data.QuestIntroduce[i]);
            builder.AppendLine();
        }
        builder.AppendLine();
        builder.Append("<color=#FFED72>퀘스트 성공 내용 :</color>");
        builder.AppendLine();
        for (int i = 0; i < data.QuestCompleteContents.Length; ++i)
        {
            builder.Append(data.QuestCompleteContents[i]);
            builder.AppendLine();
        }
        ContentsText.text = builder.ToString();

        // QuestObjective
        ObjectiveTitleText.text = "퀘스트 진행도";
        builder = new StringBuilder();
        for (int categoryIdx = 0; categoryIdx < data.QuestCategorys.Length; ++categoryIdx)
        {
            switch (data.QuestCategorys[categoryIdx])
            {
                case "Discussion":
                    builder.Append("대화 : ");
                    builder.AppendLine();

                    TargetNPCData[] npcs = data.Behaviour_Discussion.TargetNPC;
                    for (int i = 0; i < npcs.Length; ++i)
                    {
                        builder.Append($"[{npcs[i].NPCName}]");
                        if (i < npcs.Length - 1)
                            builder.Append(", ");
                    }
                    break;
                case "KillMonster":
                    builder.Append("몬스터 사냥 : ");
                    builder.AppendLine();
                    TargetMonsterData[] monsters = data.Behaviour_KillMonster.TargetMonster;
                    for (int i = 0; i < monsters.Length; ++i)
                    {
                        builder.Append($"[{monsters[i].MonsterName} : {monsters[i].KillCount}마리]");
                        if (i < monsters.Length - 1)
                            builder.Append(", ");
                    }
                    break;
                case "Building":
                    break;
                case "ItemGet":
                    break;
            }
        }
        ObjectiveContentsText.text = builder.ToString();

        // Reward
        RewardContentsText.text = GetQuestRewardSTR(data);
    }
    private string GetQuestRewardSTR(QuestData data)
    {
        StringBuilder builder = new StringBuilder();
        for (int rewardIdx = 0; rewardIdx < data.QuestRewards.Length; ++rewardIdx)
        {
            switch (data.QuestRewards[rewardIdx])
            {
                case "GetItem":
                    RewardItem[] items = data.Reward_GetItem.RewardItems;
                    for (int i = 0; i < items.Length; ++i)
                    {
                        ItemData item = ItemDB.Instance.GetItemData(items[i].ItemCode);
                        builder.Append($"[{item.Name} / {items[i].ItemCount}개]");
                        if (i < items.Length - 1)
                            builder.Append(", ");
                    }
                    break;
                case "GetStatus":
                    RewardStatus[] statuses = data.Reward_GetStatus.RewardStatuss;
                    for (int i = 0; i < statuses.Length; ++i)
                    {
                        builder.Append($"[{statuses[i].StatusName} / {statuses[i].Amount} 영구 획득]");
                        if (i < statuses.Length - 1)
                            builder.Append(", ");
                    }
                    break;
            }
        }
        return builder.ToString();
    }
}
