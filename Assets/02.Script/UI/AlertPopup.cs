using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class AlertPopup : MonoBehaviour
{
    private float elapsedTimeSinceOpen;
    private float popupHoldTime;
    public Text AlertText;
    public void OpenPopup(float time)
    {
        elapsedTimeSinceOpen = 0f;
        popupHoldTime = time;
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            StartCoroutine(IE_WaitTurnOff());
        }
    }
    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }
    public void RefreshToBuyItem(string itemName, string amount, string price)
    {
        StringBuilder builder = new StringBuilder();
        builder.Append($"<color=green>{itemName}</color> x <color=orange>{amount}</color>개");
        builder.AppendLine();
        builder.Append($"<color=#FFCC00>{(float.Parse(price) * float.Parse(amount))}</color> 원 으로 구입하였습니다!");
        AlertText.text = builder.ToString();
    }
    public void RefreshToAlert(string alert)
    {
        AlertText.text = alert;
    }
    private IEnumerator IE_WaitTurnOff()
    {
        while (true)
        {
            elapsedTimeSinceOpen += Time.deltaTime;
            yield return null;
            if (elapsedTimeSinceOpen > popupHoldTime)
            {
                gameObject.SetActive(false);
                yield break;
            }
        }
    }
}
