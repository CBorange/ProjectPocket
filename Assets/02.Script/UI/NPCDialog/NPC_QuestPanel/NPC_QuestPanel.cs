using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_QuestPanel : MonoBehaviour
{
    // Controller
    public NPC_QuestPanel_Objective objectivePanel;
    public NPC_QuestPanel_List listPanel;
    public AlertPopup alertPopup;

    // Data
    private NPCData currentNPCData;

    public void Initialize()
    {
        objectivePanel.Initialize();
        listPanel.Initialize();
    }
    public void OpenPanel(NPCData npc)
    {
        currentNPCData = npc;
        gameObject.SetActive(true);

        objectivePanel.OpenPanel(currentNPCData.NPCCode);
        listPanel.OpenPanel(currentNPCData);
    }
    public void RefreshPanel()
    {
        objectivePanel.RefreshPanel();
        listPanel.RefrehPanel();
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
        alertPopup.ClosePopup();
    }

    // Method For Comunicate Between Quest List<->Objective Panel
    public void OpenObjectivePanel_Acceptable(QuestData data)
    {
        objectivePanel.RefreshToAcceptable(data);
    }
    public void OpenObjectivePanel_Complete(QuestData data)
    {
        objectivePanel.RefreshToComplete(data);
    }
}
