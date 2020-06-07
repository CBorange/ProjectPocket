using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;
using System.Data.SqlClient;
using MonsterAttackBehaviour;
using System.Text;

public class DBConnector
{
    #region Singleton
    private DBConnector()
    {
    }
    private static DBConnector instance;
    public static DBConnector Instance
    {
        get
        {
            if (instance == null)
                instance = new DBConnector();
            return instance;
        }
    }
    #endregion

    private SqlCommand sqlCommand;
    private SqlConnection sqlConnection;

    private DataSet ConnectDB_GetDataSet(string dbName, string sql, out string errorMSG)
    {
        string conncStr = $"data source=39.123.41.181, 1433; initial catalog={dbName}; user id=sa; password=4376; Connect Timeout=10";

        try
        {
            using (SqlConnection conn = new SqlConnection(conncStr))
            {
                DataSet dataSet = new DataSet();
                conn.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conncStr);
                adapter.Fill(dataSet);
                errorMSG = string.Empty;
                return dataSet;
            }
        }
        catch(Exception e)
        {
            errorMSG = e.ToString();
            return null;
        }
    }
    private DataSet ConnectDB_GetDataSet(string dbName, string sql)
    {
        string conncStr = $"Server=39.123.41.181; Database={dbName}; uid=sa; pwd=4376";

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
        catch (Exception)
        {
            return null;
        }
    }
    private void ConnectDB_ExecuteNonQuery(string dbName, string sql)
    {
        string conncStr = $"Server=39.123.41.181; Database={dbName}; uid=sa; pwd=4376";

        try
        {
            using (SqlConnection conn = new SqlConnection(conncStr)) 
            {
                conn.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = conn;
                command.CommandText = sql;
                command.ExecuteNonQuery();
            }
        }
        catch (Exception e)
        {
            Debug.Log($"ConnectDB : ExecuteNonQuery 실행 실패 = {e.Message}");
            return;
        }
    }

    #region Load User DB Data
    public string ValiadeAccountOnDB(string id, string password)
    {
        string dbErrorMSG = string.Empty;
        string query = $"SELECT * FROM dbo.UserAccount WHERE Account_ID = '{id}' AND (Account_Password = '{password}')";
        DataSet dataSet = ConnectDB_GetDataSet("Account_DB", query,out dbErrorMSG);

        if (dataSet == null)
        {
            return $"서버에 연결할 수 없습니다. : {dbErrorMSG}";
        }

        if (dataSet.Tables.Count > 0)
        {
            if (dataSet.Tables[0].Rows.Count > 0)
                return "Success";
            else
                return "계정을 찾을 수 없습니다.";
        }
        else
            return $"서버에서 데이터를 찾을 수 없습니다 : {dbErrorMSG}";
    }
    public string LoadUserInfo(string accountID)
    {
        string dbErrorMSG = string.Empty;
        string query = $"SELECT * FROM dbo.PlayerStatus WHERE UserAccount = '{accountID}'";
        DataSet dataSet = ConnectDB_GetDataSet("PlayerInfo_DB", query, out dbErrorMSG);

        if (dataSet == null)
            return $"서버에 연결할 수 없습니다 : {dbErrorMSG}";

        DataRow row = dataSet.Tables[0].Rows[0];

        string statUsageJSON = row["StatUsageJSON"].ToString();
        PlayerStatUsage statUsage = JsonUtility.FromJson<PlayerStatUsage>(statUsageJSON);
        UserInfoProvider.Instance.Initialize(row["UserAccount"].ToString(),
                                     row["LastMap"].ToString(),
                                     row["LastPos"].ToString(),
                                     Convert.ToSingle(row["MoveSpeed"]),
                                     Convert.ToSingle(row["JumpSpeed"]),
                                     Convert.ToSingle(row["HealthPoint"]),
                                     Convert.ToSingle(row["MaxHealthPoint"]),
                                     Convert.ToSingle(row["ShieldPoint"]),
                                     Convert.ToSingle(row["AttackPoint"]),
                                     Convert.ToSingle(row["AttackSpeed"]),
                                     Convert.ToSingle(row["GatheringPower"]),
                                     Convert.ToSingle(row["LevelupExperience"]),
                                     Convert.ToSingle(row["CurrentExperience"]),
                                     (int)row["Level"],
                                     Convert.ToSingle(row["WorkPoint"]),
                                     Convert.ToSingle(row["MaxWorkPoint"]),
                                     (int)row["Gold"],
                                     (int)row["StatPoint"],
                                     statUsage,
                                     (bool)row["FirstLogin"]);
        return "Success";
    }

    public string LoadUserInventory()
    {
        string query = $"SELECT * FROM dbo.PlayerInventory WHERE UserAccount = '{UserInfoProvider.Instance.UserAccount}'";
        DataSet dataSet = ConnectDB_GetDataSet("PlayerInfo_DB", query);

        if (dataSet == null)
            return "서버에 연결할 수 없습니다.";

        object[] dataArray = dataSet.Tables[0].Rows[0].ItemArray;
        string inventoryJSON = dataArray[1].ToString();

        InventoryJSON jsonObject = JsonUtility.FromJson<InventoryJSON>(inventoryJSON);
        InventoryJSONUnit[] inventoryItemJSONInfos = jsonObject.ItemUnits;
        if (inventoryItemJSONInfos.Length == 0)
            UserInventoryProvider.Instance.Initialize(null);
        else
        {
            InventoryItem[] inventoryItems = new InventoryItem[inventoryItemJSONInfos.Length];
            for (int i = 0; i < inventoryItems.Length; ++i)
            {
                ItemData data = ItemDB.Instance.GetItemData(inventoryItemJSONInfos[i].ItemCode);
                inventoryItems[i] = new InventoryItem(data, inventoryItemJSONInfos[i].ItemCount);
            }
            UserInventoryProvider.Instance.Initialize(inventoryItems);
        }
        return "Success";
    }
    public string LoadUserBuilding()
    {
        string query = $"SELECT * FROM dbo.PlayerBuilding WHERE UserAccount = '{UserInfoProvider.Instance.UserAccount}'";
        DataSet dataSet = ConnectDB_GetDataSet("PlayerInfo_DB", query);

        if (dataSet == null)
            return "서버에 연결할 수 없습니다.";

        object[] dataArray = dataSet.Tables[0].Rows[0].ItemArray;
        string buildingJSON = dataArray[1].ToString();

        BuildingJSON jsonObj = JsonUtility.FromJson<BuildingJSON>(buildingJSON);
        BuildingStatus[] statuses = jsonObj.Statuses;
        if (statuses.Length == 0)
            UserBuildingProvider.Instance.Initialize(null);
        else
            UserBuildingProvider.Instance.Initialize(statuses);
        return "Success";
    }
    public string LoadUserQuickSlot()
    {
        string query = $"SELECT * FROM dbo.PlayerQuickSlot WHERE UserAccount = '{UserInfoProvider.Instance.UserAccount}'";
        DataSet dataSet = ConnectDB_GetDataSet("PlayerInfo_DB", query);

        if (dataSet == null)
            return "서버에 연결할 수 없습니다.";

        DataRow row = dataSet.Tables[0].Rows[0];
        UserQuickSlotProvider.Instance.Initialize((int)row["Slot_0"], (int)row["Slot_1"], (int)row["Slot_2"], (int)row["Slot_3"],
            (int)row["Slot_4"]);
        return "Success";
    }
    public string LoadUserEquipment()
    {
        string query = $"SELECT * FROM dbo.PlayerEquipments WHERE UserAccount = '{UserInfoProvider.Instance.UserAccount}'";
        DataSet dataSet = ConnectDB_GetDataSet("PlayerInfo_DB", query);

        if (dataSet == null)
            return "서버에 연결할 수 없습니다.";

        if (dataSet.Tables[0].Rows.Count == 0)
        {
            UserEquipmentProvider.Instance.Initialize(null, null, null);
            return "Success";
        }
        object[] dataArray = dataSet.Tables[0].Rows[0].ItemArray;
        int equipedWeaponCode = (int)dataArray[1];
        int equipedRingCode = (int)dataArray[2];
        int equipedNecklaceCode = (int)dataArray[3];

        UserEquipmentProvider.Instance.Initialize(ItemDB.Instance.GetWeaponData(equipedWeaponCode),
                                                  ItemDB.Instance.GetAccesorieData(equipedRingCode),
                                                  ItemDB.Instance.GetAccesorieData(equipedNecklaceCode));
        return "Success";
    }
    public string LoadUserQuests()
    {
        string query = $"SELECT * FROM dbo.PlayerQuests WHERE UserAccount = '{UserInfoProvider.Instance.UserAccount}'";
        DataSet dataSet = ConnectDB_GetDataSet("PlayerInfo_DB", query);

        object[] dataArray = dataSet.Tables[0].Rows[0].ItemArray;

        string questCodesInProgressSTR = dataArray[1].ToString();
        string questInProgress_Discussion_JSON = dataArray[2].ToString();
        string questInProgress_KillMonster_JSON = dataArray[3].ToString();
        string completedQuests = dataArray[4].ToString();

        QuestProgress_Discussion progress_Discussion = JsonUtility.FromJson<QuestProgress_Discussion>(questInProgress_Discussion_JSON);
        QuestProgress_KillMonster progress_KillMonster = JsonUtility.FromJson<QuestProgress_KillMonster>(questInProgress_KillMonster_JSON);
        int[] progressQuestCodes = null;
        int[] completedQuestCodes = null;

        if (!string.IsNullOrEmpty(questCodesInProgressSTR)) 
            progressQuestCodes = CodesSTR_To_IntegerArray(questCodesInProgressSTR);
        if (!string.IsNullOrEmpty(completedQuests)) 
            completedQuestCodes = CodesSTR_To_IntegerArray(completedQuests);

        UserQuestProvider.Instance.Initialize(progressQuestCodes, completedQuestCodes, progress_Discussion, progress_KillMonster);
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

    #region Save User Data
    public void Save_PlayerStat()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append($"UPDATE dbo.PlayerStatus SET ");

        builder.Append($"LastMap='{UserInfoProvider.Instance.LastMap}', ");

        Vector3 lastPosVec = UserInfoProvider.Instance.LastPos;
        string lastPos = $"{lastPosVec.x},{lastPosVec.y},{lastPosVec.z}";
        builder.Append($"LastPos='{lastPos}', ");
        builder.Append($"MoveSpeed='{UserInfoProvider.Instance.MoveSpeed}', ");
        builder.Append($"JumpSpeed='{UserInfoProvider.Instance.JumpSpeed}', ");
        builder.Append($"HealthPoint='{UserInfoProvider.Instance.HealthPoint}', ");
        builder.Append($"MaxHealthPoint='{UserInfoProvider.Instance.MaxHealthPoint}', ");
        builder.Append($"ShieldPoint='{UserInfoProvider.Instance.ShieldPoint}', ");
        builder.Append($"AttackPoint='{UserInfoProvider.Instance.AttackPoint}', ");
        builder.Append($"AttackSpeed='{UserInfoProvider.Instance.AttackSpeed}', ");
        builder.Append($"GatheringPower='{UserInfoProvider.Instance.GatheringPower}', ");
        builder.Append($"LevelupExperience='{UserInfoProvider.Instance.LevelupExperience}', ");
        builder.Append($"CurrentExperience='{UserInfoProvider.Instance.CurrentExperience}', ");
        builder.Append($"Level='{UserInfoProvider.Instance.Level}', ");
        builder.Append($"Workpoint='{UserInfoProvider.Instance.WorkPoint}', ");
        builder.Append($"MaxWorkPoint='{UserInfoProvider.Instance.MaxWorkPoint}', ");
        builder.Append($"Gold='{UserInfoProvider.Instance.Gold}', ");
        builder.Append($"StatPoint='{UserInfoProvider.Instance.StatPoint}', ");
        string statUsageJSON = JsonUtility.ToJson(UserInfoProvider.Instance.StatUsage);
        builder.Append($"StatUsageJSON='{statUsageJSON}', ");
        builder.Append($"FirstLogin='{UserInfoProvider.Instance.FirstLogin}' ");

        builder.Append($"WHERE UserAccount='{UserInfoProvider.Instance.UserAccount}'");

        string sql = builder.ToString();
        ConnectDB_ExecuteNonQuery("PlayerInfo_DB", sql);
    }
    public void Save_PlayerBuilding()
    {
        BuildingJSON buildingJSON = new BuildingJSON();
        buildingJSON.Statuses = UserBuildingProvider.Instance.BuildingStatus;

        string jsonSTR = JsonUtility.ToJson(buildingJSON);
        string sql = $"UPDATE dbo.PlayerBuilding SET BuildingStatusJSON='{jsonSTR}' WHERE UserAccount='{UserInfoProvider.Instance.UserAccount}'";

        ConnectDB_ExecuteNonQuery("PlayerInfo_DB", sql);
    }
    public void Save_PlayerInventory()
    {
        InventoryItem[] userItems = UserInventoryProvider.Instance.InventoryItems;
        InventoryJSON inventoryJSON = new InventoryJSON();
        inventoryJSON.ItemUnits = new InventoryJSONUnit[userItems.Length];
        for (int i = 0; i < userItems.Length; ++i)
        {
            inventoryJSON.ItemUnits[i] = new InventoryJSONUnit();
            inventoryJSON.ItemUnits[i].ItemCode = userItems[i].OriginalItemData.ItemCode;
            inventoryJSON.ItemUnits[i].ItemCount = userItems[i].ItemCount;
        }

        string jsonSTR = JsonUtility.ToJson(inventoryJSON);
        string sql = $"UPDATE dbo.PlayerInventory SET InventoryJSON='{jsonSTR}' WHERE UserAccount='{UserInfoProvider.Instance.UserAccount}'";

        ConnectDB_ExecuteNonQuery("PlayerInfo_DB", sql);
    }
    public void Save_PlayerEquipment()
    {
        StringBuilder builder = new StringBuilder();
        builder.Append($"UPDATE dbo.PlayerEquipments SET ");

        int weaponCode = 0;
        if (UserEquipmentProvider.Instance.WeaponItem != null)
            weaponCode = UserEquipmentProvider.Instance.WeaponItem.ItemCode;
        int ringCode = 0;
        if (UserEquipmentProvider.Instance.Accesorie_Ring != null)
            ringCode = UserEquipmentProvider.Instance.Accesorie_Ring.ItemCode;
        int necklaceCode = 0;
        if (UserEquipmentProvider.Instance.Accesorie_Necklace != null)
            necklaceCode = UserEquipmentProvider.Instance.Accesorie_Necklace.ItemCode;
        builder.Append($"Weapon='{weaponCode}', ");
        builder.Append($"Accesorie_Ring='{ringCode}', ");
        builder.Append($"Accesorie_Necklace='{necklaceCode}' ");

        builder.Append($"WHERE UserAccount='{UserInfoProvider.Instance.UserAccount}'");

        string sql = builder.ToString();
        ConnectDB_ExecuteNonQuery("PlayerInfo_DB", sql);
    }
    public void Save_PlayerQuickSlot()
    {
        StringBuilder builder = new StringBuilder();

        int[] itemsInSlot = UserQuickSlotProvider.Instance.ItemsInSlot;
        builder.Append($"UPDATE dbo.PlayerQuickSlot SET ");
        builder.Append($"Slot_0='{itemsInSlot[0]}', ");
        builder.Append($"Slot_1='{itemsInSlot[1]}', ");
        builder.Append($"Slot_2='{itemsInSlot[2]}', ");
        builder.Append($"Slot_3='{itemsInSlot[3]}', ");
        builder.Append($"Slot_4='{itemsInSlot[4]}' ");

        builder.Append($"WHERE UserAccount='{UserInfoProvider.Instance.UserAccount}'");
        string sql = builder.ToString();
        ConnectDB_ExecuteNonQuery("PlayerInfo_DB", sql);
    }
    public void Save_PlayerQuest()
    {
        StringBuilder sqlBuilder = new StringBuilder();
        sqlBuilder.Append($"UPDATE dbo.PlayerQuests SET ");

        StringBuilder inProgressQuestsSTR_Builder = new StringBuilder();
        for (int i = 0; i < UserQuestProvider.Instance.QuestDatasInProgress.Count; ++i)
        {
            inProgressQuestsSTR_Builder.Append($"{UserQuestProvider.Instance.QuestDatasInProgress[i].QuestCode}");
            if (i < UserQuestProvider.Instance.QuestDatasInProgress.Count - 1)
                inProgressQuestsSTR_Builder.Append(",");
        }
        sqlBuilder.Append($"QuestCodesInProgress='{inProgressQuestsSTR_Builder.ToString()}', ");

        string inprogress_DiscussionJSON = JsonUtility.ToJson(UserQuestProvider.Instance.QuestProgress_Discussion);
        sqlBuilder.Append($"QuestInProgress_Discussion_JSON='{inprogress_DiscussionJSON}', ");

        string inprogress_KillMonsterJSON = JsonUtility.ToJson(UserQuestProvider.Instance.QuestProgress_KillMonster);
        sqlBuilder.Append($"QuestInProgress_KillMonster_JSON='{inprogress_KillMonsterJSON}', ");

        StringBuilder compltedQuestsSTR_Builder = new StringBuilder();
        for (int i = 0; i < UserQuestProvider.Instance.CompletedQuests.Count; ++i)
        {
            compltedQuestsSTR_Builder.Append($"{UserQuestProvider.Instance.CompletedQuests[i].QuestCode}");
            if (i < UserQuestProvider.Instance.CompletedQuests.Count - 1)
                compltedQuestsSTR_Builder.Append(",");
        }
        sqlBuilder.Append($"CompletedQuests='{compltedQuestsSTR_Builder.ToString()}' ");
        sqlBuilder.Append($"WHERE UserAccount='{UserInfoProvider.Instance.UserAccount}'");

        string sql = sqlBuilder.ToString();
        ConnectDB_ExecuteNonQuery("PlayerInfo_DB", sql);
    }
    #endregion

    #region Load Public DB Data
    public string LoadItemDB()
    {
        string weaponQuery = $"SELECT * FROM dbo.Weapon";
        DataSet weaponDataset = ConnectDB_GetDataSet("Item_DB", weaponQuery);
        if (weaponDataset == null)
            return "Fail";
        DataRowCollection weaponRow = weaponDataset.Tables[0].Rows;
        for (int i = 0; i < weaponRow.Count; ++i)
        {
            object[] items = weaponRow[i].ItemArray;
            string jsonSTR = items[1].ToString();
            WeaponData newWeapon = JsonUtility.FromJson<WeaponData>(jsonSTR);
            ItemDB.Instance.ContainWeaponData(newWeapon);
        }

        string expendableQuery = $"SELECT * FROM dbo.Expendable";
        DataSet expendableDataset = ConnectDB_GetDataSet("Item_DB", expendableQuery);
        if (expendableDataset == null)
            return "Fail";
        DataRowCollection expendableRow = expendableDataset.Tables[0].Rows;
        for (int i = 0; i < expendableRow.Count; ++i)
        {
            object[] items = expendableRow[i].ItemArray;
            string jsonSTR = items[1].ToString();
            ExpendableData newExpendable = JsonUtility.FromJson<ExpendableData>(jsonSTR);
            ItemDB.Instance.ContainExpandableData(newExpendable);
        }

        string accesorieQuery = $"SELECT * FROM dbo.Accesorie";
        DataSet accesorieDataset = ConnectDB_GetDataSet("Item_DB", accesorieQuery);
        if (accesorieDataset == null)
            return "Fail";
        DataRowCollection accesorieRow = accesorieDataset.Tables[0].Rows;
        for (int i = 0; i < accesorieRow.Count; ++i)
        {
            object[] items = accesorieRow[i].ItemArray;
            string jsonSTR = items[1].ToString();
            AccesorieData newAccesorie = JsonUtility.FromJson<AccesorieData>(jsonSTR);
            ItemDB.Instance.ContainAccesorieData(newAccesorie);
        }

        string etcQuery = $"SELECT * FROM dbo.Etc";
        DataSet etcDataset = ConnectDB_GetDataSet("Item_DB", etcQuery);
        if (etcDataset == null)
            return "Fail";
        DataRowCollection etcRow = etcDataset.Tables[0].Rows;
        for (int i = 0; i < etcRow.Count; ++i)
        {
            object[] items = etcRow[i].ItemArray;
            string jsonSTR = items[1].ToString();
            EtcData newETC = JsonUtility.FromJson<EtcData>(jsonSTR);
            ItemDB.Instance.ContainEtcData(newETC);
        }

        return "Success";
    }
    public string LoadExperienceTable()
    {
        string query = $"SELECT * FROM dbo.ExperienceTable";
        DataSet expDataSet = ConnectDB_GetDataSet("Game_DB", query);
        if (expDataSet == null)
            return "Fail";
        DataRowCollection rows = expDataSet.Tables[0].Rows;
        for (int i = 0; i < rows.Count; ++i)
        {
            int level = (int)rows[i]["Level"];
            float exp = Convert.ToSingle(rows[i]["RequiredExperience"]);
            ExperienceTable.Instance.ContainExperienceData(level, exp);
        }
        return "Success";
    }
    public NPCData LoadNPCData(int npcCode)
    {
        string query = $"SELECT * FROM dbo.NPC WHERE NPCCode = '{npcCode}'";
        DataSet dataSet = ConnectDB_GetDataSet("Game_DB", query);

        string jsonStr = dataSet.Tables[0].Rows[0].ItemArray[1].ToString();
        NPCData npcData = JsonUtility.FromJson<NPCData>(jsonStr);

        npcData.Initialize();
        for (int i = 0; i < npcData.Quest.Length; ++i)
            npcData.QuestDatas[i] = QuestDB.Instance.GetQuestData(npcData.Quest[i]);

        return npcData;
    }
    public ResourceData LoadResourceData(int resourceCode)
    {
        string query = $"SELECT * FROM dbo.Resource WHERE ResourceCode = '{resourceCode}'";
        DataSet dataSet = ConnectDB_GetDataSet("Game_DB", query);

        string jsonStr = dataSet.Tables[0].Rows[0].ItemArray[1].ToString();
        ResourceData resourceData = JsonUtility.FromJson<ResourceData>(jsonStr);

        return resourceData;
    }
    public BuildingData LoadBuildingData(int buildingCode)
    {
        string query = $"SELECT * FROM dbo.Building WHERE BuildingCode = '{buildingCode}'";
        DataSet dataSet = ConnectDB_GetDataSet("Game_DB", query);

        string jsonStr = dataSet.Tables[0].Rows[0].ItemArray[1].ToString();
        BuildingData buildingData = JsonUtility.FromJson<BuildingData>(jsonStr);

        return buildingData;
    }
    /// <summary>
    /// 이 함수는 오로지 QuestDB -> GetQuestData Method에 의해서만 호출되어야 합니다.
    /// </summary>
    /// <param name="questCode"></param>
    /// <returns></returns>
    public QuestData LoadQuestData(int questCode)
    {
        string query = $"SELECT * FROM dbo.Quest WHERE QuestCode = '{questCode}'";
        DataSet dataSet = ConnectDB_GetDataSet("Game_DB", query);

        string jsonSTR = string.Empty;
        QuestData questData = null;
        if (dataSet != null)
        {
            // Loading Original Quest Data
            try
            {
                jsonSTR = dataSet.Tables[0].Rows[0].ItemArray[1].ToString();
                questData = JsonUtility.FromJson<QuestData>(jsonSTR);
            }
            catch(Exception e)
            {
                Debug.Log($"LoadQuestData JSON Exception : {e.Message}");
            }

            // Loading Behaviour
            for (int i = 0; i < questData.QuestCategorys.Length; ++i)
            {
                switch(questData.QuestCategorys[i])
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

            // Loading Reward
            for (int i = 0; i < questData.QuestRewards.Length; ++i)
            {
                switch(questData.QuestRewards[i])
                {
                    case "GetItem":
                        questData.Reward_GetItem = LoadQuestReward_GetItem(questData.QuestCode);
                        break;
                    case "GetStatus":
                        questData.Reward_GetStatus = LoadQuestReward_GetStatus(questData.QuestCode);
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
        DataSet dataSet = ConnectDB_GetDataSet("Game_DB", query);

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
        DataSet dataSet = ConnectDB_GetDataSet("Game_DB", query);

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
        DataSet dataSet = ConnectDB_GetDataSet("Game_DB", query);

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
        DataSet dataSet = ConnectDB_GetDataSet("Game_DB", query);

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

    private QuestReward_GetItem LoadQuestReward_GetItem(int questCode)
    {
        string query = $"SELECT * FROM dbo.QuestReward_GetItem WHERE QuestCode = '{questCode}'";
        DataSet dataSet = ConnectDB_GetDataSet("Game_DB", query);

        string jsonSTR = string.Empty;
        QuestReward_GetItem reward_GetItem = null;
        if (dataSet != null)
        {
            try
            {
                jsonSTR = dataSet.Tables[0].Rows[0].ItemArray[1].ToString();
                reward_GetItem = JsonUtility.FromJson<QuestReward_GetItem>(jsonSTR);
            }
            catch(Exception e)
            {
                Debug.Log($"QuestReward_GetItem : {questCode} 오류 / {e.Message}");
            }
            return reward_GetItem;
        }
        else
        {
            Debug.Log($"QuestReward_GetItem DB 에서 {questCode} 퀘스트 Reward 를 찾을 수 없습니다");
            return null;
        }
    }
    private QuestReward_GetStatus LoadQuestReward_GetStatus(int questCode)
    {
        string query = $"SELECT * FROM dbo.QuestReward_GetStatus WHERE QuestCode = '{questCode}'";
        DataSet dataSet = ConnectDB_GetDataSet("Game_DB", query);

        string jsonSTR = string.Empty;
        QuestReward_GetStatus reward = null;
        if (dataSet != null)
        {
            try
            {
                jsonSTR = dataSet.Tables[0].Rows[0].ItemArray[1].ToString();
                reward = JsonUtility.FromJson<QuestReward_GetStatus>(jsonSTR);
            }
            catch (Exception e)
            {
                Debug.Log($"QuestReward_GetStatus : {questCode} 오류 / {e.Message}");
            }
            return reward;
        }
        else
        {
            Debug.Log($"QuestReward_GetStatus DB 에서 {questCode} 퀘스트 Reward 를 찾을 수 없습니다");
            return null;
        }
    }

    public MonsterData LoadMonsterData(int monsterCode)
    {
        string query = $"SELECT * FROM dbo.MonsterInfo WHERE MonsterCode = '{monsterCode}'";
        DataSet dataSet = ConnectDB_GetDataSet("Game_DB", query);

        string jsonSTR = string.Empty;
        MonsterData monster = null;
        if (dataSet != null)
        {
            try
            {
                jsonSTR = dataSet.Tables[0].Rows[0].ItemArray[2].ToString();
                monster = JsonUtility.FromJson<MonsterData>(jsonSTR);
            }
            catch (Exception e)
            {
                Debug.Log($"Monter DB(DBMS) : {monsterCode} 오류 / {e.Message}");
            }
            return monster;
        }
        else
        {
            Debug.Log($"Monter DB(DBMS) 에서 {monsterCode} 몬스터 를 찾을 수 없습니다");
            return null;
        }
    }
    public MonsterPatternJSON LoadMonsterAttackPattern(int monsterCode)
    {
        string query = $"SELECT * FROM dbo.MonsterPattern WHERE MonsterCode = '{monsterCode}'";
        DataSet dataSet = ConnectDB_GetDataSet("Game_DB", query);

        string jsonSTR = string.Empty;
        MonsterPatternJSON pattern = null;
        if (dataSet != null)
        {
            try
            {
                jsonSTR = dataSet.Tables[0].Rows[0].ItemArray[1].ToString();
                pattern = JsonUtility.FromJson<MonsterPatternJSON>(jsonSTR);
            }
            catch (Exception e)
            {
                Debug.Log($"Monter DB(DBMS) : {monsterCode} 오류 / {e.Message}");
            }
            return pattern;
        }
        else
        {
            Debug.Log($"Monter DB(DBMS) 에서 {monsterCode} 몬스터 패턴 을 찾을 수 없습니다");
            return null;
        }
    }
    #endregion
}
