using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class PlayerActManager : MonoBehaviour
{
    #region Singleton
    private static PlayerActManager instance;
    public static PlayerActManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<PlayerActManager>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("PlayerActManager").AddComponent<PlayerActManager>();
                    instance = newSingleton;
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<PlayerActManager>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    private IWeaponBehaviour equipedWeaponBehaviour;
    public IWeaponBehaviour EquipedWeaponBehaviour
    {
        get { return equipedWeaponBehaviour; }
    }

    public Transform WeaponGrapPosition;
    public Animator animator;
    private GameObject weaponModel;

    // state
    private bool currentlyAttacking = false;
    public bool CurrentlyAttacking { get; }

    public void EquipWeapon(WeaponData weaponData)
    {
        UnEquipWeapon();

        weaponModel = Instantiate(Resources.Load<GameObject>($"Prefab/Weapon/{weaponData.Name}"), WeaponGrapPosition);
        weaponModel.transform.localPosition = weaponData.GrapPoint;
        weaponModel.transform.localRotation = Quaternion.Euler(weaponData.GrapRotation);

        // WeaponType에 따른 Behaviour 클래스 생성
        Type behaviourType = Type.GetType($"WeaponBehaviour_{weaponData.WeaponType}");
        if (behaviourType != null)
        {
            weaponModel.AddComponent(behaviourType);
            equipedWeaponBehaviour = weaponModel.GetComponent<IWeaponBehaviour>();
        }
        equipedWeaponBehaviour.CreateBehaviour(weaponData, EndAttack);
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
    public void AttackOnEquipmentWeapon()
    {
        if (currentlyAttacking || PlayerMovementController.Instance.CurrentlyMoving || PlayerMovementController.Instance.NowJumped)
            return;
        if (equipedWeaponBehaviour == null)
            return;
        equipedWeaponBehaviour.PlayAttack();
        currentlyAttacking = true;
    }
    private void EndAttack()
    {
        currentlyAttacking = false;
    }
}
