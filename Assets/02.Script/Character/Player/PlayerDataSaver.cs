using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataSaver : MonoBehaviour
{
    public void SavePlayerData()
    {
        UserInfoProvider.Instance.SavePlayerInfo_UpdateServerDB();
        UserBuildingProvider.Instance.Save_PlayerBuilding_UpdateServerDB();
        UserEquipmentProvider.Instance.Save_PlayerEquipment_UpdateServerDB();
        UserInventoryProvider.Instance.Save_PlayerInventory_UpdateServerDB();
        UserQuestProvider.Instance.SavePlayerQuest_UpdateServerDB();
    }
}
