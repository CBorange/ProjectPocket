using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BuildingStatus
{
    public int BuildingCode;
    public int Grade;
}
public class PlayerBuilding : MonoBehaviour, PlayerRuntimeData
{
    #region Singleton
    private static PlayerBuilding instance;
    public static PlayerBuilding Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<PlayerBuilding>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("PlayerBuilding").AddComponent<PlayerBuilding>();
                    instance = newSingleton;
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<PlayerBuilding>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    // Data
    private Dictionary<int, BuildingStatus> buildingStatuses;
    public Dictionary<int, BuildingStatus> BuildingStatuses
    {
        get { return buildingStatuses; }
    }
    public void Initialize()
    {
        buildingStatuses = new Dictionary<int, BuildingStatus>();

        BuildingStatus[] statuses = UserBuildingProvider.Instance.BuildingStatus;
        if (statuses.Length == 0)
            return;
        else
        {
            for (int i = 0; i < statuses.Length; ++i)
                buildingStatuses.Add(statuses[i].BuildingCode, statuses[i]);
            return;
        }
    }
    public BuildingStatus GetBuildingStatus(int buildingCode)
    {
        BuildingStatus foundStatus = null;
        if (buildingStatuses.TryGetValue(buildingCode, out foundStatus))
            return foundStatus;
        else
        {
            Debug.Log($"PlayerDB에 [{buildingCode}] 에 해당하는 빌딩이 존재하지 않음, " +
                $"존재하는(생성 가능한) 모든 건물의 정보가 DBMS에 존재해야 하므로 DB 내부 JSON 확인 필요");
            return null;
        }
    }
}
