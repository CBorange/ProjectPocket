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
    public void AddStatUsage(string statName, int amount)
    {
        if (statPointUsageDic.ContainsKey(statName))
        {
            statPointUsageDic[statName] += amount;
        }
        else
        {
            Debug.Log($"{statName}에 해당하는 StatUsage Data가 없습니다.");
            return;
        }
    }
    public int GetStatUsage(string statName)
    {
        int usage = 0;
        if (statPointUsageDic.TryGetValue(statName, out usage))
            return usage;
        else
        {
            Debug.Log($"{statName}에 해당하는 StatUsage Data가 없습니다.");
            return 0;
        }
    }
    public void SaveUsageForServerUpdate()
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
