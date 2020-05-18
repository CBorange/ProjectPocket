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
    private NPCData currnetNPC;

    public void Initialize()
    {
        acceptableSelectTogglePool = new List<QuestSelectToggle>();
        completeSelectTogglePool = new List<QuestSelectToggle>();

        CreateQuestSelectToggles(30, AcceptableQuestToggleGroup, acceptableSelectTogglePool, QuestSelectToggleCategory.Acceptable);
        CreateQuestSelectToggles(30, ComepleteQuestToggleGroup, completeSelectTogglePool, QuestSelectToggleCategory.Complete);
    }

    public void OpenPanel(NPCData npc)
    {
        currnetNPC = npc;
        RefrehPanel();

        if (currnetNPC.AcceptableQuests.Count > 0)
        {
            acceptableSelectTogglePool[0].GetComponent<Toggle>().isOn = true;
            acceptableSelectTogglePool[0].QuestSelected(true);
            return;
        }
        if (currnetNPC.CompleteQuests.Count > 0)
        {
            completeSelectTogglePool[0].GetComponent<Toggle>().isOn = true;
            completeSelectTogglePool[0].QuestSelected(true);
            return;
        }
    }
    public void RefrehPanel()
    {
        currnetNPC.SeperateQuestsAccordingToState();
        DeactiveAllToggles();

        // Refresh QuestToggles
        for (int i = 0; i < currnetNPC.AcceptableQuests.Count; ++i)
            acceptableSelectTogglePool[i].Refresh(currnetNPC.AcceptableQuests[i]);
        for (int i = 0; i < currnetNPC.CompleteQuests.Count; ++i)
            completeSelectTogglePool[i].Refresh(currnetNPC.CompleteQuests[i]);
    }

    private void DeactiveAllToggles()
    {
        // Deactive All Toggles
        for (int i = 0; i < acceptableSelectTogglePool.Count; ++i)
            acceptableSelectTogglePool[i].gameObject.SetActive(false);
        for (int i = 0; i < completeSelectTogglePool.Count; ++i)
            completeSelectTogglePool[i].gameObject.SetActive(false);
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
            questPanel.OpenObjectivePanel_Acceptable(selectToggle.CurrentQuest);
        }
        else if(selectToggle.ToggleCategory == QuestSelectToggleCategory.Complete)
        {
            questPanel.OpenObjectivePanel_Complete(selectToggle.CurrentQuest);
        }
    }
}
