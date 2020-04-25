using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ShopInfo
{
    public string[] SalesItemTypes;
    public ShopItem[] WeaponItems;
    public ShopItem[] AccesorieItems;
    public ShopItem[] ExpendableItems;
    public ShopItem[] EtcItems;
}
[System.Serializable]
public class ShopItem
{
    public int ItemCode;
    public int Price;
}
[System.Serializable]
public class NPCData
{
    // JSON Parsing
    public string Name;
    public int NPCCode;
    public string Introduce;
    public string[] Behaviours;
    public int[] Quest;
    public string[] Disccusion;
    public ShopInfo ShopData;

    // Quest
    [System.NonSerialized]
    public QuestData[] QuestDatas;

    [System.NonSerialized]
    private List<QuestData> acceptableQuests;
    public List<QuestData> AcceptableQuests
    {
        get { return acceptableQuests; }
    }

    [System.NonSerialized]
    private List<QuestData> completeQuests;
    public List<QuestData> CompleteQuests
    {
        get { return completeQuests; }
    }

    [System.NonSerialized]
    private List<QuestData> inProgressQuests;
    public List<QuestData> InProgressQuests
    {
        get { return inProgressQuests; }
    }

    public NPCData() { }

    // Method
    public void Initialize()
    {
        QuestDatas = new QuestData[Quest.Length];
        acceptableQuests = new List<QuestData>();
        completeQuests = new List<QuestData>();
        inProgressQuests = new List<QuestData>();
    }
    public void SeperateQuestsAccordingToState()
    {
        acceptableQuests.Clear();
        completeQuests.Clear();
        inProgressQuests.Clear();

        for (int questIdx = 0; questIdx < QuestDatas.Length; ++questIdx)
        {
            QuestData currentData = QuestDatas[questIdx];
            if (!PlayerQuest.Instance.GetQuestIsInComplete(currentData.QuestCode))
            {
                // Divide Complete & InProgress Quest
                if (PlayerQuest.Instance.GetQuestIsInProgress(currentData.QuestCode))
                {
                    if (PlayerQuest.Instance.GetQuestIsCompletedInProgress(currentData.QuestCode))
                        completeQuests.Add(currentData);
                    else
                        inProgressQuests.Add(currentData);
                }
                // Divide Acceptable Quest
                else
                {
                    // 클리어 해야할 퀘스트가 존재한다면
                    if (currentData.PrecedentQuests.Length != 0)
                    {
                        int precedentCount = 0;
                        for (int precedentIdx = 0; precedentIdx < currentData.PrecedentQuests.Length; ++precedentIdx)
                        {
                            if (PlayerQuest.Instance.GetQuestIsInComplete(currentData.PrecedentQuests[precedentIdx]))
                                precedentCount += 1;
                        }
                        if (precedentCount == currentData.PrecedentQuests.Length)
                            acceptableQuests.Add(currentData);
                    }
                    else
                        acceptableQuests.Add(currentData);
                }
            }
        }
    }
}
