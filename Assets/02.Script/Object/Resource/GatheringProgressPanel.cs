using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GatheringProgressPanel : MonoBehaviour
{
    // UI
    public Image ResourceImage;
    public Text ResoureName;
    public Slider ProgressSlider;

    // Controller
    private ResourceController resourceController;
    public Animator PlayerAnimator;

    // Data
    private ResourceData currentData;
    private float progressValue;

    public void Initiaialize()
    {

    }
    public void OpenPanel(ResourceController controller)
    {
        gameObject.SetActive(true);
        resourceController = controller;
        currentData = resourceController.CurrentData;

        Refresh();
        StartGathering();
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
    private void Refresh()
    {
        ResoureName.text = currentData.ResourceName;
        ProgressSlider.maxValue = currentData.HealthPoint;
        ProgressSlider.value = 0;
        progressValue = 0;
    }
    private string GetAnimatorParameterByResourceType()
    {
        switch (currentData.ResourceType)
        {
            case "Tree":
                return "WoodCutting";
            case "Ore":
                return "Minning";
            case "Fishing":
                break;
        }
        return string.Empty;
    }
    private void StartGathering()
    {
        PlayerAnimator.SetBool(GetAnimatorParameterByResourceType(), true);
        StartCoroutine(IE_ProgressGathering());
    }
    private IEnumerator IE_ProgressGathering()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            if (PlayerActManager.Instance.CurrentBehaviour == CharacterBehaviour.Death)
            {
                yield break;
            }
            progressValue += PlayerStat.Instance.GetStat("GatheringPower") * Time.deltaTime;
            ProgressSlider.value = progressValue;

            if (progressValue >= currentData.HealthPoint)
            {
                PlayerAnimator.SetBool(GetAnimatorParameterByResourceType(), false);
                resourceController.EndGathering();
                ClosePanel();
                yield break;
            }
        }
    }
}
