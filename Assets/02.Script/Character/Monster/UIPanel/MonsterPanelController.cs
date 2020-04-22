using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterPanelController : MonoBehaviour
{
    // UI
    private Transform WorldUI_Canvas;

    // Controller
    public MonsterStat Stat;

    // Data
    public GameObject StatusPanelPrefab;
    private MonsterStatusPanel statusPanelController;

    private void OnDestroy()
    {
        // 몬스터 UI는 Map Object의 하위가 아닌 WorldUI_Canvas 하위기 때문에 Destroy 호출
        Destroy(statusPanelController.gameObject);
    }
    public void Initialize()
    {
        WorldUI_Canvas = GameObject.Find("UICanvas_World").transform;
        CreatePanelUI();
    }
    public void Respawn()
    {
        statusPanelController.Respawn();
    }
    public void Death()
    {
        statusPanelController.Death();
    }
    private void CreatePanelUI()
    {
        GameObject newPanel = Instantiate(StatusPanelPrefab, WorldUI_Canvas);
        statusPanelController = newPanel.GetComponent<MonsterStatusPanel>();
        statusPanelController.Initialize(Stat, gameObject);
    }
}
