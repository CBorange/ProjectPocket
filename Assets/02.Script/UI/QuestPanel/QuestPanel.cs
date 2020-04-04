using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestPanel : MonoBehaviour
{
    // Child Panels
    public QuestPanel_Objective objectivePanel;
    public QuestPanel_List listPanel;

    // Data
    private QuestData[] currentQuestDatas;

    public void Initialize()
    {
        objectivePanel.Initialize();
        listPanel.Initialize();
    }
    public void OpenPanel(QuestData[] questDatas)
    {
        currentQuestDatas = questDatas;
        gameObject.SetActive(true);

        objectivePanel.OpenPanel();
        listPanel.OpenPanel(currentQuestDatas);
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    // Comunicate To QuestPanel_Objective
    public void OpenObjectivePanel_Acceptable(QuestData data)
    {
        objectivePanel.RefreshToAcceptable(data);
    }
    public void OpenObjectivePanel_Complete(QuestData data)
    {
        objectivePanel.RefreshToComplete(data);
    }
}
