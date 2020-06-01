using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour, PlayerRuntimeData
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
    }
    private AccesorieData equipedRing;
    public AccesorieData EquipedRing
    {
        get { return equipedRing; }
    }
    private AccesorieData equipedNecklace;
    public AccesorieData EquipedNecklace
    {
        get { return equipedNecklace; }
    }

    public void Initialize()
    {
        if (UserEquipmentProvider.Instance.WeaponItem != null)
            EquipWeapon(UserEquipmentProvider.Instance.WeaponItem.ItemCode);
        if (UserEquipmentProvider.Instance.Accesorie_Ring != null)
            EquipAccesorie_Ring(ItemDB.Instance.GetAccesorieData(UserEquipmentProvider.Instance.Accesorie_Ring.ItemCode));
        if (UserEquipmentProvider.Instance.Accesorie_Necklace != null)
            EquipAccesorie_Necklace(ItemDB.Instance.GetAccesorieData(UserEquipmentProvider.Instance.Accesorie_Necklace.ItemCode));
    }
    public bool HasEquipedItem(int itemCode)
    {
        if (equipedWeapon != null && equipedWeapon.ItemCode == itemCode) 
            return true;
        if (equipedRing != null && equipedRing.ItemCode == itemCode)
            return true;
        if (equipedNecklace != null && equipedNecklace.ItemCode == itemCode)
            return true;
        return false;
    }

    // UnEquip
    public void UnequipWeapon()
    {
        PlayerActManager.Instance.UnEquipWeapon();
        RemoveItemStat(equipedWeapon.WeaponStat, equipedWeapon.ItemCode);
        equipedWeapon = null;
    }
    public void UnequipAccesorie_Ring()
    {
        RemoveItemStat(equipedRing.AccesorieStat, equipedRing.ItemCode);
        equipedRing = null;
    }
    public void UnequipAccesorie_Necklace()
    {
        RemoveItemStat(equipedNecklace.AccesorieStat, equipedNecklace.ItemCode);
        equipedNecklace = null;
    }

    // Equip
    private void ApplyItemStat(StatAdditional[] stat, int itemCode)
    {
        for (int i = 0; i < stat.Length; ++i)
        {
            PlayerStat.Instance.AddChangeableStat(stat[i].StatName, itemCode, stat[i].StatValue);
        }
    }
    private void RemoveItemStat(StatAdditional[] stat, int itemCode)
    {
        for (int i = 0; i < stat.Length; ++i)
        {
            PlayerStat.Instance.RemoveChangeableStat(stat[i].StatName, itemCode);
        }
    }
    public void EquipWeapon(int itemCode)
    {
        if (equipedWeapon != null)
            UnequipWeapon();

        WeaponData weaponData = ItemDB.Instance.GetWeaponData(itemCode);
        equipedWeapon = weaponData;

        PlayerActManager.Instance.EquipWeapon(equipedWeapon);
        ApplyItemStat(equipedWeapon.WeaponStat, equipedWeapon.ItemCode);
    }
    public void EquipAccesorie_Ring(AccesorieData data)
    {
        if (equipedRing != null)
            UnequipAccesorie_Ring();
        equipedRing = data;
        ApplyItemStat(equipedRing.AccesorieStat, equipedRing.ItemCode);
    }
    public void EquipAccesorie_Necklace(AccesorieData data)
    {
        if (equipedNecklace != null)
            UnequipAccesorie_Necklace();
        equipedNecklace = data;
        ApplyItemStat(equipedNecklace.AccesorieStat, equipedNecklace.ItemCode);
    }
}
