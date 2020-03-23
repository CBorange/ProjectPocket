using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;
using System.Data.SqlClient;

public class DBConnector
{
    // Singleton
    private DBConnector() { }
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

    #region Load Unique DB Data
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
                                     dataArray[8].ToString().Trim());
        return "Success";
    }
    public string LoadUserInventory()
    {
        string query = $"SELECT * FROM dbo.InventoryItems_{UserInfoProvider.Instance.UserAccount}";
        DataSet dataSet = ConnectToDB("PlayerInfo_DB", query);

        if (dataSet == null)
            return "서버에 연결할 수 없습니다.";

        DataRowCollection rowCollection = dataSet.Tables[0].Rows;
        ImpliedItemData[] impliedItemDatas = new ImpliedItemData[rowCollection.Count];
        for (int rowIdx = 0; rowIdx < rowCollection.Count; ++rowIdx)
        {
            object[] dataArray = rowCollection[rowIdx].ItemArray;
            ImpliedItemData newData = new ImpliedItemData(dataArray[0].ToString().Trim(), (int)dataArray[1]);
            impliedItemDatas[rowIdx] = newData;
        }

        UserInventoryProvider.Instance.Initialize(impliedItemDatas);
        return "Success";
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
    #endregion
}
