using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatPanel_StatInfo : MonoBehaviour
{
    // Controller
    public PlayerStatPanel MainPanel;
    public PlayerStatStatusPanel[] StatusPanels;

    public void OpenPanel()
    {
        Refresh();
    }
    public void Refresh()
    {
        for (int i = 0; i < StatusPanels.Length; ++i)
            StatusPanels[i].Refresh();
    }
    public void ClosePanel()
    {

    }
}
