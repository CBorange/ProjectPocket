using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDB
{
    // Singleton
    private ResourceDB()
    {
        resourceDatas = new Dictionary<int, ResourceData>();
    }
    private static ResourceDB instance;
    public static ResourceDB Instance
    {
        get
        {
            if (instance == null)
                instance = new ResourceDB();
            return instance;
        }
    }

    // Data
    private Dictionary<int, ResourceData> resourceDatas;

    public ResourceData GetResourceData(int resourceCode)
    {
        ResourceData foundData;
        bool foundSuccess = resourceDatas.TryGetValue(resourceCode, out foundData);
        if (!foundSuccess)
        {
            foundData = DBConnector.Instance.LoadResourceData(resourceCode);
            resourceDatas.Add(resourceCode, foundData);
            return foundData;
        }
        else
            return foundData;
    }
}
