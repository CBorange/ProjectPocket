using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Data;

public class ItemDB
{
    // Singleton
    private ItemDB() 
    {
        weaponDatas = new Dictionary<int, WeaponData>();
        expendableDatas = new Dictionary<int, ExpendableData>();
        accesorieDatas = new Dictionary<int, AccesorieData>();
        etcDatas = new Dictionary<int, EtcData>();
    }
    private static ItemDB instance;
    public static ItemDB Instance
    {
        get
        {
            if (instance == null)
                instance = new ItemDB();
            return instance;
        }
    }
    // Data
    private Dictionary<int,WeaponData> weaponDatas;
    private Dictionary<int, ExpendableData> expendableDatas;
    private Dictionary<int, AccesorieData> accesorieDatas;
    private Dictionary<int, EtcData> etcDatas;

    // Data Contain Method
    public void ContainWeaponData(DataSet dataSet)
    {
        DataRowCollection rowCollection = dataSet.Tables[0].Rows;
        for (int rowIdx = 0; rowIdx < rowCollection.Count; ++rowIdx)
        {
            object[] dataItems = rowCollection[rowIdx].ItemArray;
            WeaponData newData = new WeaponData(dataItems[0].ToString(), dataItems[1].ToString(), (int)dataItems[2],
                dataItems[3].ToString(), Convert.ToSingle(dataItems[4]), Convert.ToSingle(dataItems[5]), Convert.ToSingle(dataItems[6]));
            weaponDatas.Add((int)dataItems[2], newData);
        }
    }
    public void ContainExpandableData(DataSet dataSet)
    {
        DataRowCollection rowCollection = dataSet.Tables[0].Rows;
        for (int rowIdx = 0; rowIdx < rowCollection.Count; ++rowIdx)
        {
            object[] dataItems = rowCollection[rowIdx].ItemArray;
            ExpendableData newData = new ExpendableData(dataItems[0].ToString(), dataItems[1].ToString(), (int)dataItems[2]);
            expendableDatas.Add((int)dataItems[2], newData);
        }
    }
    public void ContainAccesorieData(DataSet dataSet)
    {
        DataRowCollection rowCollection = dataSet.Tables[0].Rows;
        for (int rowIdx = 0; rowIdx < rowCollection.Count; ++rowIdx)
        {
            object[] dataItems = rowCollection[rowIdx].ItemArray;
            AccesorieData newData = new AccesorieData(dataItems[0].ToString(), dataItems[1].ToString(), (int)dataItems[2], dataItems[3].ToString());
            accesorieDatas.Add((int)dataItems[2], newData);
        }
    }
    public void ContainEtcData(DataSet dataSet)
    {
        DataRowCollection rowCollection = dataSet.Tables[0].Rows;
        for (int rowIdx = 0; rowIdx < rowCollection.Count; ++rowIdx)
        {
            object[] dataItems = rowCollection[rowIdx].ItemArray;
            EtcData newData = new EtcData(dataItems[0].ToString(), dataItems[1].ToString(), (int)dataItems[2]);
            etcDatas.Add((int)dataItems[2], newData);
        }
    }

    // Get Method
    public WeaponData GetWeaponData(int itemCode)
    {
        WeaponData foundData = null;
        bool isSuccess = weaponDatas.TryGetValue(itemCode,out foundData);
        if (isSuccess)
            return foundData;
        else
        {
            return null;
            Debug.Log($"{itemCode}아이템을 찾을 수 없습니다.");
        }
    }
    public ExpendableData GetExpendableData(int itemCode)
    {
        ExpendableData foundData = null;
        bool isSuccess = expendableDatas.TryGetValue(itemCode, out foundData);
        if (isSuccess)
            return foundData;
        else
        {
            return null;
            Debug.Log($"{itemCode}아이템을 찾을 수 없습니다.");
        }
    }
    public AccesorieData GetAccesorieData(int itemCode)
    {
        AccesorieData foundData = null;
        bool isSuccess = accesorieDatas.TryGetValue(itemCode, out foundData);
        if (isSuccess)
            return foundData;
        else
        {
            return null;
            Debug.Log($"{itemCode}아이템을 찾을 수 없습니다.");
        }
    }
    public EtcData GetEtcData(int itemCode)
    {
        EtcData foundData = null;
        bool isSuccess = etcDatas.TryGetValue(itemCode, out foundData);
        if (isSuccess)
            return foundData;
        else
        {
            return null;
            Debug.Log($"{itemCode}아이템을 찾을 수 없습니다.");
        }
    }
}
