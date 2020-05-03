using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class GradeSelectToggle : MonoBehaviour
{
    // Data
    private BuildingData currentData;
    private int currentGrade;
    private Action<int> selectCallback;

    // UI
    public Text Title;
    public Text StatIntroduce;

    public void Initialize(Action<int> callback)
    {
        selectCallback = callback;
    }
    public void Refresh(BuildingData data, int grade)
    {
        currentData = data;
        currentGrade = grade;

        Title.text = $"[{currentData.BuildingName}] <color=#FFA500FF>{currentGrade + 1}</color> 단계";
        StatAdditional[] stats = currentData.StatsByGrade[currentGrade].BuildingStats;
        StringBuilder builder = new StringBuilder();
        if (stats.Length == 0)
        {
            builder.Append("효과 없음");
            StatIntroduce.text = builder.ToString();
            return;
        }
        for (int i = 0; i < stats.Length; ++i)
        {
            builder.Append($"{UIText_Util.Instance.GetKorStatByEng(stats[i].StatName)} + {stats[i].StatValue}");
            if (i < stats.Length - 1)
                builder.AppendLine();
        }
    }
    public void SelectToggle(bool select)
    {
        if (!select)
            return;
        selectCallback(currentGrade);
    }
}
