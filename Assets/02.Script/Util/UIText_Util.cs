using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIText_Util
{
    // Singleton
    private UIText_Util()
    {
        booleanToQuestStringDic = new Dictionary<bool, string>();
        booleanToQuestStringDic.Add(true, "<color=cyan>완료</color>");
        booleanToQuestStringDic.Add(false, "<color=red>진행중</color>");
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

    public string BooleanToQuestIsDoneSTR(bool state)
    {
        string found = null;
        booleanToQuestStringDic.TryGetValue(state, out found);
        return found;
    }
}
