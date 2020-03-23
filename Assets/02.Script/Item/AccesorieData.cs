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
    private string accesorieType;
    public string AccesorieType
    {
        get { return accesorieType; }
        set { accesorieType = value; }
    }
    public AccesorieData(string name, string introduce, int itemCode, string accesorieType)
    {
        this.name = name;
        this.introduce = introduce;
        this.itemCode = itemCode;
        this.accesorieType = accesorieType;
    }
}
