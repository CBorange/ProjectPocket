﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInitializer : MonoBehaviour
{
    public InventoryPanel inventoryPanel;
    public PlayerStatusPanel statusPanel;
    public Player_QuestPanel player_QuestPanel;
    public NPCDialog_Panel npcDialog_Panel;
    public ShopPanel shopPanel;
    public NPC_QuestPanel npc_QuestPanel;

    private void Start()
    {
        inventoryPanel.Initialize();
        statusPanel.Initialize();
        npcDialog_Panel.Initialize();
        shopPanel.Initialize();
        player_QuestPanel.Initialize();
        npc_QuestPanel.Initialize();
    }
}
