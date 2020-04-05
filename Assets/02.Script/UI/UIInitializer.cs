using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInitializer : MonoBehaviour
{
    public InventoryPanel inventoryPanel;
    public PlayerStatusPanel statusPanel;
    public NPCDialog_Panel npcDialog_Panel;
    public Player_QuestPanel player_QuestPanel;
    public NPC_QuestPanel npc_QuestPanel;

    private void Start()
    {
        inventoryPanel.Initialize();
        statusPanel.Initialize();
        npcDialog_Panel.Initialize();
        player_QuestPanel.Initialize();
        npc_QuestPanel.Initialize();
    }
}
