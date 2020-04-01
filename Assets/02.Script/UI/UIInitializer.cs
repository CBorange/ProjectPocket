using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInitializer : MonoBehaviour
{
    public InventoryPanel inventoryPanel;
    public PlayerStatusPanel statusPanel;
    public NPCDialog_Panel npcDialog_Panel;


    private void Start()
    {
        inventoryPanel.Initialize();
        statusPanel.Initialize();
        npcDialog_Panel.Initialize();
    }
}
