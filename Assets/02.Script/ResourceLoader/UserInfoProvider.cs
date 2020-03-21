using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfoProvider
{
    // Singleton
    private UserInfoProvider() { }
    private static UserInfoProvider instance;
    public static UserInfoProvider Instance
    {
        get
        {
            if (instance == null)
                instance = new UserInfoProvider();
            return instance;
        }
    }

    // Account
    private string userAccount;
    public string UserAccount { get { return userAccount; } }

    private string lastMap;
    public string LastMap 
    {
        get { return lastMap; }
        set { lastMap = value; }
    }
    private Vector3 lastPos;
    public Vector3 LastPos
    {
        get { return lastPos; }
        set { lastPos = value; }
    }

    // Stat
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


    public void Initialize(string account, string lastMap, string lastPos, string moveSpeed, string jumpSpeed,
        string healthPoint, string shieldPoint, string attackPoint, string attackSpeed)
    {
        // Account
        userAccount = account;
        this.lastMap = lastMap;

        string[] splitedPos = lastPos.Split(',');
        this.lastPos = new Vector3(float.Parse(splitedPos[0]), float.Parse(splitedPos[1]), float.Parse(splitedPos[2]));

        // Stat
        this.moveSpeed = float.Parse(moveSpeed);
        this.jumpSpeed = float.Parse(jumpSpeed);
        this.healthPoint = float.Parse(healthPoint);
        this.shieldPoint = float.Parse(shieldPoint);
        this.attackPoint = float.Parse(attackPoint);
        this.attackSpeed = float.Parse(attackSpeed);
    }
}
