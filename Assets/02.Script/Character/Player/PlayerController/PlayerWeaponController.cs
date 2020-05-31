using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerWeaponController : MonoBehaviour
{
    // Data
    private IWeaponBehaviour equipedWeaponBehaviour;
    public IWeaponBehaviour EquipedWeaponBehaviour
    {
        get { return equipedWeaponBehaviour; }
    }

    public Transform WeaponGrapPosition;
    public Animator animator;
    private GameObject weaponModel;
    private bool nowAttacking;

    // Method
    public void EquipWeapon(WeaponData weaponData)
    {
        UnEquipWeapon();

        LoadWeaponModel(weaponData);
        weaponModel.transform.localPosition = weaponData.GrapPoint;
        weaponModel.transform.localRotation = Quaternion.Euler(weaponData.GrapRotation);

        // WeaponType에 따른 Behaviour 클래스 생성
        string weaponType = string.Empty;
        if (weaponData.WeaponType.Contains("Tool"))
            weaponType = "OneHand";
        else
            weaponType = weaponData.WeaponType;
        Type behaviourType = Type.GetType($"WeaponBehaviour_{weaponType}");
        if (behaviourType != null)
        {
            weaponModel.AddComponent(behaviourType);
            equipedWeaponBehaviour = weaponModel.GetComponent<IWeaponBehaviour>();
        }
        equipedWeaponBehaviour.CreateBehaviour(weaponData, EndAttack);
    }
    private void LoadWeaponModel(WeaponData data)
    {
        GameObject foundModel = AssetBundleCacher.Instance.LoadAndGetAsset("weapon", data.Name) as GameObject;
        weaponModel = Instantiate(foundModel, WeaponGrapPosition);
    }
    public void UnEquipWeapon()
    {
        if (weaponModel != null)
        {
            equipedWeaponBehaviour.ReleaseBehaviour();
            equipedWeaponBehaviour = null;
            Destroy(weaponModel);
            weaponModel = null;
        }
    }
    public void ExecuteAttack()
    {
        if (nowAttacking)
        {
            return;
        }
        if (PlayerActManager.Instance.CurrentBehaviour != CharacterBehaviour.Idle)
        {
            return;
        }
        if (equipedWeaponBehaviour == null)
        {
            return;
        }
        nowAttacking = true;
        PlayerActManager.Instance.CurrentBehaviour = CharacterBehaviour.Attack;
        equipedWeaponBehaviour.PlayAttack();
    }
    public void EndAttack()
    {
        nowAttacking = false;
        PlayerActManager.Instance.CurrentBehaviour = CharacterBehaviour.Idle;
    }
}
