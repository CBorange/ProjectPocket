using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class QuestNoticeText : MonoBehaviour
{
    // UI
    public Text Contents;

    // Data
    private Action<QuestNoticeText> returnToPoolCallback;

    public void Initialize(Action<QuestNoticeText> returnCallback )
    {
        returnToPoolCallback = returnCallback;
    }

    public void Refresh(string text)
    {
        gameObject.SetActive(true);
        Contents.text = text;

        Invoke("ReleaseText", 3.0f);
    }
    public void ReleaseText()
    {
        if (!gameObject.activeSelf)
            return;
        gameObject.SetActive(false);
        returnToPoolCallback(this);
    }
}
