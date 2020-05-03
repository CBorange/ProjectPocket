using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    // Data
    private BuildingData currentData;
    private StatAdditional[] buildingStat;
    private int currentGrade;

    public void RemoveEffect()
    {
        for (int i = 0; i < buildingStat.Length; ++i)
        {
            PlayerStat.Instance.RemoveChangeableStat(buildingStat[i].StatName, currentData.BuildingCode);
        }
    }
    public void Initialize(BuildingData data)
    {
        currentData = data;
        ApplyBuildingEffect();
    }
    private void ApplyBuildingEffect()
    {
        currentGrade = PlayerBuilding.Instance.GetBuildingStatus(currentData.BuildingCode).Grade;
        buildingStat = currentData.StatsByGrade[currentGrade].BuildingStats;

        for (int i = 0; i < buildingStat.Length; ++i)
        {
            PlayerStat.Instance.AddChangeableStat(buildingStat[i].StatName, currentData.BuildingCode, buildingStat[i].StatValue);
        }
    }
    public void StartInteract()
    {
        if (!IsPossibleToInteract())
            return;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        UIPanelTurner.Instance.Open_BuildingInteractPanel(currentData, this, screenPos);
    }
    public bool IsPossibleToInteract()
    {
        Vector3 distanceBetweenPlayer = PlayerActManager.Instance.transform.position - transform.position;
        if (distanceBetweenPlayer.magnitude > 3f)
            return false;
        return true;
    }
}
