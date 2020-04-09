using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestProgress_KillMonster
{
    // Data
    public TotalKillMonsterProgress[] TotalProgress;
    private Dictionary<int, TotalKillMonsterProgress> totalProgressDic;

    // Getter
    public bool GetHasCompletedOnTotalKillMonster(int questCode)
    {
        TotalKillMonsterProgress progress = null;
        bool success = totalProgressDic.TryGetValue(questCode, out progress);
        if (success)
        {
            if (progress.Completed)
                return true;
            else
                return false;
        }
        else
        {
            Debug.Log($"QuestProgress_KillMonster -> TotalKillMonsterProgress 탐색용 Dictionary에 {questCode} : 퀘스트가 존재하지 않음");
            return false;
        }
    }

    public KillMonsterProgressInfo[] GetDetailedKillMonsterProgresses(int questCode)
    {
        TotalKillMonsterProgress totalProgress = null;
        bool success = totalProgressDic.TryGetValue(questCode, out totalProgress);
        if (success)
        {
            return totalProgress.Progress;
        }
        else
        {
            Debug.Log($"QuestProgress_KillMonster -> TotalKillMonsterProgress 탐색용 Dictionary에 {questCode} : 퀘스트가 존재하지 않음");
            return null;
        }
    }

    // Method
    public void Initialize()
    {
        totalProgressDic = new Dictionary<int, TotalKillMonsterProgress>();
        if (TotalProgress == null)
            return;
        for (int i = 0; i < TotalProgress.Length; ++i)
            totalProgressDic.Add(TotalProgress[i].QuestCode, TotalProgress[i]);
    }
    public void CompleteQuest(int questCode)
    {
        if (!totalProgressDic.Remove(questCode))
        {
            Debug.Log($"QuestProgress_Discussion 퀘스트 성공 실패, {questCode}");
        }
    }
    public void StartQuest(int questCode, int[] targetMonsters)
    {
        TotalKillMonsterProgress newProgress = new TotalKillMonsterProgress();
        newProgress.QuestCode = questCode;
        newProgress.Completed = false;
        newProgress.Progress = new KillMonsterProgressInfo[targetMonsters.Length];
        for (int i = 0; i < targetMonsters.Length; ++i)
        {
            newProgress.Progress[i] = new KillMonsterProgressInfo();
            newProgress.Progress[i].TargetMonster = targetMonsters[i];
            newProgress.Progress[i].KillCount = 0;
        }
        totalProgressDic.Add(questCode, newProgress);
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
