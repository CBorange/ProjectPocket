using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPC_Controller : MonoBehaviour
{
    // Initialize On Editor Data
    public int NPCCode;

    // UI
    public NPC_QuestMark QuestMark;

    // Data
    private NPCData npcData;

    public void Interact()
    {
        Vector3 distance = PlayerActManager.Instance.transform.position - transform.position;
        if (distance.magnitude < 2)
            UIPanelTurner.Instance.Open_NPCDialogPanel(npcData, this);
    }
    public void Initialize()
    {
        LoadNPCData();
        QuestMarkerChangeAccordingToState();
    }
    public void SperateQuestsAccordingToState()
    {
        npcData.SeperateQuestsAccordingToState();
    }
    private void LoadNPCData()
    {
        npcData = NpcDB.Instance.GetNPCData(NPCCode);
    }

    public void QuestMarkerChangeAccordingToState()
    {
        SperateQuestsAccordingToState();

        if (npcData.AcceptableQuests.Count > 0)
        {
            QuestMark.ChageMark_HasAcceptable();
            return;
        }
        if (npcData.CompleteQuests.Count > 0)
        {
            QuestMark.ChangeMark_HasComplete();
            return;
        }
        if (npcData.InProgressQuests.Count > 0)
        {
            QuestMark.ChangeMark_HasInProgress();
            return;
        }
        QuestMark.TurnOffMark();
    }
    
}
