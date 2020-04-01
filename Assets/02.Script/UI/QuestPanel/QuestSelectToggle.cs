using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSelectToggle : MonoBehaviour
{
    // Data
    private QuestData currentQuest;

    public void Initialize(QuestData data)
    {
        currentQuest = data;
    }

    public void QuestSelected(bool selected)
    {
        if (!selected)
            return;
    }
}
