using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserQuestProvider
{
    // Singleton
    private UserQuestProvider() 
    {
        questDatasInProgress = new List<QuestData>();

        questProgress_Discussion = new QuestProgress_Discussion();
        questProgress_Discussion.Initiailize();

        questProgress_KillMonster = new QuestProgress_KillMonster();
        questProgress_KillMonster.Initialize();

        completedQuests = new List<QuestData>();
    }
    private static UserQuestProvider instance;
    public static UserQuestProvider Instance
    {
        get
        {
            if (instance == null)
                instance = new UserQuestProvider();
            return instance;
        }
    }

    // Data
    private List<QuestData> questDatasInProgress;
    public List<QuestData> QuestDatasInProgress
    {
        get { return questDatasInProgress; }
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
    public List<QuestData> CompletedQuests
    {
        get { return completedQuests; }
    }

    public void Initialize(int[] questCodesInProgress, int[] questCodesCompleted, QuestProgress_Discussion progress_Discussion, QuestProgress_KillMonster progress_KillMonster)
    {
        if (questCodesInProgress != null)
        {
            for (int i = 0; i < questCodesInProgress.Length; ++i)
            {
                QuestData data = QuestDB.Instance.GetQuestData(questCodesInProgress[i]);
                questDatasInProgress.Add(data);
            }
        }
        if (questCodesCompleted != null)
        {
            for (int i = 0; i < questCodesCompleted.Length; ++i)
            {
                try
                {
                    QuestData data = QuestDB.Instance.GetQuestData(questCodesCompleted[i]);
                    completedQuests.Add(data);
                }
                catch (Exception)
                {
                    Debug.Log($"UserQuestProvider : {questCodesCompleted[i]} 데이터가 존재하지 않음");
                }
            }
        }

        questProgress_Discussion = progress_Discussion;
        questProgress_Discussion.Initiailize();

        questProgress_KillMonster = progress_KillMonster;
        questProgress_KillMonster.Initialize();
    }

    public void SavePlayerQuest_UpdateServerDB()
    {
        questDatasInProgress.Clear();
        foreach (var kvp in PlayerQuest.Instance.QuestsInProgress)
            questDatasInProgress.Add(kvp.Value.OriginalQuestData);
        completedQuests.Clear();
        foreach (var kvp in PlayerQuest.Instance.CompletedQuests)
            completedQuests.Add(kvp.Value);

        PlayerQuest.Instance.SaveQuestProgressData();
        DBConnector.Instance.Save_PlayerQuest();
    }
}
