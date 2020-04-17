using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;

public class PlayerActManager : MonoBehaviour, IActController
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

    private CharacterBehaviour currentBehaviour;
    public CharacterBehaviour CurrentBehaviour
    {
        get { return currentBehaviour; }
        set { currentBehaviour = value; }
    }

    // Weapon, Attack Method
    public void EquipWeapon(WeaponData weaponData)
    {
        UnEquipWeapon();

        weaponModel = Instantiate(Resources.Load<GameObject>($"Weapon/{weaponData.Name}"), WeaponGrapPosition);
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
    public void ExecuteAttack()
    {
        if (currentBehaviour != CharacterBehaviour.Idle)
            return;
        if (equipedWeaponBehaviour == null)
            return;
        equipedWeaponBehaviour.PlayAttack();
        currentBehaviour = CharacterBehaviour.Attack;
    }
    public void EndAttack()
    {
        currentBehaviour = CharacterBehaviour.Idle;
    }

    // GetDamage Method
    public void GetDamage(float ap)
    {
        PlayerStat.Instance.GetDamage(ap);
        // 공격받은 action 취함
    }
    public void CharacterDeath()
    {
        // 사망 모션, 처리
    }
}
