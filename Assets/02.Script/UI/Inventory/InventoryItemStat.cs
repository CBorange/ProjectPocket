using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemStat : MonoBehaviour
{
    // UI
    public Text StatNameText;
    public Text StatAmountText;
    public void Refresh(string StatName, string StatAmount)
    {
        gameObject.SetActive(true);
        StatNameText.text = StatName;
        StatAmountText.text = StatAmount;
    }
}
