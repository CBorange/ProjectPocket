using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_QuestPanel_List : MonoBehaviour
{
    // Object
    public GameObject QuestSelectTogglePrefab;
    public ToggleGroup QuestToggleGroup;
    public Player_QuestPanel QuestPanel;
    private List<QuestSelectToggle> questSelectTogglePool;

    // UI
    public Text listTitleText;
    public ToggleGroup categoryToggleGroup;
    public Toggle[] categoryToggles;

    // Data
    private List<QuestData> questsInProgress;
    private List<QuestData> questsCompleted;

    public void Initialize()
    {
        questsInProgress = new List<QuestData>();
        questsCompleted = new List<QuestData>();
        questSelectTogglePool = new List<QuestSelectToggle>();
        CreateQuestSelectToggles(30);
    }
    public void OpenPanel()
    {
        categoryToggleGroup.allowSwitchOff = true;
        categoryToggles[0].isOn = true;
    }
    public void ClosePanel()
    {
        categoryToggleGroup.allowSwitchOff = true;
        for (int i = 0; i < categoryToggles.Length; ++i)
            categoryToggles[i].isOn = false;
    }

    private void CreateQuestSelectToggles(int createCount)
    {
        questSelectTogglePool.Capacity += createCount;
        for (int i = 0; i < createCount; ++i)
        {
            GameObject newToggle = Instantiate(QuestSelectTogglePrefab, QuestToggleGroup.transform);
            newToggle.GetComponent<Toggle>().group = QuestToggleGroup;

            QuestSelectToggle questSelectToggle = newToggle.GetComponent<QuestSelectToggle>();
            questSelectToggle.Initialize(QuestSelected);

            questSelectTogglePool.Add(questSelectToggle);
            newToggle.SetActive(false);
        }
    }
    private void RefreshListToProgress()
    {
        QuestToggleGroup.allowSwitchOff = true;
        for (int i = 0; i < questSelectTogglePool.Count; ++i)
        {
            questSelectTogglePool[i].GetComponent<Toggle>().isOn = false;
            questSelectTogglePool[i].gameObject.SetActive(false);
        }
        QuestToggleGroup.allowSwitchOff = false;

        listTitleText.text = "진행 중 퀘스트 목록";
        questsInProgress.Clear();
        foreach (var kvp in PlayerQuest.Instance.QuestsInProgress)
            questsInProgress.Add(kvp.Value.OriginalQuestData);

        for (int i = 0; i < questsInProgress.Count; ++i)
            questSelectTogglePool[i].Refresh(questsInProgress[i], QuestSelectToggleCategory.InProgress);
    }
    private void RefreshListToCompleted()
    {
        for (int i = 0; i < questSelectTogglePool.Count; ++i)
        {
            questSelectTogglePool[i].GetComponent<Toggle>().isOn = false;
            questSelectTogglePool[i].gameObject.SetActive(false);
        }

        listTitleText.text = "완료 퀘스트 목록";
        questsCompleted.Clear();
        foreach (var kvp in PlayerQuest.Instance.CompletedQuests)
            questsCompleted.Add(kvp.Value);

        for (int i = 0; i < questsCompleted.Count; ++i)
            questSelectTogglePool[i].Refresh(questsCompleted[i], QuestSelectToggleCategory.Complete);
    }

    // ChangeListCategory Callback Method
    public void ChangedCategory_To_InProgress(bool selected)
    {
        if (!selected)
            return;
        categoryToggleGroup.allowSwitchOff = false;
        RefreshListToProgress();
    }
    public void ChangedCategory_To_Complete(bool selected)
    {
        if (!selected)
            return;
        categoryToggleGroup.allowSwitchOff = false;
        RefreshListToCompleted();
    }

    // QuestSelected Callback
    private void QuestSelected(QuestSelectToggle selectToggle)
    {
        if (selectToggle.ToggleCategory == QuestSelectToggleCategory.InProgress)
            QuestPanel.Refresh_ObjectiveToInProgress(selectToggle.CurrentQuest.QuestCode);
        else if (selectToggle.ToggleCategory == QuestSelectToggleCategory.Complete)
            QuestPanel.Refresh_ObjectiveToCompleted(selectToggle.CurrentQuest.QuestCode);
    }
}
