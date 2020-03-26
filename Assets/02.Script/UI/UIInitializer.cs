using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIInitializer : MonoBehaviour
{
    public InventoryPanel inventoryPanel;
    public PlayerStatusPanel statusPanel;

    private void Start()
    {
        inventoryPanel.Initialize();
        statusPanel.Initialize();
    }
}
