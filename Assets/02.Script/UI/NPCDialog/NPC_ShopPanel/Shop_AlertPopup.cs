using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Shop_AlertPopup : MonoBehaviour
{
    public Text AlertText;
    public void OpenPopup()
    {
        gameObject.SetActive(true);
        Invoke("TurnOffPopup", 1f);
    }
    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }
    public void RefreshToBuyItem(string itemName, string amount, string price)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append($"{itemName} x {amount}개");
        builder.AppendLine();
        builder.Append($"<color=#FFCC00>{price}</color> 원 으로 구입하였습니다!");
        AlertText.text = builder.ToString();
    }
    public void RefreshToAlert(string alert)
    {
        AlertText.text = alert;
    }
    private void TurnOffPopup()
    {
        gameObject.SetActive(false);
    }
}
