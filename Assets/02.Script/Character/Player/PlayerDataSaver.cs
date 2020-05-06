using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataSaver : MonoBehaviour
{
    public void SavePlayerData()
    {
        UserInfoProvider.Instance.SavePlayerInfo_UpdateServerDB();
    }
}
