using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponData : ItemData
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
    private string weaponType;
    public string WeaponType
    {
        get { return weaponType; }
        set { weaponType = value; }
    }
    private float attackPoint;
    public float AttackPoint
    {
        get { return attackPoint; }
        set { attackPoint = value; }
    }
    private float attackSpeed;
    public float AttackSpeed
    {
        get { return attackSpeed; }
        set { attackSpeed = value; }
    }
    private float range;
    public float Range
    {
        get { return range; }
        set { range = value; }
    }
    private Vector3 grapPoint;
    public Vector3 GrapPoint
    {
        get { return grapPoint; }
        set { grapPoint = value; }
    }
    private Vector3 grapRotation;
    public Vector3 GrapRotation
    {
        get { return grapRotation; }
        set { grapRotation = value; }
    }
    private float triggerDelay;
    public float TriggerDelay
    {
        get { return triggerDelay; }
        set { triggerDelay = value; }
    }
    private float triggerHold;
    public float TriggerHold
    {
        get { return triggerHold; }
        set { triggerHold = value; }
    }

    public WeaponData(string name, string introduce, int itemCode, string weaponType, float attackPoint, float attackSpeed, float range,
        Vector3 grapPoint, Vector3 grapRotation, float triggerDelay, float triggerHold)
    {
        this.name = name;
        this.introduce = introduce;
        this.itemCode = itemCode;
        this.weaponType = weaponType;
        this.attackPoint = attackPoint;
        this.attackSpeed = attackSpeed;
        this.range = range;
        this.grapPoint = grapPoint;
        this.grapRotation = grapRotation;
        this.triggerDelay = triggerDelay;
        this.triggerHold = triggerHold;
    }
}
