using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStatUsage
{
    public PointUsage[] StatPointUsage;
    private Dictionary<string, int> statPointUsageDic;
    public Dictionary<string,int> StatPointUsageDic
    {
        get { return statPointUsageDic; }
    }
    public void Initialize()
    {
        statPointUsageDic = new Dictionary<string, int>();
        for (int i = 0; i < StatPointUsage.Length; ++i)
            statPointUsageDic.Add(StatPointUsage[i].StatName, StatPointUsage[i].UseAmount);
    }
    public void SaveUsage()
    {
        int idx = 0;
        foreach (var kvp in StatPointUsageDic)
        {
            StatPointUsage[idx].StatName = kvp.Key;
            StatPointUsage[idx].UseAmount = kvp.Value;
            idx += 1;
        }
    }
}

[System.Serializable]
public class PointUsage
{
    public string StatName;
    public int UseAmount;
}
