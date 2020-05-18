using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIText_Util
{
    // Singleton
    private UIText_Util()
    {
        Init_BooleanToQuestStringDic();
        Init_EngItemTypeToKor();
        Init_EngItemStatToKor();
    }
    private static UIText_Util instance;
    public static UIText_Util Instance
    {
        get
        {
            if (instance == null)
                instance = new UIText_Util();
            return instance;
        }
    }

    // Data
    private Dictionary<bool, string> booleanToQuestStringDic;
    private Dictionary<string, string> engItemTypeToKor;
    private Dictionary<string, string> engItemStatToKor;

    private void Init_BooleanToQuestStringDic()
    {
        booleanToQuestStringDic = new Dictionary<bool, string>();
        booleanToQuestStringDic.Add(true, "<color=cyan>완료</color>");
        booleanToQuestStringDic.Add(false, "<color=red>진행중</color>");
    }
    private void Init_EngItemTypeToKor()
    {
        engItemTypeToKor = new Dictionary<string, string>();
        engItemTypeToKor.Add("Weapon", "무기");
        engItemTypeToKor.Add("Expendable", "소모품");
        engItemTypeToKor.Add("Accesorie", "장신구");
        engItemTypeToKor.Add("Etc", "기타");
    }
    private void Init_EngItemStatToKor()
    {
        engItemStatToKor = new Dictionary<string, string>();
        engItemStatToKor.Add("AttackPoint", "공격력");
        engItemStatToKor.Add("AttackSpeed", "공격속도");
        engItemStatToKor.Add("MoveSpeed", "이동속도");
        engItemStatToKor.Add("MaxHealthPoint", "최대 체력");
        engItemStatToKor.Add("MaxWorkPoint", "최대 노동력");
        engItemStatToKor.Add("HealthPoint", "체력 회복");
        engItemStatToKor.Add("ShieldPoint", "방어력");
        engItemStatToKor.Add("GatheringPower", "채집 능력");
    }

    public string BooleanToQuestIsDoneSTR(bool state)
    {
        string found = null;
        booleanToQuestStringDic.TryGetValue(state, out found);
        return found;
    }
    public string GetKorItemTypeByEng(string type)
    {
        string found = null;
        engItemTypeToKor.TryGetValue(type, out found);
        return found;
    }
    public string GetKorStatByEng(string stat)
    {
        string found = null;
        if (!engItemStatToKor.TryGetValue(stat, out found))
            return stat;
        return found;
    }
}
