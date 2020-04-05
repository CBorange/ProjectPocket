﻿using System.Collections;
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
    private Dictionary<int, QuestTotalProgress> questsInProgress;
    public Dictionary<int,QuestTotalProgress> QuestsInProgress
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

    private Dictionary<int,QuestData> completedQuests;
    public Dictionary<int, QuestData> CompletedQuests
    {
        get { return completedQuests; }
    }
    public void Initialize()
    {
        // 퀘스트 진행상황 불러오기
        questsInProgress = new Dictionary<int, QuestTotalProgress>();
        questProgress_Discussion = UserQuestProvider.Instance.QuestProgress_Discussion;
        questProgress_KillMonster = UserQuestProvider.Instance.QuestProgress_KillMonster;
        completedQuests = new Dictionary<int, QuestData>();

        if (UserQuestProvider.Instance.QuestDatasInProgress.Count > 0)
        {
            for (int i = 0; i < UserQuestProvider.Instance.QuestDatasInProgress.Count; ++i) 
            {
                QuestTotalProgress totalProgress = new QuestTotalProgress();
                totalProgress.Completed = false;
                totalProgress.OriginalQuestData = UserQuestProvider.Instance.QuestDatasInProgress[i];
                questsInProgress.Add(totalProgress.OriginalQuestData.QuestCode, totalProgress);
            }
            UpdateAllQuestProgress();
        }
        
        // 완료된 퀘스트 불러오기
        if (UserQuestProvider.Instance.CompletedQuests.Count > 0)
        {
            for (int i = 0; i < UserQuestProvider.Instance.CompletedQuests.Count; ++i)
            {
                QuestData data = UserQuestProvider.Instance.CompletedQuests[i];
                completedQuests.Add(data.QuestCode, data);
            }
        }
        else
            completedQuests = new Dictionary<int, QuestData>();
    }
    private void UpdateAllQuestProgress()
    {
        foreach (var kvp in questsInProgress)
        {
            QuestData currentQuest = kvp.Value.OriginalQuestData;
            int completedCategoryCount = 0;
            for (int categoryIdx = 0; categoryIdx < currentQuest.QuestCategorys.Length; ++categoryIdx)
            {
                switch (currentQuest.QuestCategorys[categoryIdx])
                {
                    case "Discussion":
                        bool completedDiscussion = questProgress_Discussion.GetHasCompletedOnTotalDiscussion(currentQuest.QuestCode);
                        if (completedDiscussion)
                            completedCategoryCount += 1;
                        break;
                    case "KillMonster":
                        bool completedKillMonster = questProgress_KillMonster.GetHasCompletedOnTotalKillMonster(currentQuest.QuestCode);
                        if (completedKillMonster)
                            completedCategoryCount += 1;
                        break;
                }
            }
            if (completedCategoryCount == currentQuest.QuestCategorys.Length)
                kvp.Value.Completed = true;
        }
        return;
    }

    // 퀘스트에 영향을 주는 행동 Callback

    public void KilledMonster(int monsterCode)
    {

    }

    /// <summary>
    /// NPC와 대화를 시작할 때 호출하는 함수, 
    /// 해당 NPC와 대화 퀘스트가 존재하면 Discussion Quest 진행상황을 업데이트 함
    /// 만약 진행상황이 존재하지 않으면 아무것도 하지 않고 null 반환
    /// </summary>
    /// <param name="npcCode"></param>
    /// <returns></returns>
    public string[] GetDiscussionWhenTalkToNPC(int npcCode)
    {
        string[] discussionSTR = questProgress_Discussion.GetDiscussionSTR(npcCode);
        UpdateAllQuestProgress();
        if (discussionSTR != null)
            return discussionSTR;
        else
            return null;
    }

    public void UpdateGetItemQuest()
    { 

    }
    public void UpdateBuildingQuest()
    {

    }

    public void StartQuest(int questCode)
    {

    }
    public void CompleteQuest(int questCode)
    {

    }

    // 퀘스트 데이터 Getter
    public bool GetQuestIsInProgress(int questCode)
    {
        return questsInProgress.ContainsKey(questCode);
    }
    public bool GetQuestIsInComplete(int questCode)
    {
        return completedQuests.ContainsKey(questCode);
    }
    public bool GetQuestIsCompletedInProgress(int questCode)
    {
        QuestTotalProgress totalProgress = null;
        if (questsInProgress.TryGetValue(questCode, out totalProgress))
        {
            if (totalProgress.Completed)
                return true;
            else
                return false;
        }
        return false;
    }
    public DiscussionProgressInfo[] GetDetailedDiscussionProgresses(int questCode)
    {
        return questProgress_Discussion.GetDetailedDiscussionProgresses(questCode);
    }
    public KillMonsterProgressInfo[] GetDetailedKillMonsterProgresses(int questCode)
    {
        return questProgress_KillMonster.GetDetailedKillMonsterProgresses(questCode);
    }
}
