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

    // Data
    private CharacterBehaviour currentBehaviour;
    public CharacterBehaviour CurrentBehaviour
    {
        get { return currentBehaviour; }
        set { currentBehaviour = value; }
    }
    // Controller
    public Animator MyAnimator;
    public PlayerWeaponController WeaponController;

    // Weapon, Attack Method
    public void EquipWeapon(WeaponData weaponData)
    {
        WeaponController.EquipWeapon(weaponData);
    }
    public void UnEquipWeapon()
    {
        WeaponController.UnEquipWeapon();
    }
    public void ExecuteAttack()
    {
        if (currentBehaviour == CharacterBehaviour.Death)
            return;
        if (currentBehaviour == CharacterBehaviour.Gathering)
            return;
        WeaponController.ExecuteAttack();
    }
    public void EndAttack()
    {
        WeaponController.EndAttack();
    }

    // GetDamage Method
    public void GetDamage(float ap)
    {
        PlayerStat.Instance.GetDamage(ap);
        // 공격받은 action 취함
    }
    public void CharacterDeath()
    {
        StartCoroutine(IE_DeathProgress());
    }
    private IEnumerator IE_DeathProgress()
    {
        CurrentBehaviour = CharacterBehaviour.Death;
        MyAnimator.SetTrigger("Death");

        yield return new WaitForSeconds(1.1f);

        int gold = (int)PlayerStat.Instance.GetStat("Gold");
        int loss = gold / 10;
        UIPanelTurner.Instance.Open_PlayerDeathPanel(PlayerStat.Instance.GetStat("CurrentExperience"), loss);

        PlayerStat.Instance.RemoveGold(loss);
        PlayerStat.Instance.ResetExperience();
    }
}
