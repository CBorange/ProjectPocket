using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInventoryProvider
{
    // Singleton
    private UserInventoryProvider() { }
    private static UserInventoryProvider instance;
    public static UserInventoryProvider Instance
    {
        get
        {
            if (instance == null)
                instance = new UserInventoryProvider();
            return instance;
        }
    }

    private InventoryItem[] inventoryItems;
    public InventoryItem[] InventoryItems
    {
        get { return inventoryItems; }
    }
    public void Initialize(InventoryItem[] datas)
    {
        inventoryItems = datas;
    }
    public void Save_PlayerInventory_UpdateServerDB()
    {
        inventoryItems = new InventoryItem[PlayerInventory.Instance.AllItems.Count];
        int idx = 0;
        foreach (var kvp in PlayerInventory.Instance.AllItems)
        {
            inventoryItems[idx] = kvp.Value;
            idx += 1;
        }
        DBConnector.Instance.Save_PlayerInventory();
    }
}
