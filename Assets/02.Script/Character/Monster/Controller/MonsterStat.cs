using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterStat : MonoBehaviour, CharacterStat
{
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

    private float origin_MaxHealthPoint;
    public float Origin_MaxHealthPoint
    {
        get { return origin_MaxHealthPoint; }
    }

    private float origin_MaxShieldPoint;
    public float Origin_MaxShieldPoint
    {
        get { return origin_MaxShieldPoint; }
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

    private float maxHealthPoint;
    public float MaxHealthPoint
    {
        get { return maxHealthPoint; }
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
    #endregion

    // Controller
    public MonsterController Controller;

    // Data
    public int MonsterCode;
    private MonsterData data;
    public MonsterData CurrentData
    {
        get { return data; }
    }
    public Action changedStatusCallback;
    public void Initialize()
    {
        data = MonsterDB.Instance.GetMonsterData(MonsterCode);

        origin_MoveSpeed = data.MoveSpeed;
        origin_AttackSpeed = data.AttackSpeed;
        origin_AttackPoint = data.AttackPoint;
        origin_MaxHealthPoint = data.HealthPoint;
        origin_MaxShieldPoint = data.ShieldPoint;
        origin_JumpSpeed = data.JumpSpeed;

        moveSpeed = origin_MoveSpeed;
        jumpSpeed = origin_JumpSpeed;
        maxHealthPoint = origin_MaxHealthPoint;
        healthPoint = maxHealthPoint;
        shieldPoint = origin_MaxShieldPoint;
        attackPoint = origin_AttackPoint;
        attackSpeed = origin_AttackSpeed;
    }

    // Method
    public void GetDamage(float ap)
    {
        float damage = ap - shieldPoint;
        if (damage < 0)
            damage = 1;
        healthPoint -= damage;
        if (healthPoint <= 0)
        {
            healthPoint = 0;
            Controller.CharacterDeath();
        }
        else
            Controller.GetDamage();
        changedStatusCallback();
    }
}
