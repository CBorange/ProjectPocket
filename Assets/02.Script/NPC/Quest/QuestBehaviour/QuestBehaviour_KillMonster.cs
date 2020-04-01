using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestBehaviour_KillMonster
{
    public int QuestCode;
    public TargetMonsterData[] TargetMonster;
}

[System.Serializable]
public class TargetMonsterData
{
    public int Monstercode;
    public int KillCount;
}
