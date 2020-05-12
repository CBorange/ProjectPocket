using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatPanel : MonoBehaviour
{
    // Controller
    public PlayerStatPanel_StatInfo StatInfoPanel;
    public PlayerStatPanel_UsePoint UsePointPanel;

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        StatInfoPanel.OpenPanel();
        UsePointPanel.OpenPanel();
    }
    public void Refresh()
    {
        StatInfoPanel.Refresh();
        UsePointPanel.Refresh();
    }
    public void ClosePanel()
    {
        StatInfoPanel.ClosePanel();
        UsePointPanel.ClosePanel();
        gameObject.SetActive(false);
    }


}
