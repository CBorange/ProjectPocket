using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelTurner : MonoBehaviour
{
    public InventoryPanel InventoryPanel;
    public GameObject PlayerStatusPanel;
    public GameObject SettingPanel;

    public void Open_Setting()
    {
        SettingPanel.SetActive(true);
    }
    public void Open_PlayerStatus()
    {
        PlayerStatusPanel.SetActive(true);
    }
    public void Open_Invetory()
    {
        InventoryPanel.OpenInventoryPanel();
    }
}
