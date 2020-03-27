using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour, UIPanel
{
    public Text LoadingText;
    public void Initialize()
    {

    }

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void SetLoadingText(string reason)
    {
        LoadingText.text = $"{reason} 로딩중 입니다.";
    }
}
