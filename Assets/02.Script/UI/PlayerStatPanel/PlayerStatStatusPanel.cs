using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatStatusPanel : MonoBehaviour
{
    // UI
    public Text StatAmount;

    // Data
    public string StatName;

    public void Refresh()
    {
        if (StatName.Equals("UserAccount"))
            StatAmount.text = PlayerStat.Instance.UserAccount;
        else if (StatName.Equals("LastMap"))
            StatAmount.text = PlayerStat.Instance.LastMap;
        else
            StatAmount.text = PlayerStat.Instance.GetStat(StatName).ToString();
    }
}
