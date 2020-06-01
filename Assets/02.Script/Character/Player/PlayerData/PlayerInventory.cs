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
        for (int i = 0; i < inventoryItems.Length; ++i)
        {
            AddItemIntoInventoryProcess(inventoryItems[i]);
        }
    }
    public InventoryItem GetItem(int itemCode)
    {
        InventoryItem foundItem = null;
        if (allItems.TryGetValue(itemCode, out foundItem))
            return foundItem;
        else
            return null;
    }
    public void RemoveItemFromInventory(int itemCode, int count)
    {
        ItemData data = ItemDB.Instance.GetItemData(itemCode);
        switch(data.ItemType)
        {
            case "Weapon":
                if (!weaponItems.Remove(itemCode))
                {
                    Debug.Log($"{itemCode} 에 해당하는 무기 가 인벤토리에 없습니다.");
                    return;
                }
                allItems.Remove(itemCode);
                break;
            case "Accesorie":
                if (!accesorieItems.Remove(itemCode))
                {
                    Debug.Log($"{itemCode} 에 해당하는 악세사리 가 인벤토리에 없습니다.");
                    return;
                }
                allItems.Remove(itemCode);
                break;
            case "Expendable":
                InventoryItem foundExpendable = null;
                if (expendableItems.TryGetValue(itemCode, out foundExpendable))
                {
                    if (foundExpendable.ItemCount - count < 1)
                    {
                        if (foundExpendable.ItemCount - count < 0)
                            Debug.Log($"{itemCode} 에 해당하는 소모품의 소지개수가 {foundExpendable.ItemCount} 개 인데, {count} 개를 인벤토리에서 제거하려 시도했습니다." +
                                $"아이템은 제거됩니다.");
                        expendableItems.Remove(itemCode);
                        allItems.Remove(itemCode);
                    }
                    else
                        foundExpendable.ItemCount -= count;
                }
                else
                {
                    Debug.Log($"{itemCode} 에 해당하는 소모품이 인벤토리에 없습니다.");
                    return;
                }
                break;
            case "Etc":
                InventoryItem foundEtc = null;
                if (etcItems.TryGetValue(itemCode, out foundEtc))
                {
                    if (foundEtc.ItemCount - count < 1)
                    {
                        if (foundEtc.ItemCount - count < 0)
                            Debug.Log($"{itemCode} 에 해당하는 기타 의 소지개수가 {foundEtc.ItemCount} 개 인데, {count} 개를 인벤토리에서 제거하려 시도했습니다." +
                                $"아이템은 제거됩니다.");
                        etcItems.Remove(itemCode);
                        allItems.Remove(itemCode);
                    }
                    else
                        foundEtc.ItemCount -= count;
                }
                else
                {
                    Debug.Log($"{itemCode} 에 해당하는 기타 아이템이 인벤토리에 없습니다.");
                    return;
                }
                break;
        }
    }
    public void AddItemToInventory(InventoryItem[] items)
    {
        for (int i = 0; i < items.Length; ++i)
        {
            AddItemIntoInventoryProcess(items[i]);
            PlayerQuest.Instance.UpdateGetItemQuest(items[i].OriginalItemData.ItemCode);
        }
    }
    public void AddItemToInventory(InventoryItem item)
    {
        AddItemIntoInventoryProcess(item);
        PlayerQuest.Instance.UpdateGetItemQuest(item.OriginalItemData.ItemCode);
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
