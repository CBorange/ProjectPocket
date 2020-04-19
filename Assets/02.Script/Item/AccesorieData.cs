using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccesorieData : ItemData
{
    private string accesorieType;
    public string AccesorieType
    {
        get { return accesorieType; }
        set { accesorieType = value; }
    }
    public AccesorieData(string name, string introduce, int itemCode, string itemType, string accesorieType)
    {
        this.name = name;
        this.introduce = introduce;
        this.itemCode = itemCode;
        this.itemType = itemType;
        this.accesorieType = accesorieType;
    }
}
