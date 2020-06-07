using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerWeaponController : MonoBehaviour
{
    // Data
    public Transform WeaponGrapTarget;
    private WeaponData weaponData;
    private GameObject weaponModel;

    // Attack Strategy
    private AttackStrategy currentAttackStrategy;

    // Method
    public void EquipWeapon(WeaponData weaponData)
    {
        UnEquipWeapon();

        this.weaponData = weaponData;
        LoadWeaponModel(weaponData);
        weaponModel.transform.localPosition = weaponData.GrapPoint;
        weaponModel.transform.localRotation = Quaternion.Euler(weaponData.GrapRotation);
    }
    private void LoadWeaponModel(WeaponData data)
    {
        GameObject foundModel = AssetBundleCacher.Instance.LoadAndGetAsset("weapon", data.Name) as GameObject;
        weaponModel = Instantiate(foundModel, WeaponGrapTarget);

        currentAttackStrategy = weaponModel.GetComponent<AttackStrategy>();
        currentAttackStrategy.Initialize(weaponData, EndAttack);
    }
    public void UnEquipWeapon()
    {
        if (weaponModel != null)
        {
            currentAttackStrategy.ReleaseAttackStrategy();
            currentAttackStrategy = null;
            Destroy(weaponModel);
            weaponModel = null;
        }
    }
    public void ExecuteAttack()
    {
        if (PlayerActManager.Instance.CurrentBehaviour == CharacterBehaviour.Idle ||
            PlayerActManager.Instance.CurrentBehaviour == CharacterBehaviour.Move)
        {
            if (currentAttackStrategy == null)
                return;
            PlayerActManager.Instance.CurrentBehaviour = CharacterBehaviour.Attack;
            currentAttackStrategy.PlayAttack();
        }
    }
    public void EndAttack()
    {
        PlayerActManager.Instance.CurrentBehaviour = CharacterBehaviour.Idle;
    }
}
