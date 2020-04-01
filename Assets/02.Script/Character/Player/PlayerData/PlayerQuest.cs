using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuest : MonoBehaviour, PlayerRuntimeData
{
    #region Singleton
    private static PlayerQuest instance;
    public static PlayerQuest Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<PlayerQuest>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("PlayerQuest").AddComponent<PlayerQuest>();
                    instance = newSingleton;
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<PlayerQuest>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    // Data
    private List<QuestTotalProgress> questsInProgress;
    public List<QuestTotalProgress> QuestsInProgress
    {
        get { return questsInProgress; }
    }
    private QuestProgress_Discussion questProgress_Discussion;
    public QuestProgress_Discussion QuestProgress_Discussion
    {
        get { return questProgress_Discussion; }
    }
    private QuestProgress_KillMonster questProgress_KillMonster;
    public QuestProgress_KillMonster QuestProgress_KillMonster
    {
        get { return questProgress_KillMonster; }
    }

    private List<QuestData> completedQuests;
    private List<QuestData> CompletedQuests
    {
        get { return completedQuests; }
    }
    public void Initialize()
    {
        // 퀘스트 진행상황 불러오기
        if (UserQuestProvider.Instance.QuestDatasInProgress.Count > 0)
        {
            questsInProgress = new List<QuestTotalProgress>();
            questProgress_Discussion = UserQuestProvider.Instance.QuestProgress_Discussion;
            questProgress_KillMonster = UserQuestProvider.Instance.QuestProgress_KillMonster;

            for (int i = 0; i < UserQuestProvider.Instance.QuestDatasInProgress.Count; ++i) 
            {
                QuestTotalProgress totalProgress = new QuestTotalProgress();
                totalProgress.Completed = false;
                totalProgress.OriginalQuestData = UserQuestProvider.Instance.QuestDatasInProgress[i];
                questsInProgress.Add(totalProgress);
            }
            UpdateTotalQuestProgress();
        }
        else
        {
            questsInProgress = new List<QuestTotalProgress>();
            questProgress_Discussion = new QuestProgress_Discussion();
            questProgress_KillMonster = new QuestProgress_KillMonster();
        }
        // 완료된 퀘스트 불러오기
        if (UserQuestProvider.Instance.CompletedQuests.Count > 0)
            completedQuests = UserQuestProvider.Instance.CompletedQuests;
        else
            completedQuests = new List<QuestData>();
    }
    private void UpdateTotalQuestProgress()
    {
        for (int questIdx = 0; questIdx < questsInProgress.Count; ++questIdx)
        {
            QuestData currentQuest = questsInProgress[questIdx].OriginalQuestData;
            string[] questCategorys = currentQuest.QuestCategorys.Split(',');
            int completedCategoryCount = 0;

            for (int questCategoryIdx = 0; questCategoryIdx < questCategorys.Length; ++questCategoryIdx)
            {
                switch (questCategorys[questCategoryIdx])
                {
                    case "Discussion":
                        bool completedDiscussion = questProgress_Discussion.GetCompletedAllDiscussion(currentQuest.QuestCode);
                        if (completedDiscussion)
                            completedCategoryCount += 1;
                        break;
                    case "KillMonster":
                        bool completedKillMonster = questProgress_KillMonster.GetCompletedAllKillMonster(currentQuest.QuestCode);
                        if (completedKillMonster)
                            completedCategoryCount += 1;
                        break;
                }
            }
            if (completedCategoryCount == questCategorys.Length)
            {
                questsInProgress[questIdx].Completed = true;
            }
        }
        return;
    }

    // 퀘스트에 영향을 주는 행동 Callback
    public void KilledMonster(int monsterCode)
    {

    }
    public void TalkToNPC(NPCData data)
    {

    }
}
