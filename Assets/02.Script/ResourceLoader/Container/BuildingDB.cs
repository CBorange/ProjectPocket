using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingDB
{
    // Singleton
    private BuildingDB()
    {
        buildingDatas = new Dictionary<int, BuildingData>();
    }
    private static BuildingDB instance;
    public static BuildingDB Instance
    {
        get
        {
            if (instance == null)
                instance = new BuildingDB();
            return instance;
        }
    }

    // Data
    private Dictionary<int, BuildingData> buildingDatas;

    public BuildingData GetBuildingData(int buildingCode)
    {
        BuildingData foundData;
        bool foundSuccess = buildingDatas.TryGetValue(buildingCode, out foundData);
        if (!foundSuccess)
        {
            foundData = DBConnector.Instance.LoadBuildingData(buildingCode);
            buildingDatas.Add(buildingCode, foundData);
            return foundData;
        }
        else
            return foundData;
    }
}
