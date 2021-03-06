﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum QuestSelectToggleCategory
{
    Acceptable,
    InProgress,
    Complete,
    None
}
public class QuestSelectToggle : MonoBehaviour
{
    // Data
    private Action<QuestSelectToggle> selectedCallback;

    private QuestData currentQuest;
    public QuestData CurrentQuest
    {
        get { return currentQuest; }
    }
    private QuestSelectToggleCategory toggleCategory;
    public QuestSelectToggleCategory ToggleCategory
    {
        get { return toggleCategory; }
    }

    // UI
    public Text QuestNameText;

    public void Initialize(Action<QuestSelectToggle> selectedCallback, QuestSelectToggleCategory category)
    {
        this.selectedCallback = selectedCallback;
        toggleCategory = category;
    }
    public void Initialize(Action<QuestSelectToggle> selectedCallback)
    {
        this.selectedCallback = selectedCallback;
        toggleCategory = QuestSelectToggleCategory.None;
    }
    public void Refresh(QuestData data)
    {
        gameObject.SetActive(true);

        currentQuest = data;
        QuestNameText.text = currentQuest.QuestName;
    }
    public void Refresh(QuestData data, QuestSelectToggleCategory category)
    {
        gameObject.SetActive(true);

        currentQuest = data;
        toggleCategory = category;
        QuestNameText.text = currentQuest.QuestName;
    }

    public void QuestSelected(bool selected)
    {
        if (!selected)
            return;
        selectedCallback(this);
    }
}
