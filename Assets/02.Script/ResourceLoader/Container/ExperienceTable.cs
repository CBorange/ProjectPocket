using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceTable
{
    // Singleton
    private ExperienceTable()
    {
        levelTable = new Dictionary<int, float>();
    }
    private static ExperienceTable instance;
    public static ExperienceTable Instance
    {
        get
        {
            if (instance == null)
                instance = new ExperienceTable();
            return instance;
        }
    }

    // Data
    private Dictionary<int, float> levelTable;
    public Dictionary<int,float> LevelTable
    {
        get { return levelTable; }
    }

    public void ContainExperienceData(int level, float experience)
    {
        if (levelTable.ContainsKey(level))
        {
            Debug.Log($"{level} 레벨의 Experice Data가 이미 존재합니다 : ExperienceTable");
            return;
        }
        else
            levelTable.Add(level, experience);
    }
    public float GetNeedExperienceByLevel(int level)
    {
        float foundExp = 0f;
        if (levelTable.TryGetValue(level, out foundExp))
            return foundExp;
        else
        {
            Debug.Log($"{level} 레벨의 Experience Data가 ExperienceTable에 존재하지 않음");
            return 0f;
        }
    }
}
