﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;
using System.Data.SqlClient;

public class DBConnector : MonoBehaviour
{
    #region Singleton
    private static DBConnector instance;
    public static DBConnector Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<DBConnector>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("DBConnector").AddComponent<DBConnector>();
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
        var objs = FindObjectsOfType<DBConnector>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }
    #endregion

    private SqlCommand sqlCommand;
    private SqlConnection sqlConnection;

    private DataSet ConnectToDB(string dbName, string sql)
    {
        string conncStr = $"Server=192.168.0.5; Database={dbName}; uid=sa; pwd=4376";

        try
        {
            using (SqlConnection conn = new SqlConnection(conncStr))
            {
                DataSet dataSet = new DataSet();
                conn.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conncStr);
                adapter.Fill(dataSet);
                return dataSet;
            }
        }
        catch(Exception e)
        {
            return null;
        }
    }

    #region Load User DB Data
    public string ValiadeAccountOnDB(string id, string password)
    {
        string query = $"SELECT * FROM dbo.UserAccount WHERE Account_ID = '{id}' AND (Account_Password = '{password}')";
        DataSet dataSet = ConnectToDB("Account_DB", query);

        if (dataSet == null)
            return "서버에 연결할 수 없습니다.";

        string result = string.Empty;
        if (dataSet.Tables.Count > 0)
        {
            if (dataSet.Tables[0].Rows.Count > 0)
                return "Success";
            else
                return "계정을 찾을 수 없습니다.";
        }
        else
            return "서버에서 데이터를 찾을 수 없습니다.";
    }
    public string LoadUserInfo(string accountID)
    {
        string query = $"SELECT * FROM dbo.PlayerStatus WHERE UserAccount = '{accountID}'";
        DataSet dataSet = ConnectToDB("PlayerInfo_DB", query);

        if (dataSet == null)
            return "서버에 연결할 수 없습니다.";

        object[] dataArray = dataSet.Tables[0].Rows[0].ItemArray;
        UserInfoProvider.Instance.Initialize(dataArray[0].ToString().Trim(),
                                     dataArray[1].ToString().Trim(),
                                     dataArray[2].ToString().Trim(),
                                     dataArray[3].ToString().Trim(),
                                     dataArray[4].ToString().Trim(),
                                     dataArray[5].ToString().Trim(),
                                     dataArray[6].ToString().Trim(),
                                     dataArray[7].ToString().Trim(),
                                     dataArray[8].ToString().Trim(),
                                     (int)dataArray[9],
                                     (int)dataArray[10],
                                     (int)dataArray[11],
                                     (int)dataArray[12],
                                     (int)dataArray[13]);
        return "Success";
    }

    public string LoadUserInventory()
    {
        string query = $"SELECT * FROM dbo.PlayerInventory WHERE UserAccount = '{UserInfoProvider.Instance.UserAccount}'";
        DataSet dataSet = ConnectToDB("PlayerInfo_DB", query);

        if (dataSet == null)
            return "서버에 연결할 수 없습니다.";

        object[] dataArray = dataSet.Tables[0].Rows[0].ItemArray;
        string inventoryJSON = dataArray[1].ToString();

        InventoryJSON jsonObject = JsonUtility.FromJson<InventoryJSON>(inventoryJSON);
        ImpliedItemData[] impliedItemDatas = jsonObject.impliedItemDatas;
        if (impliedItemDatas.Length == 0)
            UserInventoryProvider.Instance.Initialize(null);
        else
            UserInventoryProvider.Instance.Initialize(impliedItemDatas);
        return "Success";
    }
    public string LoadUserEquipment()
    {
        string query = $"SELECT * FROM dbo.PlayerEquipments WHERE UserAccount = '{UserInfoProvider.Instance.UserAccount}'";
        DataSet dataSet = ConnectToDB("PlayerInfo_DB", query);

        if (dataSet == null)
            return "서버에 연결할 수 없습니다.";

        if (dataSet.Tables[0].Rows.Count == 0)
        {
            UserEquipmentProvider.Instance.Initialize(null, null, null);
            return "Success";
        }
        object[] itemArray = dataSet.Tables[0].Rows[0].ItemArray;
        ImpliedItemData[] impliedItemDatas = new ImpliedItemData[itemArray.Length];

        impliedItemDatas[0] = new ImpliedItemData("Weapon", (int)itemArray[1]);
        impliedItemDatas[1] = new ImpliedItemData("Accesorie", (int)itemArray[2]);
        impliedItemDatas[2] = new ImpliedItemData("Accesorie", (int)itemArray[3]);

        UserEquipmentProvider.Instance.Initialize(impliedItemDatas[0], impliedItemDatas[1], impliedItemDatas[2]);
        return "Success";
    }
    public string LoadUserQuests()
    {
        string query = $"SELECT * FROM dbo.PlayerQuests WHERE UserAccount = '{UserInfoProvider.Instance.UserAccount}'";
        DataSet dataSet = ConnectToDB("PlayerInfo_DB", query);

        object[] dataArray = dataSet.Tables[0].Rows[0].ItemArray;

        string questCodesInProgressSTR = dataArray[1].ToString();
        string questInProgress_Discussion_JSON = dataArray[2].ToString();
        string questInProgress_KillMonster_JSON = dataArray[3].ToString();
        string completedQuests = dataArray[4].ToString();

        QuestProgress_Discussion progress_Discussion;
        QuestProgress_KillMonster progress_KillMonster;
        if (!questCodesInProgressSTR.Equals("None"))
        {
            int[] progressQuestCodes = CodesSTR_To_IntegerArray(questCodesInProgressSTR);

            // Discussion
            if (questInProgress_Discussion_JSON.Equals("None"))
                progress_Discussion = new QuestProgress_Discussion();
            else
                progress_Discussion = JsonUtility.FromJson<QuestProgress_Discussion>(questInProgress_Discussion_JSON);
            // KillMonster
            if (questInProgress_KillMonster_JSON.Equals("None"))
                progress_KillMonster = new QuestProgress_KillMonster();
            else
                progress_KillMonster = JsonUtility.FromJson<QuestProgress_KillMonster>(questInProgress_KillMonster_JSON);

            UserQuestProvider.Instance.Initialize_ProgressQuest(progressQuestCodes, progress_Discussion, progress_KillMonster);
        }
        if (!completedQuests.Equals("None"))
        {
            int[] completedQuestCodes = CodesSTR_To_IntegerArray(completedQuests);
            UserQuestProvider.Instance.Initialize_CompletedQuest(completedQuestCodes);
        }
        return "Success";
    }
    private int[] CodesSTR_To_IntegerArray(string originalSTR)
    {
        string[] splitedSTR = originalSTR.Split(',');
        int[] codes = new int[splitedSTR.Length];
        for (int i = 0; i < codes.Length; ++i)
            codes[i] = int.Parse(splitedSTR[i]);
        return codes;
    }
    #endregion

    #region Load Public DB Data
    public string LoadItemDB()
    {
        string weaponQuery = $"SELECT * FROM dbo.Weapon";
        DataSet weaponDataset = ConnectToDB("Item_DB", weaponQuery);
        if (weaponDataset == null)
            return "Fail";
        ItemDB.Instance.ContainWeaponData(weaponDataset);

        string expendableQuery = $"SELECT * FROM dbo.Expendable";
        DataSet expendableDataset = ConnectToDB("Item_DB", expendableQuery);
        if (expendableDataset == null)
            return "Fail";
        ItemDB.Instance.ContainExpandableData(expendableDataset);

        string accesorieQuery = $"SELECT * FROM dbo.Accesorie";
        DataSet accesorieDataset = ConnectToDB("Item_DB", accesorieQuery);
        if (accesorieDataset == null)
            return "Fail";
        ItemDB.Instance.ContainAccesorieData(accesorieDataset);

        string etcQuery = $"SELECT * FROM dbo.Etc";
        DataSet etcDataset = ConnectToDB("Item_DB", etcQuery);
        if (etcDataset == null)
            return "Fail";
        ItemDB.Instance.ContainEtcData(etcDataset);

        return "Success";
    }
    public NPCData LoadNPCData(int npcCode)
    {
        string query = $"SELECT * FROM dbo.NPC WHERE NPCCode = '{npcCode}'";
        DataSet dataSet = ConnectToDB("Game_DB", query);

        string jsonStr = dataSet.Tables[0].Rows[0].ItemArray[1].ToString();
        NPCData npcData = JsonUtility.FromJson<NPCData>(jsonStr);

        npcData.QuestDatas = new QuestData[npcData.Quest.Length];
        for (int i = 0; i < npcData.Quest.Length; ++i)
        {
            npcData.QuestDatas[i] = LoadQuestData(npcData.Quest[i]);
        }
        return npcData;
    }
    public QuestData LoadQuestData(int questCode)
    {
        string query = $"SELECT * FROM dbo.Quest WHERE QuestCode = '{questCode}'";
        DataSet dataSet = ConnectToDB("Game_DB", query);

        string jsonSTR = string.Empty;
        QuestData questData = null;
        if (dataSet != null)
        {
            try
            {
                jsonSTR = dataSet.Tables[0].Rows[0].ItemArray[1].ToString();
                questData = JsonUtility.FromJson<QuestData>(jsonSTR);
            }
            catch(Exception e)
            {
                Debug.Log($"LoadQuestData JSON Exception : {e.Message}");
            }

            string[] questCategorys = questData.QuestCategorys.Split(',');
            for (int i = 0; i < questCategorys.Length; ++i)
            {
                switch(questCategorys[i])
                {
                    case "Discussion":
                        questData.Behaviour_Discussion = LoadQuestBehaviour_Discussion(questData.QuestCode);
                        break;
                    case "Building":
                        questData.Behaviour_Building = LoadQuestBehaviour_Building(questData.QuestCode);
                        break;
                    case "GetItem":
                        questData.Behaviour_GetItem = LoadQuestBehaviour_GetItem(questData.QuestCode);
                        break;
                    case "KillMonster":
                        questData.Behaviour_KillMonster = LoadQuestBehaviour_KillMonster(questCode);
                        break;
                }
            }
            return questData;
        }
        else
        {
            Debug.Log($"QuestDB 에서 {questCode} 퀘스트를 찾을 수 없습니다");
            return null;
        }
    }
    private QuestBehaviour_Discussion LoadQuestBehaviour_Discussion(int questCode)
    {
        string query = $"SELECT * FROM dbo.QuestBehaviour_Discussion WHERE QuestCode = '{questCode}'";
        DataSet dataSet = ConnectToDB("Game_DB", query);

        string jsonSTR = string.Empty;
        QuestBehaviour_Discussion behaviour_Discussion = null;
        if (dataSet != null)
        {
            try
            {
                jsonSTR = dataSet.Tables[0].Rows[0].ItemArray[1].ToString();
                behaviour_Discussion = JsonUtility.FromJson<QuestBehaviour_Discussion>(jsonSTR);
            }
            catch(Exception e)
            {
                Debug.Log($"QuestBehaviour_Discussion : {questCode} 오류 / {e.Message}");
            }
            return behaviour_Discussion;
        }
        else
        {
            Debug.Log($"QuestBehaviour_Discussion DB 에서 {questCode} 퀘스트 Behaviour 를 찾을 수 없습니다");
            return null;
        }
    }
    private QuestBehaviour_Building LoadQuestBehaviour_Building(int questCode)
    {
        string query = $"SELECT * FROM dbo.QuestBehaviour_Building WHERE QuestCode = '{questCode}'";
        DataSet dataSet = ConnectToDB("Game_DB", query);

        string jsonSTR = string.Empty;
        QuestBehaviour_Building behaviour_Building = null;
        if (dataSet != null)
        {
            try
            {
                jsonSTR = dataSet.Tables[0].Rows[0].ItemArray[1].ToString();
                behaviour_Building = JsonUtility.FromJson<QuestBehaviour_Building>(jsonSTR);
            }
            catch(Exception e)
            {
                Debug.Log($"QuestBehaviour_Building : {questCode} 오류 / {e.Message}");
            }
            return behaviour_Building;
        }
        else
        {
            Debug.Log($"QuestBehaviour_Building DB 에서 {questCode} 퀘스트 Behaviour 를 찾을 수 없습니다");
            return null;
        }
    }
    private QuestBehaviour_GetItem LoadQuestBehaviour_GetItem(int questCode)
    {
        string query = $"SELECT * FROM dbo.QuestBehaviour_GetItem WHERE QuestCode = '{questCode}'";
        DataSet dataSet = ConnectToDB("Game_DB", query);

        string jsonSTR = string.Empty;
        QuestBehaviour_GetItem behaviour_GetItem = null;
        if (dataSet != null)
        {
            try
            {
                jsonSTR = dataSet.Tables[0].Rows[0].ItemArray[1].ToString();
                behaviour_GetItem = JsonUtility.FromJson<QuestBehaviour_GetItem>(jsonSTR);
            }
            catch(Exception e)
            {
                Debug.Log($"QuestBehaviour_GetItem : {questCode} 오류 / {e.Message}");
            }
            return behaviour_GetItem;
        }
        else
        {
            Debug.Log($"QuestBehaviour_GetItem DB 에서 {questCode} 퀘스트 Behaviour 를 찾을 수 없습니다");
            return null;
        }
    }
    private QuestBehaviour_KillMonster LoadQuestBehaviour_KillMonster(int questCode)
    {
        string query = $"SELECT * FROM dbo.QuestBehaviour_KillMonster WHERE QuestCode = '{questCode}'";
        DataSet dataSet = ConnectToDB("Game_DB", query);

        string jsonSTR = string.Empty;
        QuestBehaviour_KillMonster behaviour_KillMonster = null;
        if (dataSet != null)
        {
            try
            {
                jsonSTR = dataSet.Tables[0].Rows[0].ItemArray[1].ToString();
                behaviour_KillMonster = JsonUtility.FromJson<QuestBehaviour_KillMonster>(jsonSTR);
            }
            catch(Exception e)
            {
                Debug.Log($"QuestBehaviour_KillMonster : {questCode} 오류 / {e.Message}");
            }
            return behaviour_KillMonster;
        }
        else
        {
            Debug.Log($"QuestBehaviour_KillMonster DB 에서 {questCode} 퀘스트 Behaviour 를 찾을 수 없습니다");
            return null;
        }
    }
    #endregion
}
