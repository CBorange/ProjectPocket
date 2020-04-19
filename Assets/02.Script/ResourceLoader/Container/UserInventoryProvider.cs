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
}
