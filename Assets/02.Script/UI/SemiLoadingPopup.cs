using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SemiLoadingPopup : MonoBehaviour
{
    // UI
    public Text LoadingText;

    public void OpenPopup(string text)
    {
        LoadingText.text = text;
        gameObject.SetActive(true);
    }
    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }
}
