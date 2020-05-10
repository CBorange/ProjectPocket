using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceController : MonoBehaviour
{
    // Controller
    public ItemDropper dropper;

    // Data
    public int ResourceCode;
    public GameObject livingResource;
    public GameObject deathResource;
    private bool isActivated;
    private ResourceData currentData;
    public ResourceData CurrentData
    {
        get { return currentData; }
    }

    public void Start()
    {
        isActivated = true;
        currentData = ResourceDB.Instance.GetResourceData(ResourceCode);
        dropper.Initialize(transform, currentData.DropItemDatas, null);
        return;
    }
    public void EndGathering()
    {
        PlayerStat.Instance.DoWork(currentData.WorkPointUsage);
        dropper.Death();
        PlayerStat.Instance.GainExperience(currentData.Experience);
        PlayerActManager.Instance.CurrentBehaviour = CharacterBehaviour.Idle;
        isActivated = false;
        livingResource.SetActive(false);
        if (deathResource != null)
            deathResource.SetActive(true);
    }
    public void StartIteractWithResource()
    {
        WeaponData playerTool = PlayerEquipment.Instance.EquipedWeapon;
        if (playerTool == null || !playerTool.WeaponType.Contains("Tool"))
            return;
        if (!playerTool.WeaponType.Contains(currentData.CanGatheringTool))
            return;
        if (!IsPossibleToInteract())
            return;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        UIPanelTurner.Instance.Open_ResourceInteractPanel(this, screenPos);
    }
    public bool IsPossibleToInteract()
    {
        if (!isActivated)
            return false;
        Vector3 distanceBetweenPlayer = PlayerActManager.Instance.transform.position - transform.position;
        if (distanceBetweenPlayer.magnitude > 1.5f)
            return false;
        return true;
    }
}
