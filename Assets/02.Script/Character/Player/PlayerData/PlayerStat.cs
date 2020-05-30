using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStat : MonoBehaviour, PlayerRuntimeData
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
    // Special Stat
    private string userAccount;
    public string UserAccount
    {
        get { return userAccount; }
    }
    private string lastMap;
    public string LastMap
    {
        get { return lastMap; }
    }
    private PlayerStatUsage statUsage;
    public PlayerStatUsage StatUsage
    {
        get { return statUsage; }
    }
    private bool firstLogin;
    public bool FirstLogin
    {
        get { return firstLogin; }
        set { firstLogin = value; }
    }

    // Stat Dictionary
    private Dictionary<string, int> integerStatDic;
    private Dictionary<string, float> floatStatDic;

    // Character Change Status 
    private Dictionary<string, Dictionary<int, float>> statChangeDics;

    #endregion Stat

    private Action changedStatusCallback;
    public void Initialize()
    {
        integerStatDic = new Dictionary<string, int>();
        floatStatDic = new Dictionary<string, float>();
        InitPlayerStat();
        InitChangedStatDictionary();
    }
    private void InitPlayerStat()
    {
        UserInfoProvider userData = UserInfoProvider.Instance;
        floatStatDic.Add("Origin_MoveSpeed", userData.MoveSpeed);
        floatStatDic.Add("Origin_JumpSpeed", userData.JumpSpeed);
        floatStatDic.Add("Origin_MaxHealthPoint", userData.MaxHealthPoint);
        floatStatDic.Add("Origin_ShieldPoint", userData.ShieldPoint);
        floatStatDic.Add("Origin_AttackPoint", userData.AttackPoint);
        floatStatDic.Add("Origin_AttackSpeed", userData.AttackSpeed);
        floatStatDic.Add("Origin_GatheringPower", userData.GatheringPower);
        floatStatDic.Add("Origin_MaxWorkPoint", userData.MaxWorkPoint);

        floatStatDic.Add("MoveSpeed", GetStat("Origin_MoveSpeed"));
        floatStatDic.Add("JumpSpeed", GetStat("Origin_JumpSpeed"));
        floatStatDic.Add("MaxHealthPoint", GetStat("Origin_MaxHealthPoint"));
        floatStatDic.Add("HealthPoint", GetStat("MaxHealthPoint"));
        floatStatDic.Add("ShieldPoint", GetStat("Origin_ShieldPoint"));
        floatStatDic.Add("AttackPoint", GetStat("Origin_AttackPoint"));
        floatStatDic.Add("AttackSpeed", GetStat("Origin_AttackSpeed"));
        floatStatDic.Add("GatheringPower", GetStat("Origin_GatheringPower"));
        floatStatDic.Add("LevelupExperience", userData.LevelupExperience);
        floatStatDic.Add("CurrentExperience", userData.CurrentExperience);
        floatStatDic.Add("WorkPoint", userData.WorkPoint);
        floatStatDic.Add("MaxWorkPoint", userData.MaxWorkPoint);

        integerStatDic.Add("Gold", userData.Gold);
        integerStatDic.Add("Level", userData.Level);
        integerStatDic.Add("StatPoint", userData.StatPoint);

        userAccount = userData.UserAccount;
        lastMap = userData.LastMap;
        statUsage = userData.StatUsage;
        firstLogin = userData.FirstLogin;
        statUsage.Initialize();
    }
    private void InitChangedStatDictionary()
    {
        statChangeDics = new Dictionary<string, Dictionary<int, float>>();
        statChangeDics.Add("AttackPoint", new Dictionary<int, float>());
        statChangeDics.Add("AttackSpeed", new Dictionary<int, float>());
        statChangeDics.Add("GatheringPower", new Dictionary<int, float>());
        statChangeDics.Add("MaxHealthPoint", new Dictionary<int, float>());
        statChangeDics.Add("MaxWorkPoint", new Dictionary<int, float>());
        statChangeDics.Add("MoveSpeed", new Dictionary<int, float>());
        statChangeDics.Add("JumpSpeed", new Dictionary<int, float>());
        statChangeDics.Add("ShieldPoint", new Dictionary<int, float>());

    }
    public void AttachUICallback(Action callback)
    {
        changedStatusCallback = callback;
        changedStatusCallback();
    }

    public float GetStat(string statName)
    {
        float foundFloat = 0;
        if (floatStatDic.TryGetValue(statName,out foundFloat))
            return foundFloat;
        int foundInt;
        if (integerStatDic.TryGetValue(statName, out foundInt))
            return foundInt;
        Debug.Log($"PlayerStat Dictionary : {statName}에 해당하는 Value가 존재하지 않음");
        return 0;
    }
    #region Status Change Method

    // Special Stat Callback
    public void GetDamage(float ap)
    {
        if (PlayerActManager.Instance.CurrentBehaviour == CharacterBehaviour.Death)
            return;
        float damage = ap - GetStat("ShieldPoint");
        if (damage < 0)
            damage = 1;
        floatStatDic["HealthPoint"] -= damage;

        if (floatStatDic["HealthPoint"] <= 0)
        {
            floatStatDic["HealthPoint"] = 0;
            PlayerActManager.Instance.CharacterDeath();
        }
        changedStatusCallback();
    }
    public void Heal(float amount)
    {
        if (amount + floatStatDic["HealthPoint"] > floatStatDic["MaxHealthPoint"])
            floatStatDic["HealthPoint"] = floatStatDic["MaxHealthPoint"];
        else
            floatStatDic["HealthPoint"] += amount;

        changedStatusCallback();
    }
    public void DoWork(int pointUsage)
    {
        if (floatStatDic["WorkPoint"] - pointUsage < 0)
            Debug.Log("남은 노동력을 초과하여 사용하여 시도함 : PlayerStat");
        floatStatDic["WorkPoint"] -= pointUsage;

        changedStatusCallback();
    }
    public void RecoverWorkPoint(int amount)
    {
        if (amount + floatStatDic["WorkPoint"] > floatStatDic["MaxWorkPoint"])
            floatStatDic["WorkPoint"] = floatStatDic["MaxWorkPoint"];
        else
            floatStatDic["WorkPoint"] += amount;

        changedStatusCallback();
    }
    // Gold Change
    public void AddGold(int amount)
    {
        integerStatDic["Gold"] += amount;
        changedStatusCallback();
    }
    public void RemoveGold(int amount)
    {
        if (integerStatDic["Gold"] - amount < 0)
        {
            Debug.Log("소지금이 0원 보다 적음");
            integerStatDic["Gold"] = 0;
        }
        else
            integerStatDic["Gold"] -= amount;
        changedStatusCallback();
    }
    public void GainExperience(float amount)
    {
        floatStatDic["CurrentExperience"] += amount;
        if (floatStatDic["CurrentExperience"] >= floatStatDic["LevelupExperience"])
        {
            floatStatDic["CurrentExperience"] -= floatStatDic["LevelupExperience"];
            integerStatDic["Level"] += 1;
            integerStatDic["StatPoint"] += 1;
            floatStatDic["LevelupExperience"] = ExperienceTable.Instance.GetNeedExperienceByLevel(integerStatDic["Level"]);
            UIPanelTurner.Instance.Open_UniveralNoticePanel("레벨업!", $"<color=orange>{integerStatDic["Level"]}</color> 레벨을 달성하였습니다!", 2.0f);
        }
        changedStatusCallback();
    }
    public void ResetExperience()
    {
        floatStatDic["CurrentExperience"] = 0;
        changedStatusCallback();
    }
    public void UseStatPoint(int amount)
    {
        if (integerStatDic["StatPoint"] - amount < 0)
        {
            Debug.Log($"현재 스텟포인트 : {integerStatDic["StatPoint"]}, 사용하려는 포인트 : {amount} 로 사용량이 보유중인 스텟포인트 보다 많음");
            return;
        }
        if (integerStatDic["StatPoint"] > 0)
        {
            integerStatDic["StatPoint"] -= amount;
        }
    }
    #endregion
    // Getter For Change Method
    public void AddChangeableStat(string statName, int id, float amount)
    {
        Dictionary<int, float> foundChangeableStatDic;
        if (statChangeDics.TryGetValue(statName, out foundChangeableStatDic))
        {
            foundChangeableStatDic.Add(id, amount);
            UpdateStat(statName, $"Origin_{statName}");
        }
        else
        {
            Debug.Log($"ChangeableStat Dic에 {statName}에 해당하는 Key 가 없습니다.");
            return;
        }

    }
   
    public void RemoveChangeableStat(string statName, int id)
    {
        Dictionary<int, float> foundChangeableStatDic;
        if (statChangeDics.TryGetValue(statName, out foundChangeableStatDic))
        {
            foundChangeableStatDic.Remove(id);
            UpdateStat(statName, $"Origin_{statName}");
        }
        else
        {
            Debug.Log($"ChangeableStat Dic에 {statName}에 해당하는 Key 가 없습니다.");
            return;
        }
    }
    public void AddPermanenceStat(string statName, float amount)
    {
        floatStatDic[$"Origin_{statName}"] += amount;
        UpdateStat(statName, $"Origin_{statName}");
    }
    private void UpdateStat(string statName, string originStatName)
    {
        Dictionary<int, float> foundChangeableStatDic;
        if (statChangeDics.TryGetValue(statName, out foundChangeableStatDic))
        {
            float changedValue = 0;
            foreach (var kvp in foundChangeableStatDic)
            {
                changedValue += kvp.Value;
            }
            floatStatDic[statName] = GetStat(originStatName) + changedValue;
            changedStatusCallback?.Invoke();
        }
        else
        {
            Debug.Log($"ChangeableStat Dic에 {statName}에 해당하는 Key 가 없습니다.");
            return;
        }
    }
}
