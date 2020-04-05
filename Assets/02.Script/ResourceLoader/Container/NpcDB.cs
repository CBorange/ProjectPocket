using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcDB
{
    // Singleton
    private NpcDB()
    {
        npcDatas = new Dictionary<int, NPCData>();
    }
    private static NpcDB instance;
    public static NpcDB Instance
    {
        get
        {
            if (instance == null)
                instance = new NpcDB();
            return instance;
        }
    }

    // Data
    private Dictionary<int, NPCData> npcDatas;

    public NPCData GetNPCData(int npcCode)
    {
        NPCData foundData;
        bool foundSuccess = npcDatas.TryGetValue(npcCode, out foundData);
        if (!foundSuccess)
        {
            foundData = DBConnector.Instance.LoadNPCData(npcCode);
            npcDatas.Add(npcCode, foundData);
            return foundData;
        }
        else
            return foundData;
    }
}
