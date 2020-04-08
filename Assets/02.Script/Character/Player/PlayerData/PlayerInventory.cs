using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, PlayerRuntimeData
{
    #region Singleton
    private static PlayerInventory instance;
    public static PlayerInventory Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<PlayerInventory>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("PlayerInventory").AddComponent<PlayerInventory>();
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
        var objs = FindObjectsOfType<PlayerInventory>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    // Data
    private Dictionary<int, ImpliedItemData> impliedWeaponDataInfos;
    public Dictionary<int, ImpliedItemData> ImpliedWeaponDataInfos
    {
        get { return impliedWeaponDataInfos; }
    }
    private Dictionary<int, ImpliedItemData> impliedAccesorieDataInfos;
    public Dictionary<int, ImpliedItemData> ImpliedAccesorieDataInfos
    {
        get { return impliedAccesorieDataInfos; }
    }
    private Dictionary<int, ImpliedItemData> impliedExpendableDataInfos;
    public Dictionary<int, ImpliedItemData> ImpliedExpendableDataInfos
    {
        get { return impliedExpendableDataInfos; }
    }
    private Dictionary<int, ImpliedItemData> impliedEtcDataInfos;
    public Dictionary<int, ImpliedItemData> ImpliedEtcDataInfos
    {
        get { return impliedEtcDataInfos; }
    }
    private Dictionary<int, ImpliedItemData> allItems;
    public Dictionary<int, ImpliedItemData> AllItems
    {
        get { return allItems; }
    }

    public void Initialize()
    {
        impliedWeaponDataInfos = new Dictionary<int, ImpliedItemData>();
        impliedAccesorieDataInfos = new Dictionary<int, ImpliedItemData>();
        impliedExpendableDataInfos = new Dictionary<int, ImpliedItemData>();
        impliedEtcDataInfos = new Dictionary<int, ImpliedItemData>();
        allItems = new Dictionary<int, ImpliedItemData>();

        ImpliedItemData[] inventoryItems = UserInventoryProvider.Instance.ImplitedItemDatas;
        if (inventoryItems == null)
            return;                                                                                                       
        for (int i = 0; i < inventoryItems.Length; ++i)
        {
            AddItemToInventory(inventoryItems[i]);
        }
    }
    public void AddItemToInventory(ImpliedItemData data)
    {
        switch (data.ItemType)
        {
            case "Weapon":
                if (allItems.ContainsKey(data.ItemCode))
                    return;
                impliedWeaponDataInfos.Add(data.ItemCode, data);
                break;
            case "Expendable":
                impliedExpendableDataInfos.Add(data.ItemCode, data);
                break;
            case "Accesorie":
                if (allItems.ContainsKey(data.ItemCode))
                    return;
                impliedAccesorieDataInfos.Add(data.ItemCode, data);
                break;
            case "Etc":
                impliedEtcDataInfos.Add(data.ItemCode, data);
                break;
        }
        allItems.Add(data.ItemCode, data);
    }
}
