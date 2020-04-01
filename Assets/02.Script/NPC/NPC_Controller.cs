using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPC_Controller : MonoBehaviour
{
    public int NPCCode;

    private NPCData npcData;
    private QuestData questData;

    public void Interact()
    {
        UIPanelTurner.Instance.Open_NPCDialogPanel(npcData);
    }
    private void Start()
    {
        LoadNPCData();
    }
    private void LoadNPCData()
    {
        npcData = DBConnector.Instance.LoadNPCData(NPCCode);
    }
}
