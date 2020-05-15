using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

public class MapLoader : MonoBehaviour
{
    #region Singleton
    private static MapLoader instance;
    public static MapLoader Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<MapLoader>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("MapLoader").AddComponent<MapLoader>();
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
        var objs = FindObjectsOfType<MapLoader>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    public LoadingPanel loadingPanel;
    private GameObject loadedMapPrefab;
    private MapController loadedMap;
    private string loadedMapName;
    private int specificLoadPosIndex = -1;

    private void Start()
    {
        LoadMap(UserInfoProvider.Instance.LastMap, -1);
    }
    public void LoadMap(string mapName, int loadPosIdx)
    {
        if (loadedMap != null)
            Destroy(loadedMap.gameObject);

        loadingPanel.OpenPanel();
        loadingPanel.SetLoadingText($"맵 : {mapName}");

        loadedMapName = mapName;
        specificLoadPosIndex = loadPosIdx;
        StartCoroutine(IE_AsyncLoadMap());
    }
    private IEnumerator IE_AsyncLoadMap()
    {
        var request = Resources.LoadAsync<GameObject>($"Map/Map_{loadedMapName}");
        while (!request.isDone)
        {
            yield return request;
        }
        loadedMapPrefab = request.asset as GameObject;
        InstantiateMap();
    }
    private void InstantiateMap()
    {
        loadingPanel.ClosePanel();
        try
        {
            loadedMap = Instantiate(loadedMapPrefab).GetComponent<MapController>();
        }
        catch(Exception)
        {
            Debug.Log($"{loadedMapName} 맵 Instantiate 실패");
            GameObject firstVillage = Resources.Load<GameObject>($"Map/Map_FirstVillage");
            loadedMap = Instantiate(firstVillage).GetComponent<MapController>();
        }

        if (UserInfoProvider.Instance.LastMap.Equals(loadedMapName))
            PlayerCoordinator.Instance.SetPlayerPosition(UserInfoProvider.Instance.LastPos);
        else
        {
            if (specificLoadPosIndex == -1)
                PlayerCoordinator.Instance.SetPlayerPosition(loadedMap.PlayerStartPos.position);
            else
                PlayerCoordinator.Instance.SetPlayerPosition(loadedMap.SpecificPos[specificLoadPosIndex]);
        }

        UserInfoProvider.Instance.LastMap = loadedMapName;
    }
}
