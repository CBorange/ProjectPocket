using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    // Data
    private BuildingData currentData;

    public void Initialize(BuildingData data)
    {
        currentData = data;

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
