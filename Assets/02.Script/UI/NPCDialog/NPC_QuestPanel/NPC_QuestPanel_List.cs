using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NPC_QuestPanel_List : MonoBehaviour
{
    // Object
    public GameObject QuestSelectTogglePrefab;
    private List<QuestSelectToggle> acceptableSelectTogglePool;
    private List<QuestSelectToggle> completeSelectTogglePool;

    // UI
    public ToggleGroup AcceptableQuestToggleGroup;
    public ToggleGroup ComepleteQuestToggleGroup;
    public NPC_QuestPanel questPanel;

    // Data
    private List<QuestData> acceptableQuests;
    private List<QuestData> completeQuests;

    public void Initialize()
    {
        acceptableSelectTogglePool = new List<QuestSelectToggle>();
        completeSelectTogglePool = new List<QuestSelectToggle>();

        CreateQuestSelectToggles(30, AcceptableQuestToggleGroup, acceptableSelectTogglePool, QuestSelectToggleCategory.Acceptable);
        CreateQuestSelectToggles(30, ComepleteQuestToggleGroup, completeSelectTogglePool, QuestSelectToggleCategory.Complete);

        acceptableQuests = new List<QuestData>();
        completeQuests = new List<QuestData>();
    }

    public void OpenPanel(QuestData[] originalQuestDatas)
    {
        // Deactive All Toggles
        for (int i = 0; i < acceptableSelectTogglePool.Count; ++i)
        {
            acceptableSelectTogglePool[i].gameObject.SetActive(false);
            acceptableSelectTogglePool[i].GetComponent<Toggle>().isOn = false;
        }
        for (int i = 0; i < completeSelectTogglePool.Count; ++i)
        {
            completeSelectTogglePool[i].gameObject.SetActive(false);
            completeSelectTogglePool[i].GetComponent<Toggle>().isOn = false;
        }

        // Divide Quests To Acceptable & Complete
        acceptableQuests.Clear();
        completeQuests.Clear();

        for (int questIdx = 0; questIdx < originalQuestDatas.Length; ++questIdx)
        {
            QuestData currentData = originalQuestDatas[questIdx];
            if (!PlayerQuest.Instance.GetQuestIsInComplete(currentData.QuestCode))
            {
                // Divide Complete Quest
                if (PlayerQuest.Instance.GetQuestIsInProgress(currentData.QuestCode))
                {
                    if (PlayerQuest.Instance.GetQuestIsCompletedInProgress(currentData.QuestCode))
                        completeQuests.Add(currentData);
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

        // Refresh QuestToggles
        for (int i = 0; i < acceptableQuests.Count; ++i)
            acceptableSelectTogglePool[i].Refresh(acceptableQuests[i]);
        for (int i = 0; i < completeQuests.Count; ++i)
            completeSelectTogglePool[i].Refresh(completeQuests[i]);
    }

    private void CreateQuestSelectToggles(int createCount, ToggleGroup parentGroup, List<QuestSelectToggle> togglePool, QuestSelectToggleCategory category)
    {
        togglePool.Capacity += createCount;
        for (int i = 0; i < createCount; ++i)
        {
            GameObject newToggle = Instantiate(QuestSelectTogglePrefab, parentGroup.transform);
            newToggle.GetComponent<Toggle>().group = parentGroup;

            QuestSelectToggle questSelectToggle = newToggle.GetComponent<QuestSelectToggle>();
            questSelectToggle.Initialize(QuestSelected, category);

            togglePool.Add(questSelectToggle);
            newToggle.SetActive(false);
        }
    }

    private void QuestSelected(QuestSelectToggle selectToggle)
    {
        if (selectToggle.ToggleCategory == QuestSelectToggleCategory.Acceptable)
        {
            for (int i = 0; i < completeSelectTogglePool.Count; ++i)
                completeSelectTogglePool[i].GetComponent<Toggle>().isOn = false;
            questPanel.OpenObjectivePanel_Acceptable(selectToggle.CurrentQuest);
        }
        else if(selectToggle.ToggleCategory == QuestSelectToggleCategory.Complete)
        {
            for (int i = 0; i < acceptableSelectTogglePool.Count; ++i)
                acceptableSelectTogglePool[i].GetComponent<Toggle>().isOn = false;
            questPanel.OpenObjectivePanel_Complete(selectToggle.CurrentQuest);
        }
    }
}
