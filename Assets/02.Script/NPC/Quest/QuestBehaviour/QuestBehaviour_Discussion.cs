using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetNPCData
{
    public int NPCCode;
    public string NPCName;
    public string[] ChangedDiscussion;
}
[System.Serializable]
public class QuestBehaviour_Discussion
{
    public int QuestCode;
    public TargetNPCData[] TargetNPC;
    public string[] GetChangedDiscussion(int npcCode)
    {
        for (int targetIdx = 0; targetIdx < TargetNPC.Length; ++targetIdx)
        {
            if (TargetNPC[targetIdx].NPCCode == npcCode)
            {
                return TargetNPC[targetIdx].ChangedDiscussion;
            }
        }
        return null;
    }
}
