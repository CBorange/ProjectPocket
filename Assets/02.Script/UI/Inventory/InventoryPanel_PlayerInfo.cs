using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel_PlayerInfo : MonoBehaviour
{
    // UI
    public Text AccountName;
    public Text PlayerLevel;
    public ToggleGroup EquipmentPanel;
    public InventoryPanel inventoryPanel;

    // Toggle
    public ItemSelectToggle WeaponEquipmentToggle;
    public ItemSelectToggle RingEquipmentToggle;
    public ItemSelectToggle NecklaceEquipmentToggle;

    public void Initialize()
    {
        WeaponEquipmentToggle.Initialize(EquipmentSelected);
        RingEquipmentToggle.Initialize(EquipmentSelected);
        NecklaceEquipmentToggle.Initialize(EquipmentSelected);
    }

    public void RefreshPlayerInfoPanel()
    {
        // PlayerInfo Refresh
        AccountName.text = UserInfoProvider.Instance.UserAccount;
        PlayerLevel.text = PlayerStat.Instance.Level.ToString();

        //Apply Data To UI
        WeaponEquipmentToggle.Refresh(PlayerEquipment.Instance.EquipedWeaponImpliedData);
        RingEquipmentToggle.Refresh(PlayerEquipment.Instance.EquipedRingImpliedData);
        NecklaceEquipmentToggle.Refresh(PlayerEquipment.Instance.EquipedNecklaceImpliedData);
    }
    public void DeselectAllToggle()
    {
        WeaponEquipmentToggle.GetComponent<Toggle>().isOn = false;
        RingEquipmentToggle.GetComponent<Toggle>().isOn = false;
        NecklaceEquipmentToggle.GetComponent<Toggle>().isOn = false;
    }

    private void EquipmentSelected(ItemSelectToggle selectToggle, string itemType)
    {
        inventoryPanel.DeselectAllItemTableToggles();
        inventoryPanel.ResetItemInteractPanel();
        inventoryPanel.RefreshItemIntroduce(selectToggle.CurrentItem.Name, selectToggle.CurrentItem.Introduce);
        inventoryPanel.ActiveUseItemBtn(() =>
        {
            switch(itemType)
            {
                case "Weapon":
                    PlayerEquipment.Instance.EquipWeapon(selectToggle.CurrentItemImpliedData.ItemCode);
                    break;
                case "Accesorie":
                    AccesorieData data = ItemDB.Instance.GetAccesorieData(selectToggle.CurrentItemImpliedData.ItemCode);
                    if (data.AccesorieType.Equals("Ring"))
                        PlayerEquipment.Instance.EquipAccesorie_Ring(data);
                    else if (data.AccesorieType.Equals("Necklace"))
                        PlayerEquipment.Instance.EquipAccesorie_Necklace(data);
                    break;
            }
        });
    }
}
