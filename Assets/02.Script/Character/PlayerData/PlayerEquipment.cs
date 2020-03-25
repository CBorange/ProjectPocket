using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    #region Singleton
    private static PlayerEquipment instance;
    public static PlayerEquipment Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<PlayerEquipment>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("PlayerEquipment").AddComponent<PlayerEquipment>();
                    instance = newSingleton;
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<PlayerEquipment>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    //Data 
    private WeaponData equipedWeapon;
    public WeaponData EquipedWeapon
    {
        get { return equipedWeapon; }
        set { equipedWeapon = value; }
    }
    private ImpliedItemData equipedWeaponImpliedData;
    public ImpliedItemData EquipedWeaponImpliedData
    {
        get { return equipedWeaponImpliedData; }
        set { equipedWeaponImpliedData = value; }
    }

    private AccesorieData equipedRing;
    public AccesorieData EquipedRing
    {
        get { return equipedRing; }
        set { equipedRing = value; }
    }
    private ImpliedItemData equipedRingImpliedData;
    public ImpliedItemData EquipedRingImpliedData
    {
        get { return equipedRingImpliedData; }
        set { equipedRingImpliedData = value; }
    }

    private AccesorieData equipedNecklace;
    public AccesorieData EquipedNecklace
    {
        get { return equipedNecklace; }
        set { equipedNecklace = value; }
    }
    private ImpliedItemData equipedNecklaceImpliedData;
    public ImpliedItemData EquipedNecklaceImpliedData
    {
        get { return equipedNecklaceImpliedData; }
        set { equipedNecklaceImpliedData = value; }
    }

    private void Start()
    {
        equipedWeaponImpliedData = UserEquipmentProvider.Instance.WeaponItem;
        equipedRingImpliedData = UserEquipmentProvider.Instance.Accesorie_Ring;
        equipedNecklaceImpliedData = UserEquipmentProvider.Instance.Accesorie_Necklace;

        equipedWeapon = ItemDB.Instance.GetWeaponData(equipedWeaponImpliedData.ItemCode);
        equipedRing = ItemDB.Instance.GetAccesorieData(equipedRingImpliedData.ItemCode);
        equipedNecklace = ItemDB.Instance.GetAccesorieData(equipedNecklaceImpliedData.ItemCode);
    }

    // UnEquip
    public void UnequipWeapon()
    {
        equipedWeapon = null;
    }
    public void UnequipAccesorie_Ring()
    {
        equipedRing = null;
    }
    public void UnequipAccesorie_Necklace()
    {
        equipedNecklace = null;
    }

    // Equip
    public void EquipWeapon(int itemCode)
    {
        WeaponData weaponData = ItemDB.Instance.GetWeaponData(itemCode);
        equipedWeapon = weaponData;
    }
    public void EquipAccesorie_Ring(AccesorieData data)
    {
        equipedRing = data;
    }
    public void EquipAccesorie_Necklace(AccesorieData data)
    {
        equipedNecklace = data;
    }
}
