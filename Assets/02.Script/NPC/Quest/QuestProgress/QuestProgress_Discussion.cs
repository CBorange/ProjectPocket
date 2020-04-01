using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestProgress_Discussion
{
    public TotalDiscussionProgress[] TotalProgress;
    public bool GetCompletedAllDiscussion(int questCode)
    {
        TotalDiscussionProgress currentProgress = null;
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
public class TotalDiscussionProgress
{
    public int QuestCode;
    public bool Completed;
    public DiscussionProgressInfo[] Progress;
}
[System.Serializable]
public class DiscussionProgressInfo
{
    public int TargetNPC;
    public bool TalkCompleted;
}
