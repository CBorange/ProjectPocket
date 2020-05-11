using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniversalNoticePanel : MonoBehaviour
{
    // UI
    public Text TitleText;
    public Text ContentsText;

    public void OpenPanel(string titleTxt, string contentsTxt, float viewTime)
    {
        gameObject.SetActive(true);
        TitleText.text = titleTxt;
        ContentsText.text = contentsTxt;

        Invoke("ClosePanel", viewTime);
    }
    private void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}
