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
        // Public Data
        if (!DBConnector.Instance.LoadItemDB().Equals("Success"))
            Debug.Log("아이템DB 로드 에러");
        if (!DBConnector.Instance.LoadExperienceTable().Equals("Success"))
            Debug.Log("경험치테이블 로드 에러");

        // UserData
        if (!DBConnector.Instance.LoadUserInventory().Equals("Success"))
            Debug.Log("유저 인벤토리 로드 에러");
        if (!DBConnector.Instance.LoadUserEquipment().Equals("Success"))
            Debug.Log("유저 장비 로드 에러");
        if (!DBConnector.Instance.LoadUserQuests().Equals("Success"))
            Debug.Log("유저 퀘스트 로드 에러");
        if (!DBConnector.Instance.LoadUserBuilding().Equals("Success"))
            Debug.Log("유저 빌딩 정보 로드 에러");
    }
    private void LoadRuntimeData()
    {
        PlayerStat.Instance.Initialize();
        PlayerEquipment.Instance.Initialize();
        PlayerBuilding.Instance.Initialize();
        PlayerQuest.Instance.Initialize();
        PlayerInventory.Instance.Initialize();
    }
}
