using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_QuestPanel : MonoBehaviour
{
    // Child Panels
    public NPC_QuestPanel_Objective objectivePanel;
    public NPC_QuestPanel_List listPanel;

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
        listPanel.RefrehPanel();
        objectivePanel.RefreshPanel();
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
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
