using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;
using System.Data.SqlClient;
using MonsterAttackBehaviour;
using System.Text;

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

    private DataSet ConnectDB_GetDataSet(string dbName, string sql)
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
        catch(Exception)
        {
            return null;
        }
    }
    private void ConnectDB_ExecuteNonQuery(string dbName, string sql)
    {
        string conncStr = $"Server=192.168.0.5; Database={dbName}; uid=sa; pwd=4376";

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
        string query = $"SELECT * FROM dbo.UserAccount WHERE Account_ID = '{id}' AND (Account_Password = '{password}')";
        DataSet dataSet = ConnectDB_GetDataSet("Account_DB", query);

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
        DataSet dataSet = ConnectDB_GetDataSet("PlayerInfo_DB", query);

        if (dataSet == null)
            return "서버에 연결할 수 없습니다.";

        DataRow row = dataSet.Tables[0].Rows[0];
        UserInfoProvider.Instance.Initialize(row["UserAccount"].ToString(),
                                     row["LastMap"].ToString(),
                                     row["LastPos"].ToString(),
                                     Convert.ToSingle(row["MoveSpeed"]),
                                     Convert.ToSingle(row["JumpSpeed"]),
                                     Convert.ToSingle(row["HealthPoint"]),
                                     Convert.ToSingle(row["MaxHealthPoint"]),
                                     Convert.ToSingle(row["SheildPoint"]),
                                     Convert.ToSingle(row["AttackPoint"]),
                                     Convert.ToSingle(row["AttackSpeed"]),
                                     Convert.ToSingle(row["GatheringPower"]),
                                     (int)row["LevelupExperience"],
                                     (int)row["CurrentExperience"],
                                     (int)row["Level"],
                                     (int)row["WorkPoint"],
                                     (int)row["MaxWorkPoint"],
                                     (int)row["Gold"]);
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

        QuestProgress_Discussion progress_Discussion;
        QuestProgress_KillMonster progress_KillMonster;
        if (!questCodesInProgressSTR.Equals("None"))
        {
            int[] progressQuestCodes = CodesSTR_To_IntegerArray(questCodesInProgressSTR);

            // Discussion
            if (questInProgress_Discussion_JSON.Equals("None"))
            {
                progress_Discussion = new QuestProgress_Discussion();
                progress_Discussion.Initiailize();
            }
            else
                progress_Discussion = JsonUtility.FromJson<QuestProgress_Discussion>(questInProgress_Discussion_JSON);
            // KillMonster
            if (questInProgress_KillMonster_JSON.Equals("None"))
            {
                progress_KillMonster = new QuestProgress_KillMonster();
                progress_KillMonster.Initialize();
            }
            else
                progress_KillMonster = JsonUtility.FromJson<QuestProgress_KillMonster>(questInProgress_KillMonster_JSON);

            UserQuestProvider.Instance.Initialize_ProgressQuest(progressQuestCodes, progress_Discussion, progress_KillMonster);
        }
        else
            UserQuestProvider.Instance.Initialize();
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
        builder.Append($"SheildPoint='{UserInfoProvider.Instance.ShieldPoint}', ");
        builder.Append($"AttackPoint='{UserInfoProvider.Instance.AttackPoint}', ");
        builder.Append($"AttackSpeed='{UserInfoProvider.Instance.AttackSpeed}', ");
        builder.Append($"GatheringPower='{UserInfoProvider.Instance.GatheringPower}', ");
        builder.Append($"LevelupExperience='{UserInfoProvider.Instance.LevelupExperience}', ");
        builder.Append($"CurrentExperience='{UserInfoProvider.Instance.CurrentExperience}', ");
        builder.Append($"Level='{UserInfoProvider.Instance.Level}', ");
        builder.Append($"Workpoint='{UserInfoProvider.Instance.WorkPoint}', ");
        builder.Append($"MaxWorkPoint='{UserInfoProvider.Instance.MaxWorkPoint}', ");
        builder.Append($"Gold='{UserInfoProvider.Instance.Gold}' ");

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
    public void Save_PlayerQuest()
    {

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
