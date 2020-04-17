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

    private void Start()
    {
        LoadMap(UserInfoProvider.Instance.LastMap);
    }
    public void LoadMap(string mapName)
    {
        if (loadedMap != null)
            Destroy(loadedMap);

        loadingPanel.OpenPanel();
        loadingPanel.SetLoadingText($"맵 : {mapName}");

        loadedMapName = mapName;
        StartCoroutine(IE_AsyncLoadMap(loadedMapName));
    }
    private IEnumerator IE_AsyncLoadMap(string mapName)
    {
        var request = Resources.LoadAsync<GameObject>($"Map/Map_{mapName}");
        while (!request.isDone)
        {
            yield return request;
        }
        loadedMapPrefab = request.asset as GameObject;
        InstantiateMap();
    }
    private void InstantiateMap()
    {
        try
        {
            loadedMap = Instantiate(loadedMapPrefab).GetComponent<MapController>();
        }
        catch(Exception)
        {
            GameObject firstVillage = Resources.Load<GameObject>($"Map/Map_FirstVillage");
            loadedMap = Instantiate(firstVillage).GetComponent<MapController>();
        }
        loadingPanel.ClosePanel();

        if (UserInfoProvider.Instance.LastMap.Equals(loadedMapName))
            PlayerCoordinator.Instance.SetPlayerPosition(UserInfoProvider.Instance.LastPos);
        else
        {
            PlayerCoordinator.Instance.SetPlayerPosition(loadedMap.PlayerStartPos.position);
        }
    }
}
