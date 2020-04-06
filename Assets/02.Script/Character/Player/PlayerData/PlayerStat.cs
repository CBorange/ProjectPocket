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

    // Character Change Status 
    private Dictionary<int, float> apChanged;
    private Dictionary<int, float> spChanged;
    private Dictionary<int, float> hpChanged;
    private Dictionary<int, float> moveSpeedChanged;
    private Dictionary<int, float> jumpSpeedChanged;
    private Dictionary<int, float> attackSpeedChanged;

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
    private int max_workPoint;
    public int Max_WorkPoint
    {
        get { return max_workPoint; }
    }
    private int workPoint;
    public int WorkPoint
    {
        get { return workPoint; }
    }
    private int gold;
    public int Gold
    {
        get { return gold; }
    }
    #endregion Stat

    private Action changedStatusCallback;
    public void Initialize()
    {
        UserInfoProvider userData = UserInfoProvider.Instance;
        origin_MoveSpeed = userData.MoveSpeed;
        origin_JumpSpeed = userData.JumpSpeed;
        origin_MaxHealthPoint = userData.HealthPoint;
        origin_MaxShieldPoint = userData.ShieldPoint;
        origin_AttackPoint = userData.AttackPoint;
        origin_AttackSpeed = userData.AttackSpeed;

        moveSpeed = Origin_MoveSpeed;
        jumpSpeed = Origin_JumpSpeed;
        maxHealthPoint = origin_MaxHealthPoint;
        healthPoint = maxHealthPoint;
        shieldPoint = origin_MaxShieldPoint;
        attackPoint = Origin_AttackPoint;
        attackSpeed = Origin_AttackSpeed;
        levelupExperience = userData.LevelupExperience;
        currentExperience = userData.CurrentExperience;
        level = userData.Level;
        max_workPoint = userData.WorkPoint;
        workPoint = max_workPoint;
        gold = userData.Gold;

        apChanged = new Dictionary<int, float>();
        spChanged = new Dictionary<int, float>();
        hpChanged = new Dictionary<int, float>();
        moveSpeedChanged = new Dictionary<int, float>();
        jumpSpeedChanged = new Dictionary<int, float>();
        attackSpeedChanged = new Dictionary<int, float>();

        changedStatusCallback();
    }
    public void AttachUICallback(Action callback)
    {
        changedStatusCallback = callback;
    }

    // Status Change Method
    public void AddChangeAP(int id, float ap)
    {
        apChanged.Add(id, ap);
        ApplyAPChangeValue();
    }
    public void RemoveChangeAP(int id)
    {
        apChanged.Remove(id);
        ApplyAPChangeValue();
    }
    private void ApplyAPChangeValue()
    {
        float changedValue = 0;
        foreach (var kvp in apChanged)
        {
            changedValue += kvp.Value;
        }
        attackPoint = origin_AttackPoint + changedValue;
        changedStatusCallback();
    }
}
