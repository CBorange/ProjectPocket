using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponData : ItemData
{
    public string WeaponType;
    public float Range;
    public Vector3 GrapPoint;
    public Vector3 GrapRotation;
    public float TriggerDelay;
    public float TriggerHold;
    public ItemStat[] WeaponStat;
}
