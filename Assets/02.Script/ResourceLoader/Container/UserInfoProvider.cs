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
    private float maxHealthPoint;
    public float MaxHealthPoint
    {
        get { return maxHealthPoint; }
        set { maxHealthPoint = value; }
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
    private float levelupExperience;
    public float LevelupExperience
    {
        get { return levelupExperience; }
        set { levelupExperience = value; }
    }
    private float currentExperience;
    public float CurrentExperience
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
    private float workPoint;
    public float WorkPoint
    {
        get { return workPoint; }
        set { workPoint = value; }
    }
    private float maxWorkPoint;
    public float MaxWorkPoint
    {
        get { return maxWorkPoint; }
        set { maxWorkPoint = value; }
    }
    private int gold;
    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    }
    private int statPoint;
    public int StatPoint
    {
        get { return statPoint; }
    }
    private PlayerStatUsage statUsage;
    public PlayerStatUsage StatUsage
    {
        get { return statUsage; }
    }

    public void Initialize(string account, string lastMap, string lastPos, float moveSpeed, float jumpSpeed,
        float healthPoint, float maxHealthPoint, float shieldPoint, float attackPoint, float attackSpeed, float gatheringPower, float levelupExperience, float currentExperience, int level,
        float workPoint, float maxWorkPoint, int gold, int statPoint, PlayerStatUsage statUsage)
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
        this.maxHealthPoint = maxHealthPoint;
        this.shieldPoint = shieldPoint;
        this.attackPoint = attackPoint;
        this.attackSpeed = attackSpeed;
        this.gatheringPower = gatheringPower;
        this.levelupExperience = levelupExperience;
        this.currentExperience = currentExperience;
        this.level = level;
        this.workPoint = workPoint;
        this.maxWorkPoint = maxWorkPoint;
        this.gold = gold;
        this.statPoint = statPoint;
        this.statUsage = statUsage;
    }
    public void SavePlayerInfo_UpdateServerDB()
    {
        this.lastPos = PlayerActManager.Instance.transform.position;

        this.moveSpeed = PlayerStat.Instance.Origin_MoveSpeed;
        this.jumpSpeed = PlayerStat.Instance.Origin_JumpSpeed;

        float saveHealthPoint = 0;
        if (PlayerStat.Instance.HealthPoint >= PlayerStat.Instance.MaxHealthPoint)
            saveHealthPoint = PlayerStat.Instance.MaxHealthPoint;
        else
            saveHealthPoint = PlayerStat.Instance.HealthPoint;
        this.healthPoint = saveHealthPoint;
        this.maxHealthPoint = PlayerStat.Instance.Origin_MaxHealthPoint;
        this.shieldPoint = PlayerStat.Instance.Origin_ShieldPoint;
        this.attackPoint = PlayerStat.Instance.Origin_AttackPoint;
        this.attackSpeed = PlayerStat.Instance.Origin_AttackSpeed;
        this.gatheringPower = PlayerStat.Instance.Origin_GatheringPower;
        this.levelupExperience = PlayerStat.Instance.LevelupExperience;
        this.currentExperience = PlayerStat.Instance.CurrentExperience;
        this.level = PlayerStat.Instance.Level;

        float saveWorkPoint = 0;
        if (PlayerStat.Instance.WorkPoint >= PlayerStat.Instance.Max_WorkPoint)
            saveWorkPoint = PlayerStat.Instance.Max_WorkPoint;
        else
            saveWorkPoint = PlayerStat.Instance.WorkPoint;
        this.workPoint = saveWorkPoint;
        this.maxWorkPoint = PlayerStat.Instance.Origin_MaxWorkPoint;
        this.gold = PlayerStat.Instance.Gold;
        this.statPoint = PlayerStat.Instance.StatPoint;
        this.statUsage = PlayerStat.Instance.StatUsage;
        PlayerStat.Instance.StatUsage.SaveUsage();

        DBConnector.Instance.Save_PlayerStat();
    }
}
