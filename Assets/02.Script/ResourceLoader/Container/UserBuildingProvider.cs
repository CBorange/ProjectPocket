using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserBuildingProvider
{
    // Singleton
    private UserBuildingProvider() { }
    private static UserBuildingProvider instance;
    public static UserBuildingProvider Instance
    {
        get
        {
            if (instance == null)
                instance = new UserBuildingProvider();
            return instance;
        }
    }

    private BuildingStatus[] buildingStatus;
    public BuildingStatus[] BuildingStatus
    {
        get { return buildingStatus; }
    }
    public void Initialize(BuildingStatus[] statuses)
    {
        buildingStatus = statuses;
    }
}
