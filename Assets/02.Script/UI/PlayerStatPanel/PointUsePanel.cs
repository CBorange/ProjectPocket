using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointUsePanel : MonoBehaviour
{
    // Controller
    public PlayerStatPanel StatPanel;

    // UI
    public Text UsageText;

    // Data
    public string StatName;
    public float IncreaseStatWhenUse;

    public void Refresh()
    {
        UsageText.text = PlayerStat.Instance.StatUsage.GetStatUsage(StatName).ToString();
    }
    public void UsePoint()
    {
        if (PlayerStat.Instance.GetStat("StatPoint") < 1)
            return;
        PlayerStat.Instance.AddPermanenceStat(StatName, IncreaseStatWhenUse);
        PlayerStat.Instance.UseStatPoint(1);
        PlayerStat.Instance.StatUsage.AddStatUsage(StatName, 1);
        StatPanel.Refresh();
    }
}
