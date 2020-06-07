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
        currentBehaviour = CharacterBehaviour.Idle;
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
    public EffectManager MyEffectManager;
    public SoundManager MySoundManager;
    public Animator MyAnimator;
    public PlayerWeaponController WeaponController;

    private void Start()
    {
        MyEffectManager.UseTextEffect();
    }
    // PlayerAct Callback
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
        WeaponController.ExecuteAttack();
    }
    public void EndAttack()
    {
        WeaponController.EndAttack();
    }
    public void GetItem()
    {
        MySoundManager.PlayOneShot("GetItem");
    }
    public void GatheringHit()
    {
        string weaponType = PlayerEquipment.Instance.EquipedWeapon.WeaponType;
        if (weaponType.Contains("Pickaxe"))
        {
            MySoundManager.PlayOneShot("Mining");
        }
        else if (weaponType.Contains("Axe"))
        {
            MySoundManager.PlayOneShot("WoodCutting");
        }
    }
    public void UsePotion()
    {
        MyEffectManager.PlayParticle("Potion");
        MySoundManager.PlayOneShot("Potion");
    }

    // GetDamage Method
    public void GetDamage(float ap)
    {
        MyEffectManager.PlayParticle("GetHit");
        MySoundManager.PlayOneShot("GetHit");
        if (PlayerActManager.Instance.CurrentBehaviour == CharacterBehaviour.Death)
            return;
        MyEffectManager.PlayHitTextEffect(PlayerStat.Instance.GetDamage(ap), Color.yellow);
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
