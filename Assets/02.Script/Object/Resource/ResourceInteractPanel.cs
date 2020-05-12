using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceInteractPanel : MonoBehaviour
{
    // UI
    public Text ResourceName;
    public Text WorkPointUsageText;

    // Controller
    private ResourceController resourceController;

    // Data
    private ResourceData currentData;

    private void FixedUpdate()
    {
        if (!resourceController.IsPossibleToInteract())
            gameObject.SetActive(false);
    }
    public void Initialize()
    {

    }
    public void OpenPanel(ResourceController controller, Vector3 resourceScreenPos)
    {
        gameObject.SetActive(true);

        transform.position = resourceScreenPos;
        resourceController = controller;
        currentData = resourceController.CurrentData;

        ResourceName.text = currentData.ResourceName;
        WorkPointUsageText.text = $"소모 노동력 : {currentData.WorkPointUsage}";
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    public void StartGathering()
    {
        if (PlayerStat.Instance.GetStat("WorkPoint") - currentData.WorkPointUsage < 0)
            return;
        if (PlayerActManager.Instance.CurrentBehaviour != CharacterBehaviour.Idle)
            return;
        gameObject.SetActive(false);

        PlayerMovementController.Instance.LookTarget(resourceController.gameObject);
        PlayerActManager.Instance.CurrentBehaviour = CharacterBehaviour.Gathering;
        UIPanelTurner.Instance.Open_GatheringProgressPanel(resourceController);
    }
}
