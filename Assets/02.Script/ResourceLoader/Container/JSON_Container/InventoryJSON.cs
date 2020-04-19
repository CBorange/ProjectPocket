using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryJSON
{
    public InventoryJSONUnit[] ItemUnits;
}

[System.Serializable]
public class InventoryJSONUnit
{
    public int ItemCode;
    public int ItemCount;
}
