using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Data;

[System.Serializable]
public class ImpliedItemData
{
    public string ItemType;
    public int ItemCode;
    public int ItemCount;
    public ImpliedItemData() { }
    public ImpliedItemData(string itemType, int itemCode, int itemCount)
    {
        ItemType = itemType;
        ItemCode = itemCode;
        ItemCount = itemCount;
    }
}
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

            string[] splitedGrapPoint = dataItems[7].ToString().Split(',');
            string[] splitedGrapRotation = dataItems[8].ToString().Split(',');
            Vector3 grapPoint = new Vector3(float.Parse(splitedGrapPoint[0]), float.Parse(splitedGrapPoint[1]), float.Parse(splitedGrapPoint[2]));
            Vector3 grapRotation = new Vector3(float.Parse(splitedGrapRotation[0]), float.Parse(splitedGrapRotation[1]), float.Parse(splitedGrapRotation[2]));

            WeaponData newData = new WeaponData(dataItems[0].ToString(), dataItems[1].ToString(), (int)dataItems[2],
                dataItems[3].ToString(), Convert.ToSingle(dataItems[4]), Convert.ToSingle(dataItems[5]), Convert.ToSingle(dataItems[6]),
                grapPoint, grapRotation);
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
            Debug.Log($"{itemCode}아이템을 찾을 수 없습니다.");
            return null;
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
            Debug.Log($"{itemCode}아이템을 찾을 수 없습니다.");
            return null;
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
            Debug.Log($"{itemCode}아이템을 찾을 수 없습니다.");
            return null;
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
            Debug.Log($"{itemCode}아이템을 찾을 수 없습니다.");
            return null;
        }
    }
    public ItemData GetItemData(int itemCode)
    {
        ItemData foundData = null;

        if (itemCode > 10000 && itemCode < 20000)
            foundData = GetWeaponData(itemCode);
        else if (itemCode > 20000 && itemCode < 30000)
            foundData = GetAccesorieData(itemCode);
        else if (itemCode > 30000 && itemCode < 40000)
            foundData = GetExpendableData(itemCode);
        else if (itemCode > 40000 && itemCode < 50000)
            foundData = GetEtcData(itemCode);

        if (foundData == null)
            Debug.Log($"ItemData 불러오기 에러 : {itemCode}는 아이템 코드가 아니거나 존재하지 않습니다.");
        return foundData;
    }
}
