using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonsterData
{
    public int MonsterCode;
    public string MonsterKorName;
    public string MonsterEngName;
    public float MoveSpeed;
    public float JumpSpeed;
    public float HealthPoint;
    public float ShieldPoint;
    public float AttackPoint;
    public float AttackSpeed;
    public float AttackRange;
    public float AttackSpread;
    public float AttackHeight;
    public float Experience;
    public DropItemData[] DropItemDatas;
    public DropGoldData GoldData;
}
[System.Serializable]
public class DropItemData
{
    public int ItemCode;
    public int DropPercentage;
    public int MinDropCount;
    public int MaxDropCount;
}
[System.Serializable]
public class DropGoldData
{
    public int MinDropAmount;
    public int MaxDropAmount;
    public string GoldPrefab;
}
