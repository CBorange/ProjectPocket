using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

public class PlayerDataSaver : MonoBehaviour
{
    // Controller
    public SemiLoadingPopup LoadingPopup;

    public void SaveAndPrintResult()
    {
        SaveAndPrintProcess();
    }
    public string SavePlayerData()
    {
        var task = SaveProcess();
        string result = task.Result;
        return result;
    }
    private async Task<string> SaveProcess()
    {
        string result = await Task<string>.Factory.StartNew(SendDataToServer);
        return result;
    }
    private async void SaveAndPrintProcess()
    {
        LoadingPopup.OpenPopup("플레이어 정보를 저장중입니다...");
        string result = await Task<string>.Factory.StartNew(SendDataToServer);
        if (result.Equals("Success"))
        {
            LoadingPopup.ClosePopup();
            UIPanelTurner.Instance.Open_UniveralNoticePanel("저장 성공!", "플레이어 정보를 서버에 저장하였습니다!", 2.0f);
        }
        else
        {
            LoadingPopup.ClosePopup();
            UIPanelTurner.Instance.Open_UniveralNoticePanel("저장 실패!", $"저장을 실패하였습니다. : {result}", 2.0f);
        }
    }
    private string SendDataToServer()
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
        catch (Exception e)
        {
            return $"Failed : {e.ToString()}";
        }
        return "Success";
    }
}
