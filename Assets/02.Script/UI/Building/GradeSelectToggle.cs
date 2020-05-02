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
        for (int i = 0; i < stats.Length; ++i)
        {
            if (stats[i].StatName.Equals("") && stats[i].StatValue == 0)
                builder.Append("효과 없음");
            else
                builder.Append($"{UIText_Util.Instance.GetKorStatByEng(stats[i].StatName)} + {stats[i].StatValue}");
            if (i < stats.Length - 1)
                builder.AppendLine();
        }
        StatIntroduce.text = builder.ToString();
    }
    public void SelectToggle(bool select)
    {
        if (!select)
            return;
        selectCallback(currentGrade);
    }
}
