using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestProgress_Discussion
{
    // Data
    public TotalDiscussionProgress[] TotalProgress;
    private Dictionary<int, TotalDiscussionProgress> totalProgressDic;

    // Getter
    public bool GetHasCompletedByQuestCode(int questCode)
    {
        TotalDiscussionProgress totalProgress = null;
        if (totalProgressDic.TryGetValue(questCode, out totalProgress)) 
        {
            if (totalProgress.Completed)
                return true;
            else
                return false;
        }
        else
        {
            Debug.Log($"QuestProgress_Discussion -> TotalDiscussionProgress 탐색용 Dictionary에 {questCode} : 퀘스트가 존재하지 않음");
            return false;
        }
    }
    public DiscussionProgressInfo[] GetDetailedDiscussionProgresses(int questCode)
    {
        TotalDiscussionProgress totalProgress = null;
        if (totalProgressDic.TryGetValue(questCode, out totalProgress))
        {
            return totalProgress.Progress;
        }
        else
        {
            Debug.Log($"QuestProgress_Discussion -> TotalDiscussionProgress 탐색용 Dictionary에 {questCode} : 퀘스트가 존재하지 않음");
            return null;
        }
    }

    // Method
    public void Initiailize()
    {
        totalProgressDic = new Dictionary<int, TotalDiscussionProgress>();
        if (TotalProgress == null)
            return;
        for (int i = 0; i < TotalProgress.Length; ++i)
            totalProgressDic.Add(TotalProgress[i].QuestCode, TotalProgress[i]);
    }
    private void UpdateProgress()
    {
        foreach (var kvp in totalProgressDic)
        {
            DiscussionProgressInfo[] info = kvp.Value.Progress;
            int talkCompleteCount = 0;
            for (int i = 0; i < info.Length; ++i)
            {
                if (info[i].TalkCompleted)
                    talkCompleteCount += 1;
            }
            if (talkCompleteCount == info.Length)
                kvp.Value.Completed = true;
        }
    }

    public string[] GetDiscussionSTR(int npcCode)
    {
        foreach(var kvp in totalProgressDic)
        {
            DiscussionProgressInfo[] progressInfo = kvp.Value.Progress;
            for (int i = 0; i < progressInfo.Length; ++i)
            {
                if (progressInfo[i].TargetNPC == npcCode && !progressInfo[i].TalkCompleted)
                {
                    progressInfo[i].TalkCompleted = true;
                    UpdateProgress();
                    return QuestDB.Instance.GetQuestData(kvp.Value.QuestCode).Behaviour_Discussion.GetChangedDiscussion(npcCode);
                }
            }
        }
        return null;
    }
    public void CompleteQuest(int questCode) 
    {
        if (!totalProgressDic.Remove(questCode))
        {
            Debug.Log($"QuestProgress_Discussion 퀘스트 성공 실패, {questCode}");
        }
    }
    public void StartQuest(int questCode, int[] targetNPCs)
    {
        TotalDiscussionProgress newProgress = new TotalDiscussionProgress();
        newProgress.QuestCode = questCode;
        newProgress.Completed = false;
        newProgress.Progress = new DiscussionProgressInfo[targetNPCs.Length];
        for (int i = 0; i < targetNPCs.Length; ++i)
        {
            newProgress.Progress[i] = new DiscussionProgressInfo();
            newProgress.Progress[i].TargetNPC = targetNPCs[i];
            newProgress.Progress[i].TalkCompleted = false;
        }
        totalProgressDic.Add(questCode, newProgress);
    }
}
// 퀘스트에 해당
[System.Serializable]
public class TotalDiscussionProgress
{
    public int QuestCode;
    public bool Completed;
    public DiscussionProgressInfo[] Progress;
}

// 퀘스트 -> 대화내용(Array)에 해당
[System.Serializable]
public class DiscussionProgressInfo
{
    public int TargetNPC;
    public bool TalkCompleted;
}
