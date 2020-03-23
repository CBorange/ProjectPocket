using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpliedItemData
{
    public string ItemType;
    public int ItemCode;
    public ImpliedItemData(string itemType, int itemCode)
    {
        ItemType = itemType;
        ItemCode = itemCode;
    }
}
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

    private ImpliedItemData[] impliedItemDatas;
    public ImpliedItemData[] ImplitedItemDatas
    {
        get { return impliedItemDatas; }
    }
    public void Initialize(ImpliedItemData[] datas)
    {
        impliedItemDatas = datas;
    }
}
