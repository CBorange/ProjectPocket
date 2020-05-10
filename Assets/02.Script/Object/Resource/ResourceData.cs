using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourceData
{
    public int ResourceCode;
    public string ResourceName;
    public string ResourceType;
    public string CanGatheringTool;
    public int HealthPoint;
    public int WorkPointUsage;
    public float Experience;
    public DropItemData[] DropItemDatas;
}
