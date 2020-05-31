using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;

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
    private bool recoveryStatAfterLoad;
    private string loadedMapName;
    private int specificLoadPosIndex = -1;

    private void Start()
    {
        LoadMap(UserInfoProvider.Instance.LastMap, -1, false);
    }
    public void LoadMap(string mapName, int loadPosIdx, bool recoveryStat)
    {
        if (loadedMap != null)
            Destroy(loadedMap.gameObject);

        loadingPanel.OpenPanel();
        loadingPanel.SetLoadingText_MapLoading(mapName);

        loadedMapName = mapName;
        specificLoadPosIndex = loadPosIdx;
        recoveryStatAfterLoad = recoveryStat;

        StartCoroutine(IE_LoadAsset());
    }
    private IEnumerator IE_LoadAsset()
    {
        AssetBundle bundle = null;
        string bundleName = "map";
        if (AssetBundleCacher.Instance.HasAleadyCachingBundle(bundleName))
            bundle = AssetBundleCacher.Instance.GetBundle(bundleName);
        else
        {
            var request = AssetBundle.LoadFromFileAsync($"{Application.streamingAssetsPath}/AssetBundles/{bundleName}");
            yield return request;
            bundle = request.assetBundle;
            AssetBundleCacher.Instance.CachingBundle(bundle, bundleName);
        }
        var assetRequest = bundle.LoadAssetAsync<GameObject>($"Map_{loadedMapName}");
        yield return assetRequest;

        loadedMapPrefab = assetRequest.asset as GameObject;
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
            return;
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

        if (recoveryStatAfterLoad)
            PlayerStat.Instance.Heal(99999);
        UserInfoProvider.Instance.LastMap = loadedMapName;
    }
}
