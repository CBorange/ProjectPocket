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

    // UnEquip
    public void UnequipWeapon()
    {
        PlayerActManager.Instance.UnEquipWeapon();
        ItemStat[] weaponStats = equipedWeapon.WeaponStat;
        for (int statIdx = 0; statIdx < weaponStats.Length; ++statIdx)
        {
            PlayerStat.Instance.RemoveChangeableStat(weaponStats[statIdx].StatName, equipedWeapon.ItemCode);
        }
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
        if (equipedWeapon != null)
            UnequipWeapon();

        WeaponData weaponData = ItemDB.Instance.GetWeaponData(itemCode);
        equipedWeapon = weaponData;

        PlayerActManager.Instance.EquipWeapon(equipedWeapon);
        ItemStat[] weaponStats = equipedWeapon.WeaponStat;
        for (int statIdx = 0; statIdx < weaponStats.Length; ++statIdx)
        {
            PlayerStat.Instance.AddChangeableStat(weaponStats[statIdx].StatName, equipedWeapon.ItemCode, weaponStats[statIdx].StatValue);
        }
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
