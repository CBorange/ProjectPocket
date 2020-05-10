using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStat : MonoBehaviour, ICharacterStat, PlayerRuntimeData
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
    // Controller
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

    private float gatheringPower;
    public float GatheringPower
    {
        get { return gatheringPower; }
    }

    // Character Change Status 
    private Dictionary<int, float> attackPoint_StatChange;
    private Dictionary<int, float> sheildPoint_StatChange;
    private Dictionary<int, float> maxHealthPoint_StatChange;
    private Dictionary<int, float> moveSpeed_StatChange;
    private Dictionary<int, float> jumpSpeed_StatChange;
    private Dictionary<int, float> attackSpeed_StatChange;
    private Dictionary<int, float> gatheringPower_StatChange;

    // Character Stat Change Method
    private Dictionary<string, Action<int, float>> addChangeable_StatDic;
    private Dictionary<string, Action<int>> removeChangeable_StatDic;
    private Dictionary<string, Action<float>> addPermanence_StatDic;

    // Player Only Stat
    private float origin_GatheringPower;
    public float Origin_GatheringPower
    {
        get { return origin_GatheringPower; }
    }

    private int origin_MaxWorkPoint;
    public int Origin_MaxWorkPoint
    {
        get { return origin_MaxWorkPoint; }
    }

    private float levelupExperience;
    public float LevelupExperience
    {
        get { return levelupExperience; }
    }
    private float currentExperience;
    public float CurrentExperience
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
        InitPlayerStat();
        InitChangedStatDictionary();
        changedStatusCallback();
    }
    private void InitPlayerStat()
    {
        UserInfoProvider userData = UserInfoProvider.Instance;
        origin_MoveSpeed = userData.MoveSpeed;
        origin_JumpSpeed = userData.JumpSpeed;
        origin_MaxHealthPoint = userData.MaxHealthPoint;
        origin_MaxShieldPoint = userData.ShieldPoint;
        origin_AttackPoint = userData.AttackPoint;
        origin_AttackSpeed = userData.AttackSpeed;
        origin_GatheringPower = userData.GatheringPower;
        origin_MaxWorkPoint = userData.MaxWorkPoint;

        moveSpeed = Origin_MoveSpeed;
        jumpSpeed = Origin_JumpSpeed;
        maxHealthPoint = origin_MaxHealthPoint;
        healthPoint = userData.HealthPoint;
        shieldPoint = origin_MaxShieldPoint;
        attackPoint = Origin_AttackPoint;
        attackSpeed = Origin_AttackSpeed;
        gatheringPower = origin_GatheringPower;
        levelupExperience = userData.LevelupExperience;
        currentExperience = userData.CurrentExperience;
        level = userData.Level;
        max_workPoint = userData.MaxWorkPoint;
        workPoint = userData.WorkPoint;
        gold = userData.Gold;
    }
    private void InitChangedStatDictionary()
    {
        attackPoint_StatChange = new Dictionary<int, float>();
        sheildPoint_StatChange = new Dictionary<int, float>();
        maxHealthPoint_StatChange = new Dictionary<int, float>();
        moveSpeed_StatChange = new Dictionary<int, float>();
        jumpSpeed_StatChange = new Dictionary<int, float>();
        attackSpeed_StatChange = new Dictionary<int, float>();
        gatheringPower_StatChange = new Dictionary<int, float>();

        addChangeable_StatDic = new Dictionary<string, Action<int, float>>();
        addChangeable_StatDic.Add("AttackPoint", AddChangeableAP);
        addChangeable_StatDic.Add("AttackSpeed", AddChangeableAttackSpeed);
        addChangeable_StatDic.Add("GatheringPower", AddChangeableGP);
        addChangeable_StatDic.Add("MaxHealthPoint", AddChangeableMaxHP);

        removeChangeable_StatDic = new Dictionary<string, Action<int>>();
        removeChangeable_StatDic.Add("AttackPoint", RemoveChangeableAP);
        removeChangeable_StatDic.Add("AttackSpeed", RemoveChangeableAttackSpeed);
        removeChangeable_StatDic.Add("GatheringPower", RemoveChangeableGP);
        removeChangeable_StatDic.Add("MaxHealthPoint", RemoveChangeableMaxHP);

        addPermanence_StatDic = new Dictionary<string, Action<float>>();
        addPermanence_StatDic.Add("AttackPoint", AddPermanenceAP);
        addPermanence_StatDic.Add("AttackSpeed", AddPermanenceAttackSpeed);
        addPermanence_StatDic.Add("GatheringPower", AddPermanenceGP);
        addPermanence_StatDic.Add("MaxHealthPoint", AddPermanenceMaxHP);
    }
    public void AttachUICallback(Action callback)
    {
        changedStatusCallback = callback;
    }

    #region Status Change Method

    // Special Stat Callback
    public void GetDamage(float ap)
    {
        float damage = ap - shieldPoint;
        if (damage < 0)
            damage = 1;
        healthPoint -= damage;

        if (healthPoint <= 0)
        {
            healthPoint = 0;
            PlayerActManager.Instance.CharacterDeath();
        }
        changedStatusCallback();
    }
    public void Heal(float amount)
    {
        if (amount + healthPoint > maxHealthPoint)
            healthPoint = maxHealthPoint;
        else
            healthPoint += amount;

        changedStatusCallback();
    }
    public void DoWork(int pointUsage)
    {
        if (workPoint - pointUsage < 0)
            Debug.Log("남은 노동력을 초과하여 사용하여 시도함 : PlayerStat");
        workPoint -= pointUsage;

        changedStatusCallback();
    }
    public void RecoverWorkPoint(int amount)
    {
        if (amount + workPoint > max_workPoint)
            workPoint = max_workPoint;
        else
            workPoint += amount;

        changedStatusCallback();
    }
    public void GainExperience(float amount)
    {
        currentExperience += amount;
        if (currentExperience >= levelupExperience)
        {
            currentExperience -= levelupExperience;
            level += 1;
            levelupExperience = ExperienceTable.Instance.GetNeedExperienceByLevel(level);
            UIPanelTurner.Instance.Open_LevelupNoticePopup(level);
        }
        changedStatusCallback();
    }

    // Getter For Change Method
    public void AddChangeableStat(string statName, int id, float amount)
    {
        Action<int, float> foundMethod = null;
        if (!addChangeable_StatDic.TryGetValue(statName, out foundMethod))
        {
            Debug.Log($"{statName} : 스텟 변화량 추가 Method가 존재하지 않습니다.");
            return;
        }
        foundMethod(id, amount);

    }
    public bool GetExistChangeableStatInDic(string statName, int id)
    {
        switch(statName)
        {
            case "AttackPoint":
                if (attackPoint_StatChange.ContainsKey(id))
                    return true;
                return false;
            case "AttackSpeed":
                if (attackSpeed_StatChange.ContainsKey(id))
                    return true;
                return false;
            case "GatheringPower":
                if (gatheringPower_StatChange.ContainsKey(id))
                    return true;
                return false;
            case "MaxHealthPoint":
                if (maxHealthPoint_StatChange.ContainsKey(id))
                    return true;
                return false;
            default:
                Debug.Log($"{statName} 에 해당하는 ChangeableStat 이 없습니다.");
                return false;
        }
    }
    public void RemoveChangeableStat(string statName, int id)
    {
        Action<int> foundMethod = null;
        if (!removeChangeable_StatDic.TryGetValue(statName, out foundMethod))
        {
            Debug.Log($"{statName} : 스텟 변화량 제거 Method가 존재하지 않습니다.");
            return;
        }
        foundMethod(id);
    }
    public void AddPermanenceStat(string statName, float amount)
    {
        Action<float> foundMethod = null;
        if (!addPermanence_StatDic.TryGetValue(statName, out foundMethod))
        {
            Debug.Log($"{statName} : 스텟 영구 추가 Method가 존재하지 않습니다.");
            return;
        }
        foundMethod(amount);
    }


    // MaxHP Change
    private void AddPermanenceMaxHP(float gp)
    {
        origin_MaxHealthPoint += gp;
        UpdateMaxHealthPoint();
    }
    private void AddChangeableMaxHP(int id, float gp)
    {
        maxHealthPoint_StatChange.Add(id, gp);
        UpdateMaxHealthPoint();
    }
    private void RemoveChangeableMaxHP(int id)
    {
        maxHealthPoint_StatChange.Remove(id);
        UpdateMaxHealthPoint();
    }
    private void UpdateMaxHealthPoint()
    {
        float changedValue = 0;
        foreach (var kvp in maxHealthPoint_StatChange)
        {
            changedValue += kvp.Value;
        }
        maxHealthPoint = origin_MaxHealthPoint + changedValue;
        changedStatusCallback();
    }
    // GP Change
    private void AddPermanenceGP(float gp)
    {
        origin_GatheringPower += gp;
        UpdateGatheringPower();
    }
    private void AddChangeableGP(int id, float gp)
    {
        gatheringPower_StatChange.Add(id, gp);
        UpdateGatheringPower();
    }
    private void RemoveChangeableGP(int id)
    {
        gatheringPower_StatChange.Remove(id);
        UpdateGatheringPower();
    }
    private void UpdateGatheringPower()
    {
        float changedValue = 0;
        foreach (var kvp in gatheringPower_StatChange)
        {
            changedValue += kvp.Value;
        }
        gatheringPower = origin_GatheringPower + changedValue;
        changedStatusCallback();
    }
    // AP Change
    private void AddPermanenceAP(float ap)
    {
        origin_AttackPoint += ap;
        UpdateAttackPoint();
    }
    private void AddChangeableAP(int id, float ap)
    {
        attackPoint_StatChange.Add(id, ap);
        UpdateAttackPoint();
    }
    private void RemoveChangeableAP(int id)
    {
        attackPoint_StatChange.Remove(id);
        UpdateAttackPoint();
    }
    private void UpdateAttackPoint()
    {
        float changedValue = 0;
        foreach (var kvp in attackPoint_StatChange)
        {
            changedValue += kvp.Value;
        }
        attackPoint = origin_AttackPoint + changedValue;
        changedStatusCallback();
    }

    // AttackSpeed Change
    private void AddPermanenceAttackSpeed(float attackSpeed)
    {
        origin_AttackSpeed += attackSpeed;
        UpdateAttackSpeed();
    }
    private void AddChangeableAttackSpeed(int id, float attackSpeed)
    {
        attackSpeed_StatChange.Add(id, attackSpeed);
        UpdateAttackSpeed();
    }
    private void RemoveChangeableAttackSpeed(int id)
    {
        attackSpeed_StatChange.Remove(id);
        UpdateAttackSpeed();
    }
    private void UpdateAttackSpeed()
    {
        float changedValue = 0;
        foreach (var kvp in attackSpeed_StatChange)
        {
            changedValue += kvp.Value;
        }
        attackSpeed = origin_AttackSpeed + changedValue;
        changedStatusCallback();
    }

    // Gold Change
    public void AddGold(int amount)
    {
        gold += amount;
        changedStatusCallback();
    }
    public void RemoveGold(int amount)
    {
        if (gold - amount < 0)
            Debug.Log("소지금이 0원 보다 적음");
        gold -= amount;
        changedStatusCallback();
    }
    #endregion
}
