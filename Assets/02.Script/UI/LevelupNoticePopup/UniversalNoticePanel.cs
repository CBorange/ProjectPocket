using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UniversalNoticePanel : MonoBehaviour
{
    // UI
    public Text TitleText;
    public Text ContentsText;

    // Data
    private float elapsedTime = 0f;
    private float onTime;
    private bool panelIsActive;

    public void OpenPanel(string titleTxt, string contentsTxt, float viewTime)
    {
        elapsedTime = 0;
        onTime = viewTime;
        
        gameObject.SetActive(true);
        TitleText.text = titleTxt;
        ContentsText.text = contentsTxt;

        if (!panelIsActive)
        {
            StartCoroutine(IE_WaitClose());
            panelIsActive = true;
        }
    }
    private void ClosePanel()
    {
        panelIsActive = false;
        gameObject.SetActive(false);
    }

    private IEnumerator IE_WaitClose()
    {
        while (true)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= onTime)
            {
                ClosePanel();
            }
        }
    }
}
