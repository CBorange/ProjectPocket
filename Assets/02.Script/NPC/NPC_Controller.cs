using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPC_Controller : MonoBehaviour
{
    // Controller
    private BoxCollider myCollider;

    // UI
    public Transform PortraitCenter;
    public NPC_QuestMark QuestMark;

    // Data
    public int NPCCode;
    private float myColliderSize;
    private NPCData npcData;

    public void Interact()
    {
        UIPanelTurner.Instance.Open_NPCDialogPanel(npcData, this);
    }
    public void Initialize()
    {
        LoadNPCData();
        QuestMarkerChangeAccordingToState();

        myCollider = GetComponent<BoxCollider>();
        myColliderSize = ((myCollider.bounds.size.x + myCollider.bounds.size.z) / 2);
    }
    public void SperateQuestsAccordingToState()
    {
        npcData.SeperateQuestsAccordingToState();
    }
    private void LoadNPCData()
    {
        npcData = NpcDB.Instance.GetNPCData(NPCCode);
    }
    public bool IsPossibleToInteract()
    {
        Vector3 distanceBetweenPlayer = PlayerActManager.Instance.transform.position - transform.position;
        if (distanceBetweenPlayer.magnitude - myColliderSize > 1.5f)
            return false;
        return true;
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
