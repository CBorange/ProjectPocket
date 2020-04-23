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
    public void ContainWeaponData(WeaponData data)
    {
        weaponDatas.Add(data.ItemCode, data);
    }
    public void ContainExpandableData(ExpendableData data)
    {
        expendableDatas.Add(data.ItemCode, data);
    }
    public void ContainAccesorieData(AccesorieData data)
    {
        accesorieDatas.Add(data.ItemCode, data);
    }
    public void ContainEtcData(EtcData data)
    {
        etcDatas.Add(data.ItemCode, data);
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
