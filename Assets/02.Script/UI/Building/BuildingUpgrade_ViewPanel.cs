using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUpgrade_ViewPanel : MonoBehaviour
{
    // Controller
    public BuildingUpgradePanel UpgradePanel;

    // UI
    public ToggleGroup GradeToggleGroup;

    // Data
    public GameObject GradeTogglePrefab;
    private BuildingData currentData;
    private List<GradeSelectToggle> togglePool;

    public void Initialize()
    {
        togglePool = new List<GradeSelectToggle>();
         
        CreateGradeTogglePool();
    }
    public void OpenPanel(BuildingData data)
    {
        currentData = data;
        Refresh();
    }
    public void ClosePanel()
    {

    }

    private void CreateGradeTogglePool()
    {
        for (int i = 0; i < 5; ++i)
        {
            GameObject newToggle = Instantiate(GradeTogglePrefab);
            newToggle.transform.SetParent(GradeToggleGroup.transform, false);
            newToggle.GetComponent<Toggle>().group = GradeToggleGroup;

            GradeSelectToggle selectToggle = newToggle.GetComponent<GradeSelectToggle>();
            selectToggle.Initialize(SelectGradeToggle);

            togglePool.Add(selectToggle);
        }
    }
    private void Refresh()
    {
        for (int i = 0; i < 5; ++i)
            togglePool[i].Refresh(currentData, i);
        togglePool[0].GetComponent<Toggle>().isOn = true;

    }
    // Callback
    private void SelectGradeToggle(int grade)
    {
        UpgradePanel.RefreshInteractPanel(grade);
    }
}
