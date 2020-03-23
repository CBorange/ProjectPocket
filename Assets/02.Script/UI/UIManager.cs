using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public InventoryPanel inventoryPanel;

    private void Start()
    {
        inventoryPanel.Initialize();
    }
}
