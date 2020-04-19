using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpendableData : ItemData
{
    public ExpendableData(string name, string introduce, int itemCode, string itemType)
    {
        this.name = name;
        this.introduce = introduce;
        this.itemCode = itemCode;
        this.itemType = itemType;
    }
}
