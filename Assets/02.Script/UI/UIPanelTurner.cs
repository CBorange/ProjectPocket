using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelTurner : MonoBehaviour
{
    public InventoryPanel InventoryPanel;
    public GameObject PlayerInfoPanel;
    public GameObject SettingPanel;

    public void Open_Setting()
    {
        SettingPanel.SetActive(true);
    }
    public void Open_PlayerStatus()
    {
        PlayerInfoPanel.SetActive(true);
    }
    public void Open_Invetory()
    {
        InventoryPanel.OpenPanel();
    }
}
