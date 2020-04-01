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
        questProgress_KillMonster = new QuestProgress_KillMonster();
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

    public void Initialize_ProgressQuest(int[] questCodesInProgress, QuestProgress_Discussion progress_Discussion, QuestProgress_KillMonster progress_KillMonster)
    {
        for (int i = 0; i < questCodesInProgress.Length; ++i)
        {
            QuestData data = DBConnector.Instance.LoadQuestData(questCodesInProgress[i]);
            questDatasInProgress.Add(data);
        }
        questProgress_Discussion = progress_Discussion;
        questProgress_KillMonster = progress_KillMonster;
    }
    public void Initialize_CompletedQuest(int[] completedQuestCodes)
    {
        for (int i = 0; i < completedQuestCodes.Length; ++i)
        {
            try
            {
                QuestData data = DBConnector.Instance.LoadQuestData(completedQuestCodes[i]);
                completedQuests.Add(data);
            }
            catch(Exception)
            {
                Debug.Log($"UserQuestProvider : {completedQuestCodes[i]} 데이터가 존재하지 않음");
            }
        }
    }
}
