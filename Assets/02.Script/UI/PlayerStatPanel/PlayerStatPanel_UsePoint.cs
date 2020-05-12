using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatPanel_UsePoint : MonoBehaviour
{
    // Controller
    public PlayerStatPanel MainPanel;
    public PointUsePanel[] PointUsePanels;
    public PlayerStatStatusPanel LeftStatPanel;
    // UI

    public void OpenPanel()
    {
        Refresh();
    }
    public void Refresh()
    {
        for (int i = 0; i < PointUsePanels.Length; ++i)
            PointUsePanels[i].Refresh();
        LeftStatPanel.Refresh();
    }
    public void ClosePanel()
    {

    }
}
