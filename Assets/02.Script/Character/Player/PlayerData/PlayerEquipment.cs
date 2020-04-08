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
    private ImpliedItemData equipedWeaponImpliedData;
    public ImpliedItemData EquipedWeaponImpliedData
    {
        get { return equipedWeaponImpliedData; }
    }

    private AccesorieData equipedRing;
    public AccesorieData EquipedRing
    {
        get { return equipedRing; }
    }
    private ImpliedItemData equipedRingImpliedData;
    public ImpliedItemData EquipedRingImpliedData
    {
        get { return equipedRingImpliedData; }
    }

    private AccesorieData equipedNecklace;
    public AccesorieData EquipedNecklace
    {
        get { return equipedNecklace; }
    }
    private ImpliedItemData equipedNecklaceImpliedData;
    public ImpliedItemData EquipedNecklaceImpliedData
    {
        get { return equipedNecklaceImpliedData; }
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
        PlayerStat.Instance.RemoveChangeAP(equipedWeapon.ItemCode);
        equipedWeapon = null;
        equipedWeaponImpliedData = null;
    }
    public void UnequipAccesorie_Ring()
    {
        equipedRing = null;
        equipedRingImpliedData = null;
    }
    public void UnequipAccesorie_Necklace()
    {
        equipedNecklace = null;
        equipedNecklaceImpliedData = null;
    }

    // Equip
    public void EquipWeapon(int itemCode)
    {
        if (equipedWeapon != null)
            UnequipWeapon();

        WeaponData weaponData = ItemDB.Instance.GetWeaponData(itemCode);
        equipedWeapon = weaponData;
        equipedWeaponImpliedData = new ImpliedItemData("Weapon", itemCode, 1);

        PlayerActManager.Instance.EquipWeapon(equipedWeapon);
        PlayerStat.Instance.AddChangeAP(equipedWeapon.ItemCode, equipedWeapon.AttackPoint);
    }
    public void EquipAccesorie_Ring(AccesorieData data)
    {
        equipedRing = data;
        equipedRingImpliedData = new ImpliedItemData("Accesorie", data.ItemCode, 1);
    }
    public void EquipAccesorie_Necklace(AccesorieData data)
    {
        equipedNecklace = data;
        equipedNecklaceImpliedData = new ImpliedItemData("Accesorie", data.ItemCode, 1);
    }
}
