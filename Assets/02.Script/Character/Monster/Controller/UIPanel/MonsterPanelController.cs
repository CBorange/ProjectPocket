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

    public void Initialize()
    {
        WorldUI_Canvas = GameObject.Find("UICanvas_World").transform;
        CreatePanelUI();
    }
    private void CreatePanelUI()
    {
        GameObject newPanel = Instantiate(StatusPanelPrefab, WorldUI_Canvas);
        statusPanelController = newPanel.GetComponent<MonsterStatusPanel>();
        statusPanelController.Initialize(Stat, gameObject);
    }
}
