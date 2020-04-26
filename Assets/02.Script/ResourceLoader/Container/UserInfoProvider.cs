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
    private float gatheringPower;
    public float GatheringPower
    {
        get { return gatheringPower; }
    }
    private int levelupExperience;
    public int LevelupExperience
    {
        get { return levelupExperience; }
        set { levelupExperience = value; }
    }
    private int currentExperience;
    public int CurrentExperience
    {
        get { return currentExperience; }
        set { currentExperience = value; }
    }
    private int level;
    public int Level
    {
        get { return level; }
        set { level = value; }
    }
    private int workPoint;
    public int WorkPoint
    {
        get { return workPoint; }
        set { workPoint = value; }
    }
    private int gold;
    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    }

    public void Initialize(string account, string lastMap, string lastPos, float moveSpeed, float jumpSpeed,
        float healthPoint, float shieldPoint, float attackPoint, float attackSpeed, float gatheringPower, int levelupExperience, int currentExperience, int level,
        int workPoint, int gold)
    {
        // Account
        userAccount = account;
        this.lastMap = lastMap;

        string[] splitedPos = lastPos.Split(',');
        this.lastPos = new Vector3(float.Parse(splitedPos[0]), float.Parse(splitedPos[1]), float.Parse(splitedPos[2]));

        // Stat
        this.moveSpeed = moveSpeed;
        this.jumpSpeed = jumpSpeed;
        this.healthPoint = healthPoint;
        this.shieldPoint = shieldPoint;
        this.attackPoint = attackPoint;
        this.attackSpeed = attackSpeed;
        this.gatheringPower = gatheringPower;
        this.levelupExperience = levelupExperience;
        this.currentExperience = currentExperience;
        this.level = level;
        this.workPoint = workPoint;
        this.gold = gold;
    }
}
