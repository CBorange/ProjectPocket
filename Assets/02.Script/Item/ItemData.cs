using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData
{
    protected string name;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    protected string introduce;
    public string Introduce
    {
        get { return introduce; }
        set { introduce = value; }
    }
    protected string itemType;
    public string ItemType
    {
        get { return itemType; }
        set { itemType = value; }
    }
    protected int itemCode;
    public int ItemCode
    {
        get { return itemCode; }
        set { itemCode = value; }
    }
}
