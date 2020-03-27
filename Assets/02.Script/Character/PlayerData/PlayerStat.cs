using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStat : MonoBehaviour, CharacterStat, PlayerRuntimeData
{
    #region Singleton
    private static PlayerStat instance;
    public static PlayerStat Instance
    {
        get 
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<PlayerStat>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("PlayerStat").AddComponent<PlayerStat>();
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
        var objs = FindObjectsOfType<PlayerStat>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    #region Stat
    // Character Origin Stat
    private float origin_MoveSpeed;
    public float Origin_MoveSpeed
    {
        get { return origin_MoveSpeed; }
    }

    private float origin_JumpSpeed;
    public float Origin_JumpSpeed
    {
        get { return origin_JumpSpeed; }
    }

    private float max_HealthPoint;
    public float Max_HealthPoint
    {
        get { return max_HealthPoint; }
    }

    private float origin_ShieldPoint;
    public float Origin_ShieldPoint
    {
        get { return origin_ShieldPoint; }
    }

    private float origin_AttackPoint;
    public float Origin_AttackPoint
    {
        get { return origin_AttackPoint; }
    }

    private float origin_AttackSpeed;
    public float Origin_AttackSpeed
    {
        get { return origin_AttackSpeed; }
    }

    // Character Current Stat
    private float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
    }

    private float jumpSpeed;
    public float JumpSpeed
    {
        get { return jumpSpeed; }
    }

    private float healthPoint;
    public float HealthPoint
    {
        get { return healthPoint; }
    }

    private float shieldPoint;
    public float ShieldPoint
    {
        get { return shieldPoint; }
    }

    private float attackPoint;
    public float AttackPoint
    {
        get { return attackPoint; }
    }

    private float attackSpeed;
    public float AttackSpeed
    {
        get { return attackSpeed; }
    }

    // Player Only Stat
    private int levelupExperience;
    public int LevelupExperience
    {
        get { return levelupExperience; }
    }
    private int currentExperience;
    public int CurrentExperience
    {
        get { return currentExperience; }
    }
    private int level;
    public int Level
    {
        get { return level; }
    }
    #endregion Stat

    private Action changedStatusCallback;
    public void Initialize()
    {
        UserInfoProvider userData = UserInfoProvider.Instance;
        origin_MoveSpeed = userData.MoveSpeed;
        origin_JumpSpeed = userData.JumpSpeed;
        max_HealthPoint = userData.HealthPoint;
        origin_ShieldPoint = userData.ShieldPoint;
        origin_AttackPoint = userData.AttackPoint;
        origin_AttackSpeed = userData.AttackSpeed;

        moveSpeed = Origin_MoveSpeed;
        jumpSpeed = Origin_JumpSpeed;
        healthPoint = Max_HealthPoint;
        shieldPoint = Origin_ShieldPoint;
        attackPoint = Origin_AttackPoint;
        attackSpeed = Origin_AttackSpeed;
        levelupExperience = userData.LevelupExperience;
        currentExperience = userData.CurrentExperience;
        level = userData.Level;
    }
    public void AttachUICallback(Action callback)
    {
        changedStatusCallback = callback;
    }
    public void EquipWeapon(float plusAP)
    {
        attackPoint += plusAP;
        changedStatusCallback();
    }
    public void UnEquipWeapon(float minusAP)
    {
        attackPoint -= minusAP;
        changedStatusCallback();
    }
}
