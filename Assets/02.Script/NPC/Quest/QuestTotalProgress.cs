using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTotalProgress
{
    private bool completed;
    public bool Completed
    {
        get { return completed; }
        set { completed = value; }
    }
    private QuestData originalQuestData;
    public QuestData OriginalQuestData
    {
        get { return originalQuestData; }
        set { originalQuestData = value; }
    }
}
