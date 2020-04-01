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

    }
    public void OpenPanel(QuestData[] questDatas)
    {
        currentQuestDatas = questDatas;
        gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

}
