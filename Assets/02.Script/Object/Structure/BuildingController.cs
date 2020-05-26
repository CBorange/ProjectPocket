using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    // Controller
    private BoxCollider myCollider;

    // Data
    private BuildingData currentData;
    private StatAdditional[] buildingStat;
    private float myColliderSize;
    private int currentGrade;

    public void Initialize(BuildingData data)
    {
        currentData = data;
        myCollider = GetComponent<BoxCollider>();
        myColliderSize = ((myCollider.bounds.size.x + myCollider.bounds.size.z) / 2);
    }
    public void StartInteract()
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        UIPanelTurner.Instance.Open_BuildingInteractPanel(currentData, this, screenPos);
    }
    public bool IsPossibleToInteract()
    {
        Vector3 distanceBetweenPlayer = PlayerActManager.Instance.transform.position - transform.position;
        if (distanceBetweenPlayer.magnitude - myColliderSize > 1.5f)
            return false;
        return true;
    }
}
