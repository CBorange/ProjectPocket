using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour, CharacterStat
{
    // Character Origin Stat
    private float origin_MoveSpeed;
    public float Origin_MoveSpeed
    {
        get { return origin_MoveSpeed; }
        set { origin_MoveSpeed = value; }
    }

    private float origin_JumpSpeed;
    public float Origin_JumpSpeed
    {
        get { return origin_JumpSpeed; }
        set { origin_JumpSpeed = value; }
    }

    private float origin_HealthPoint;
    public float Origin_HealthPoint
    {
        get { return origin_HealthPoint; }
        set { origin_HealthPoint = value; }
    }

    private float origin_ShieldPoint;
    public float Origin_ShieldPoint
    {
        get { return origin_ShieldPoint; }
        set { origin_ShieldPoint = value; }
    }

    private float origin_AttackPoint;
    public float Origin_AttackPoint
    {
        get { return origin_AttackPoint; }
        set { origin_AttackPoint = value; }
    }

    private float origin_AttackSpeed;
    public float Origin_AttackSpeed
    {
        get { return origin_AttackSpeed; }
        set { origin_AttackSpeed = value; }
    }
    // Character Current Stat
    private float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    private float jumpSpeed;
    public float JumpSpeed
    {
        get { return jumpSpeed; }
        set { jumpSpeed = value; }
    }

    private float healthPoint;
    public float HealthPoint
    {
        get { return healthPoint; }
        set { healthPoint = value; }
    }

    private float shieldPoint;
    public float ShieldPoint
    {
        get { return shieldPoint; }
        set { shieldPoint = value; }
    }

    private float attackPoint;
    public float AttackPoint
    {
        get { return attackPoint; }
        set { attackPoint = value; }
    }

    private float attackSpeed;
    public float AttackSpeed
    {
        get { return attackSpeed; }
        set { attackSpeed = value; }
    }

    private void Start()
    {
        UserInfoProvider userData = UserInfoProvider.Instance;
        Origin_MoveSpeed = userData.MoveSpeed;
        Origin_JumpSpeed = userData.JumpSpeed;
        Origin_HealthPoint = userData.HealthPoint;
        Origin_ShieldPoint = userData.ShieldPoint;
        Origin_AttackPoint = userData.AttackPoint;
        Origin_AttackSpeed = userData.AttackSpeed;

        MoveSpeed = Origin_MoveSpeed;
        JumpSpeed = Origin_JumpSpeed;
        HealthPoint = Origin_HealthPoint;
        ShieldPoint = Origin_ShieldPoint;
        AttackPoint = Origin_AttackPoint;
        AttackSpeed = Origin_AttackSpeed;
    }
}
