using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDataSaver : MonoBehaviour
{
    public void SavePlayerData()
    {
        try
        {
            UserInfoProvider.Instance.SavePlayerInfo_UpdateServerDB();
            UserBuildingProvider.Instance.Save_PlayerBuilding_UpdateServerDB();
            UserEquipmentProvider.Instance.Save_PlayerEquipment_UpdateServerDB();
            UserInventoryProvider.Instance.Save_PlayerInventory_UpdateServerDB();
            UserQuickSlotProvider.Instance.Save_PlayerQuickSlot_UpdateServerDB();
            UserQuestProvider.Instance.SavePlayerQuest_UpdateServerDB();
        }
        catch(Exception e)
        {
            Debug.Log($"저장 실패 : {e.ToString()}");
            UIPanelTurner.Instance.Open_UniveralNoticePanel("저장 실패!", "저장을 실패하였습니다.", 2.0f);
            return;
        }
        UIPanelTurner.Instance.Open_UniveralNoticePanel("저장 성공!", "플레이어 정보를 서버에 저장하였습니다!", 2.0f);
    }
}
