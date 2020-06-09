using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuest : MonoBehaviour, PlayerRuntimeData, QuestUpdater
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

    #region Observer
    private List<QuestObserver> questObservers;
    public void AddObserver(QuestObserver observer)
    {
        questObservers.Add(observer);
    }
    public void DeleteObserver(QuestObserver observer)
    {
        questObservers.Remove(observer);
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
        questObservers = new List<QuestObserver>();
        questObservers.Add(QuestNoticePopup.Instance.GetComponent<QuestObserver>());

        // 퀘스트 진행상황 불러오기
        questsInProgress = new Dictionary<int, QuestTotalProgress>();
        completedQuests = new Dictionary<int, QuestData>();

        questProgress_Discussion = UserQuestProvider.Instance.QuestProgress_Discussion;
        questProgress_KillMonster = UserQuestProvider.Instance.QuestProgress_KillMonster;
        for (int i = 0; i < questObservers.Count; ++i)
        {
            questProgress_Discussion.AddObserver(questObservers[i]);
            questProgress_KillMonster.AddObserver(questObservers[i]);
        }

        if (UserQuestProvider.Instance.QuestDatasInProgress.Count > 0)
        {
            for (int i = 0; i < UserQuestProvider.Instance.QuestDatasInProgress.Count; ++i) 
            {
                QuestTotalProgress totalProgress = new QuestTotalProgress();
                totalProgress.Completed = false;
                totalProgress.OriginalQuestData = UserQuestProvider.Instance.QuestDatasInProgress[i];
                for (int questTypeIdx = 0; i < totalProgress.OriginalQuestData.QuestCategorys.Length; ++i)
                {
                    switch (totalProgress.OriginalQuestData.QuestCategorys[questTypeIdx])
                    {
                        case "GetItem":
                            totalProgress.OriginalQuestData.Behaviour_GetItem.Initialize();
                            for (int observerIdx = 0; observerIdx < questObservers.Count; ++observerIdx)
                                totalProgress.OriginalQuestData.Behaviour_GetItem.AddObserver(questObservers[observerIdx]);
                            break;
                        case "Building":
                            totalProgress.OriginalQuestData.Behaviour_Building.Initialize();
                            for (int observerIdx = 0; observerIdx < questObservers.Count; ++observerIdx)
                                totalProgress.OriginalQuestData.Behaviour_Building.AddObserver(questObservers[observerIdx]);
                            break;
                    }
                }
                
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
    /// <summary>
    /// 퀘스트의 각 Behaviour가 완수 되었는지 검사하고 이를 해당 QuestProgress에 업데이트 시킴
    /// </summary>
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
                        if (questProgress_Discussion.GetHasCompletedByQuestCode(currentQuest.QuestCode))
                            completedCategoryCount += 1;
                        break;
                    case "KillMonster":
                        if (questProgress_KillMonster.GetHasCompletedByQuestCode(currentQuest.QuestCode))
                            completedCategoryCount += 1;
                        break;
                    case "Building":
                        if (currentQuest.Behaviour_Building.GetHasCompletedAllBuildingConstruct())
                            completedCategoryCount += 1;
                        break;
                    case "GetItem":
                        if (currentQuest.Behaviour_GetItem.GetHasCompletedAllItemGet())
                            completedCategoryCount += 1;
                        break;
                }
            }
            // 이전에 완료되지 않았고, 새로 완료될 때 실행
            if (completedCategoryCount == currentQuest.QuestCategorys.Length &&
                !kvp.Value.Completed)
            {
                for (int i = 0; i < questObservers.Count; ++i)
                    questObservers[i].Update_QuestComplete(currentQuest.QuestCode);
                kvp.Value.Completed = true;
            }
            else if (completedCategoryCount != currentQuest.QuestCategorys.Length)
                kvp.Value.Completed = false;
        }
    }

    // 퀘스트에 영향을 주는 행동 Callback

    public void KilledMonster(int monsterCode)
    {
        questProgress_KillMonster.KilledMonster(monsterCode);
        UpdateAllQuestProgress();
        NPC_ControllerGroup.Instance.QuestStateWasChanged();
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
        NPC_ControllerGroup.Instance.QuestStateWasChanged();
        if (discussionSTR != null)
            return discussionSTR;
        else
            return null;
    }

    public void UpdateQuestSellItem()
    {
        UpdateAllQuestProgress();
        NPC_ControllerGroup.Instance.QuestStateWasChanged();
    }
    public void UpdateGetItemQuest(int itemCode)
    {
        foreach (var kvp in questsInProgress)
        {
            QuestData currentQuest = kvp.Value.OriginalQuestData;
            for (int categoryIdx = 0; categoryIdx < currentQuest.QuestCategorys.Length; ++categoryIdx)
            {
                if (currentQuest.QuestCategorys[categoryIdx].Equals("GetItem"))
                {
                    currentQuest.Behaviour_GetItem.NoticeQuestProgress(itemCode);
                }
            }
        }
        UpdateAllQuestProgress();
        NPC_ControllerGroup.Instance.QuestStateWasChanged();
    }
    public void UpdateBuildingQuest(int buildingCode)
    {
        foreach (var kvp in questsInProgress)
        {
            QuestData currentQuest = kvp.Value.OriginalQuestData;
            for (int categoryIdx = 0; categoryIdx < currentQuest.QuestCategorys.Length; ++categoryIdx)
            {
                if (currentQuest.QuestCategorys[categoryIdx].Equals("Building"))
                {
                    currentQuest.Behaviour_Building.NoticeQuestProgress(buildingCode);
                }
            }
        }
        UpdateAllQuestProgress();
        NPC_ControllerGroup.Instance.QuestStateWasChanged();
    }

    public void StartQuest(int npcCode, int questCode)
    {
        QuestTotalProgress newProgress = new QuestTotalProgress();
        newProgress.Completed = false;
        newProgress.OriginalQuestData = QuestDB.Instance.GetQuestData(questCode);
        questsInProgress.Add(questCode, newProgress);

        string[] categorys = newProgress.OriginalQuestData.QuestCategorys;
        for (int categoryIdx = 0; categoryIdx < categorys.Length; ++categoryIdx) 
        {
            switch(categorys[categoryIdx])
            {
                case "Discussion":
                    TargetNPCData[] npcDatas = newProgress.OriginalQuestData.Behaviour_Discussion.TargetNPC;

                    int[] npcCodes = new int[npcDatas.Length];
                    for (int i = 0; i < npcCodes.Length; ++i)
                        npcCodes[i] = npcDatas[i].NPCCode;

                    questProgress_Discussion.StartQuest(questCode, npcCodes);
                    break;
                case "KillMonster":
                    TargetMonsterData[] monsterDatas = newProgress.OriginalQuestData.Behaviour_KillMonster.TargetMonster;
                    questProgress_KillMonster.StartQuest(questCode, monsterDatas);
                    break;
                case "GetItem":
                    newProgress.OriginalQuestData.Behaviour_GetItem.Initialize();
                    for (int i = 0; i < questObservers.Count; ++i)
                        newProgress.OriginalQuestData.Behaviour_GetItem.AddObserver(questObservers[i]);
                    break;
                case "Building":
                    newProgress.OriginalQuestData.Behaviour_Building.Initialize();
                    for (int i = 0; i < questObservers.Count; ++i)
                        newProgress.OriginalQuestData.Behaviour_Building.AddObserver(questObservers[i]);
                    break;
            }
        }

        UpdateAllQuestProgress();
        NPC_ControllerGroup.Instance.QuestStateWasChanged();
    }
    public void CompleteQuest(int npcCode, QuestData data)
    {
        // 보상 수령
        string[] rewardSTR = data.QuestRewards;
        for (int rewardIdx = 0; rewardIdx < rewardSTR.Length; ++rewardIdx)
        {
            switch(rewardSTR[rewardIdx])
            {
                case "GetItem":
                    RewardItem[] items = data.Reward_GetItem.RewardItems;
                    InventoryItem[] newItems = new InventoryItem[items.Length];
                    for (int itemIdx = 0; itemIdx < items.Length; ++itemIdx)
                    {
                        ItemData rewardItem = ItemDB.Instance.GetItemData(items[itemIdx].ItemCode);
                        newItems[itemIdx] = new InventoryItem(rewardItem, items[itemIdx].ItemCount);
                        
                    }
                    PlayerInventory.Instance.AddItemToInventory(newItems);
                    break;
                case "GetStatus":
                    RewardStatus[] statuss = data.Reward_GetStatus.RewardStatuss;
                    for (int statusIdx = 0; statusIdx < statuss.Length; ++statusIdx)
                    {
                        switch(statuss[statusIdx].StatusType)
                        {
                            case "Gold":
                                PlayerStat.Instance.AddGold(statuss[statusIdx].Amount);
                                break;
                        }
                    }
                    break;
            }
        }

        // 퀘스트 종류별로 퀘스트 완료시에 필요한 처리 실행
        for (int i = 0; i < data.QuestCategorys.Length; ++i)
        {
            switch (data.QuestCategorys[i])
            {
                case "Discussion":
                    questProgress_Discussion.CompleteQuest(data.QuestCode);
                    break;
                case "KillMonster":
                    questProgress_KillMonster.CompleteQuest(data.QuestCode);
                    break;
                case "GetItem":
                    TargetItemData[] items = data.Behaviour_GetItem.TargetItem;
                    for (int itemIdx = 0; itemIdx < items.Length; ++itemIdx)
                        PlayerInventory.Instance.RemoveItemFromInventory(items[itemIdx].ItemCode, items[itemIdx].ItemCount);
                    for (int observerIdx = 0; observerIdx < questObservers.Count; ++observerIdx)
                        data.Behaviour_GetItem.DeleteObserver(questObservers[observerIdx]);
                    break;
                case "Building":
                    for (int observerIdx = 0; observerIdx < questObservers.Count; ++observerIdx)
                        data.Behaviour_Building.DeleteObserver(questObservers[observerIdx]);
                    break;
            }
        }
        if (questsInProgress.Remove(data.QuestCode))
            completedQuests.Add(data.QuestCode, data);
        else
        {
            Debug.Log($"퀘스트 성공 실패, {data.QuestCode}");
        }
        NPC_ControllerGroup.Instance.QuestStateWasChanged();
    }
    public void SaveQuestProgressData()
    {
        questProgress_Discussion.SaveProgress();
        questProgress_KillMonster.SaveProgress();
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
