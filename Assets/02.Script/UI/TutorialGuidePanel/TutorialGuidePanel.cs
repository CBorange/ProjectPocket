using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class TutorialGuidePanel : MonoBehaviour
{
    // UI
    public Image TutorialImage;
    public Text TutorialTitle;

    // Data
    public Sprite[] TutorialSprites;
    public string[] TutorialTitleTexts;
    private int currentTutorialIndex;

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        currentTutorialIndex = 0;
        RefreshPanel();
    }
    private void RefreshPanel()
    {
        TutorialImage.sprite = TutorialSprites[currentTutorialIndex];
        TutorialTitle.text = TutorialTitleTexts[currentTutorialIndex];
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void GoNextGuide()
    {
        currentTutorialIndex += 1;
        if (currentTutorialIndex > 4)
            currentTutorialIndex = 4;
        RefreshPanel();
    }
    public void GoPreviousGuide()
    {
        currentTutorialIndex -= 1;
        if (currentTutorialIndex < 0)
            currentTutorialIndex = 0;
        RefreshPanel();
    }
}
