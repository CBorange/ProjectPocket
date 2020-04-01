using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestProgress_KillMonster
{
    public TotalKillMonsterProgress[] TotalProgress;
    public bool GetCompletedAllKillMonster(int questCode)
    {
        TotalKillMonsterProgress currentProgress = null;
        if (TotalProgress.Length == 0)
        {
            Debug.Log($"Discussion 진행상황이 존재하지 않습니다 : {questCode}");
            return false;
        }
        // Search
        for (int i = 0; i < TotalProgress.Length; ++i)
        {
            if (TotalProgress[i].QuestCode == questCode)
                currentProgress = TotalProgress[i];
        }

        if (currentProgress.Completed)
            return true;
        else
            return false;
    }
}
[System.Serializable]
public class TotalKillMonsterProgress
{
    public bool Completed;
    public int QuestCode;
    public KillMonsterProgressInfo[] Progress;
}
[System.Serializable]
public class KillMonsterProgressInfo
{
    public int TargetMonster;
    public int KillCount;
}
