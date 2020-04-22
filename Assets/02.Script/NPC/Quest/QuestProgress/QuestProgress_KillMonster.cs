using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

[System.Serializable]
public class QuestProgress_KillMonster
{
    // Data
    public TotalKillMonsterProgress[] TotalProgress;
    private Dictionary<int, TotalKillMonsterProgress> totalProgressDic;

    // Getter
    public bool GetHasCompletedByQuestCode(int questCode)
    {
        TotalKillMonsterProgress progress = null;
        if (totalProgressDic.TryGetValue(questCode, out progress))
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
        if (totalProgressDic.TryGetValue(questCode, out totalProgress))
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
    public void StartQuest(int questCode, TargetMonsterData[] targets)
    {
        TotalKillMonsterProgress newProgress = new TotalKillMonsterProgress();
        newProgress.QuestCode = questCode;
        newProgress.Completed = false;
        newProgress.Progress = new KillMonsterProgressInfo[targets.Length];
        for (int i = 0; i < targets.Length; ++i)
        {
            newProgress.Progress[i] = new KillMonsterProgressInfo();
            newProgress.Progress[i].TargetMonster = targets[i].MonsterCode;
            newProgress.Progress[i].CurrentKillCount = 0;
            newProgress.Progress[i].GoalKillCount = targets[i].KillCount;
        }
        totalProgressDic.Add(questCode, newProgress);
    }
    private void UpdateProgress()
    {
        foreach (var kvp in totalProgressDic)
        {
            KillMonsterProgressInfo[] progressInfos = kvp.Value.Progress;
            int huntCompleteCount = 0;
            for (int i = 0; i < progressInfos.Length; ++i)
            {
                if (progressInfos[i].CurrentKillCount >= progressInfos[i].CurrentKillCount)
                    huntCompleteCount += 1;
            }
            if (huntCompleteCount == progressInfos.Length)
                kvp.Value.Completed = true;
        }
    }
    public void KilledMonster(int monsterCode)
    {
        foreach (var kvp in totalProgressDic)
        {
            KillMonsterProgressInfo[] progressInfos = kvp.Value.Progress;
            for (int i = 0; i < progressInfos.Length; ++i)
            {
                if ((progressInfos[i].TargetMonster == monsterCode) &&
                    (progressInfos[i].GoalKillCount >= progressInfos[i].CurrentKillCount))
                {
                    progressInfos[i].CurrentKillCount += 1;
                    UpdateProgress();
                }
            }
        }
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
    public int CurrentKillCount;
    public int GoalKillCount;
}
