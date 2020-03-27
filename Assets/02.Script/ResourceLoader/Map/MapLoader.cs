using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

public class MapLoader : MonoBehaviour
{
    public LoadingPanel loadingPanel;
    private GameObject mapObject;

    private void Start()
    {
        loadingPanel.OpenPanel();
        loadingPanel.SetLoadingText($"맵 : {UserInfoProvider.Instance.LastMap}");

        StartCoroutine(IE_AsyncLoadMap());
    }
    private async void InstantiateMap()
    {
        GameObject map = Instantiate(mapObject);
        loadingPanel.ClosePanel();
    }
    private IEnumerator IE_AsyncLoadMap()
    {
        var request = Resources.LoadAsync<GameObject>($"Map/Map_{UserInfoProvider.Instance.LastMap}");
        while (!request.isDone)
        {
            yield return request;
        }
        mapObject = request.asset as GameObject;
        InstantiateMap();
    }
}
