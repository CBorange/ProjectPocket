﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

[System.Serializable]
public class QuestProgress_KillMonster : QuestUpdater
{
    #region Observer
    private List<QuestObserver> questObservers;
    public void AddObserver(QuestObserver observer)
    {
        questObservers.Add(observer);
    }
    public void DeleteObserver(QuestObserver observer)
    {
        questObservers.Remove(observer);
    }
    #endregion
    
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
        questObservers = new List<QuestObserver>();
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
                if (progressInfos[i].CurrentKillCount >= progressInfos[i].GoalKillCount)
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
            for (int killIdx = 0; killIdx < progressInfos.Length; ++killIdx)
            {
                if (progressInfos[killIdx].TargetMonster == monsterCode)
                {
                    if (progressInfos[killIdx].CurrentKillCount < progressInfos[killIdx].GoalKillCount)
                    {
                        progressInfos[killIdx].CurrentKillCount += 1;

                        for (int i = 0; i < questObservers.Count; ++i)
                            questObservers[i].Update_KillMonster(kvp.Value.QuestCode, monsterCode,
                            progressInfos[killIdx].CurrentKillCount, progressInfos[killIdx].GoalKillCount);
                        UpdateProgress();
                    }
                    break;
                }
            }
        }
    }
    public void SaveProgress()
    {
        TotalProgress = new TotalKillMonsterProgress[totalProgressDic.Count];
        int idx = 0;
        foreach (var kvp in totalProgressDic)
        {
            TotalProgress[idx] = kvp.Value;
            idx += 1;
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
