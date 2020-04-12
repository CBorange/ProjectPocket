using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDB
{
    // Singleton
    private MonsterDB()
    {
        monsterDatas = new Dictionary<int, MonsterData>();
    }
    private static MonsterDB instance;
    public static MonsterDB Instance
    {
        get
        {
            if (instance == null)
                instance = new MonsterDB();
            return instance;
        }
    }

    // Data
    private Dictionary<int, MonsterData> monsterDatas;
    public Dictionary<int, MonsterData> MonsterDatas
    {
        get { return monsterDatas; }
    }

    // Method
    public MonsterData GetMonsterData(int monsterCode)
    {
        MonsterData foundJSON = null;
        if (monsterDatas.TryGetValue(monsterCode, out foundJSON))
        {
            return foundJSON;
        }
        else
        {
            foundJSON = DBConnector.Instance.LoadMonsterData(monsterCode);
            if (foundJSON == null)
            {
                Debug.Log($"Monster DB : {monsterCode} 로드 실패");
                return null;
            }
            else
            {
                monsterDatas.Add(monsterCode, foundJSON);
                return foundJSON;
            }
        }
    }

}
