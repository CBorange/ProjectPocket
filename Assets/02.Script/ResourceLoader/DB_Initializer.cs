using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB_Initializer : MonoBehaviour
{
    private void Start()
    {
        LoadDBData();
        LoadRuntimeData();
    }
    private void LoadDBData()
    {
        if (!DBConnector.Instance.LoadUserInventory().Equals("Success"))
            Debug.Log("유저인벤토리 로드 에러");
        if (!DBConnector.Instance.LoadUserEquipment(UserInfoProvider.Instance.UserAccount).Equals("Success"))
            Debug.Log("유저인벤토리 로드 에러");
        if (!DBConnector.Instance.LoadItemDB().Equals("Success"))
            Debug.Log("아이템DB 로드 에러");
    }
    private void LoadRuntimeData()
    {
        PlayerStat.Instance.Initialize();
        PlayerInventory.Instance.Initialize();
        PlayerEquipment.Instance.Initialize();
    }
}
