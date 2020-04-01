using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class QuestDB
{
    // Singleton
    private QuestDB()
    {
        questDatas = new Dictionary<int, QuestData>();
    }
    private static QuestDB instance;
    public static QuestDB Instance
    {
        get
        {
            if (instance == null)
                instance = new QuestDB();
            return instance;
        }
    }

    // Data
    private Dictionary<int, QuestData> questDatas;

    public QuestData GetQuestData(int questCode)
    {
        QuestData foundData;
        bool foundSuccess = questDatas.TryGetValue(questCode, out foundData);
        if (!foundSuccess)
        {
            foundData = DBConnector.Instance.LoadQuestData(questCode);
            questDatas.Add(questCode, foundData);
            return foundData;
        }
        else
            return foundData;
    }
}
