using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserEquipmentProvider
{
    // Singleton
    private UserEquipmentProvider() { }
    private static UserEquipmentProvider instance;
    public static UserEquipmentProvider Instance
    {
        get
        {
            if (instance == null)
                instance = new UserEquipmentProvider();
            return instance;
        }
    }

    private WeaponData weaponItem;
    public WeaponData WeaponItem
    {
        get { return weaponItem; }
    }
    private AccesorieData accesorie_Ring;
    public AccesorieData Accesorie_Ring
    {
        get { return accesorie_Ring; }
    }
    private AccesorieData accesorie_Necklace;
    public AccesorieData Accesorie_Necklace
    {
        get { return accesorie_Necklace; }
    }
    public void Initialize(WeaponData weaponData, AccesorieData ringData, AccesorieData necklaceData)
    {
        weaponItem = weaponData;
        accesorie_Ring = ringData;
        accesorie_Necklace = necklaceData;
    }
}
