using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem
{
    public ItemData OriginalItemData;
    public int ItemCount;
    public InventoryItem() { }
    public InventoryItem(ItemData data, int itemCount)
    {
        OriginalItemData = data;
        ItemCount = itemCount;
    }
}
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
    private Dictionary<int, InventoryItem> weaponItems;
    public Dictionary<int, InventoryItem> WeaponItems
    {
        get { return weaponItems; }
    }
    private Dictionary<int, InventoryItem> accesorieItems;
    public Dictionary<int, InventoryItem> AccesorieItems
    {
        get { return accesorieItems; }
    }
    private Dictionary<int, InventoryItem> expendableItems;
    public Dictionary<int, InventoryItem> ExpendableItems
    {
        get { return expendableItems; }
    }
    private Dictionary<int, InventoryItem> etcItems;
    public Dictionary<int, InventoryItem> EtcItems
    {
        get { return etcItems; }
    }
    private Dictionary<int, InventoryItem> allItems;
    public Dictionary<int, InventoryItem> AllItems
    {
        get { return allItems; }
    }

    public void Initialize()
    {
        weaponItems = new Dictionary<int, InventoryItem>();
        accesorieItems = new Dictionary<int, InventoryItem>();
        expendableItems = new Dictionary<int, InventoryItem>();
        etcItems = new Dictionary<int, InventoryItem>();
        allItems = new Dictionary<int, InventoryItem>();

        InventoryItem[] inventoryItems = UserInventoryProvider.Instance.InventoryItems;
        if (inventoryItems == null)
            return;
        AddItemToInventory(inventoryItems);
    }
    public void AddItemToInventory(InventoryItem[] items)
    {
        for (int i = 0; i < items.Length; ++i)
        {
            AddItemIntoInventoryProcess(items[i]);
        }
    }
    public void AddItemToInventory(InventoryItem item)
    {
        AddItemIntoInventoryProcess(item);
    }
    private void AddItemIntoInventoryProcess(InventoryItem item)
    {
        ItemData data = item.OriginalItemData;
        switch (data.ItemType)
        {
            case "Weapon":
                if (weaponItems.ContainsKey(data.ItemCode))
                    return;
                weaponItems.Add(data.ItemCode, item);
                allItems.Add(data.ItemCode, item);
                break;
            case "Expendable":
                InventoryItem foundExpendable = null;
                if (expendableItems.TryGetValue(data.ItemCode, out foundExpendable))
                {
                    foundExpendable.ItemCount += item.ItemCount;
                }
                else
                {
                    expendableItems.Add(data.ItemCode, item);
                    allItems.Add(data.ItemCode, item);
                }
                break;
            case "Accesorie":
                if (accesorieItems.ContainsKey(data.ItemCode))
                    return;
                accesorieItems.Add(data.ItemCode, item);
                allItems.Add(data.ItemCode, item);
                break;
            case "Etc":
                InventoryItem foundEtc = null;
                if (etcItems.TryGetValue(data.ItemCode, out foundEtc))
                {
                    foundEtc.ItemCount += item.ItemCount;
                }
                else
                {
                    etcItems.Add(data.ItemCode, item);
                    allItems.Add(data.ItemCode, item);
                }
                break;
        }
    }
}
