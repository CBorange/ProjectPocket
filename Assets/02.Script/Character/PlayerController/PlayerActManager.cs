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

    public void EquipWeapon(WeaponData weaponData)
    {
        weaponModel = Instantiate(Resources.Load<GameObject>($"Prefab/Weapon/{weaponData.Name}"), WeaponGrapPosition);
        weaponModel.transform.localPosition = weaponData.GrapPoint;
        weaponModel.transform.localRotation = Quaternion.Euler(weaponData.GrapRotation);

        // WeaponType에 따른 Behaviour 클래스 생성
        Assembly creator = Assembly.GetExecutingAssembly();
        object obj = creator.CreateInstance($"WeaponBehaviour_{weaponData.WeaponType}");
        if (obj is IWeaponBehaviour)
            equipedWeaponBehaviour = (IWeaponBehaviour)obj;

        equipedWeaponBehaviour.CreateBehaviour(weaponData);
    }
    public void UnEquipWeapon()
    {
        Destroy(weaponModel);
        equipedWeaponBehaviour = null;
    }
    public void AttackOnEquipmentWeapon()
    {
        if (equipedWeaponBehaviour == null)
            return;
        equipedWeaponBehaviour.PlayAttack();
    }
}
