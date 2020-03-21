using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    public string ItemType;
    public int ItemCode;
    public ItemData(string itemType, int itemCode)
    {
        ItemType = itemType;
        ItemCode = itemCode;
    }
}
