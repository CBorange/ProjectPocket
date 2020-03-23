using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccesorieData : ItemData
{
    private string name;
    public string Name
    {
        get { return name; }
        set { name = value; }
    }
    private string introduce;
    public string Introduce
    {
        get { return introduce; }
        set { introduce = value; }
    }
    private int itemCode;
    public int ItemCode
    {
        get { return itemCode; }
        set { itemCode = value; }
    }
    public AccesorieData(string name, string introduce, int itemCode)
    {
        this.name = name;
        this.introduce = introduce;
        this.itemCode = itemCode;
    }
}
