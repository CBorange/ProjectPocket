using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingInteractPanel : MonoBehaviour
{
    // UI
    public Text BuildingNameText;

    // Data
    private BuildingData currentData;
    private BuildingController controller;

    private void FixedUpdate()
    {
        if (!controller.IsPossibleToInteract())
        {
            gameObject.SetActive(false);
            return;
        }
    }
    public void Initialize()
    {

    }
    public void OpenPanel(BuildingData data, BuildingController controller, Vector3 screenPos)
    {
        currentData = data;
        this.controller = controller;

        transform.position = screenPos;
        BuildingNameText.text = currentData.BuildingName;
        gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    public void ConfingBuilding()
    {
        ClosePanel();
        UIPanelTurner.Instance.Open_BuildingUpgradePanel(currentData, controller.transform.parent.GetComponent<StructureBuilder>());
    }
}
