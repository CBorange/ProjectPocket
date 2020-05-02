using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUpgradePanel : MonoBehaviour
{
    // Controller
    public BuildingUpgrade_ViewPanel ViewPanel;
    public BuildingUpgrade_InteractPanel InteractPanel;
    private StructureBuilder currentBuilder;

    // Data
    private BuildingData currentData;

    public void Initialize()
    {
        ViewPanel.Initialize();
        InteractPanel.Initialize();
    }
    public void OpenPanel(BuildingData data, StructureBuilder builder)
    {
        currentData = data;
        currentBuilder = builder;

        InteractPanel.OpenPanel(currentData, currentBuilder);
        ViewPanel.OpenPanel(currentData);
        gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    public void RefreshInteractPanel(int selectGrade)
    {
        InteractPanel.Refresh(selectGrade);
    }
}
