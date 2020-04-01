using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TargetNPCData
{
    public int NPCCode;
    public string[] ChangedDiscussion;
}
[System.Serializable]
public class QuestBehaviour_Discussion
{
    public int QuestCode;
    public TargetNPCData[] TargetNPC;
}
