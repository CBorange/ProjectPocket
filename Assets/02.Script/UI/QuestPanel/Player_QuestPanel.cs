using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_QuestPanel : MonoBehaviour
{
    // Child Panels
    public Player_QuestPanel_List listPanel;
    public Player_QuestPanel_Objective objectivePanel;

    public void Initialize()
    {
        listPanel.Initialize();
        objectivePanel.Initialize();
    }
    public void OpenPanel()
    {
        gameObject.SetActive(true);
        listPanel.OpenPanel();
        objectivePanel.OpenPanel();
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
        listPanel.ClosePanel();
        objectivePanel.ClosePanel();
    }

    // List <-> Objective 패널 상호작용 Method
    public void Refresh_ObjectiveToInProgress(int questCode)
    {
        objectivePanel.RefreshToInProgress(questCode);
    }
    public void Refresh_ObjectiveToCompleted(int questCode)
    {
        objectivePanel.RefreshToCompleted(questCode);
    }
}
