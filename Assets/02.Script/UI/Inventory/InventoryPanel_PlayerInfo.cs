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
    public EquipmentSelectToggle WeaponToggle;
    public EquipmentSelectToggle RingToggle;
    public EquipmentSelectToggle NecklaceToggle;

    // Data
    private EquipmentSelectToggle currentToggle;

    public void Initialize()
    {
        WeaponToggle.Initialize(EquipmentSelected);
        RingToggle.Initialize(EquipmentSelected);
        NecklaceToggle.Initialize(EquipmentSelected);
    }

    public void RefreshPlayerInfoPanel()
    {
        // PlayerInfo Refresh
        AccountName.text = UserInfoProvider.Instance.UserAccount;
        PlayerLevel.text = $"LV. <color=cyan>{PlayerStat.Instance.Level.ToString()}</color>";

        //Apply Data To UI
        WeaponToggle.Refresh(PlayerEquipment.Instance.EquipedWeaponImpliedData);
        RingToggle.Refresh(PlayerEquipment.Instance.EquipedRingImpliedData);
        NecklaceToggle.Refresh(PlayerEquipment.Instance.EquipedNecklaceImpliedData);
    }
    public void DeselectAllToggle()
    {
        EquipmentPanel.allowSwitchOff = true;
        currentToggle = null;
        WeaponToggle.GetComponent<Toggle>().isOn = false;
        RingToggle.GetComponent<Toggle>().isOn = false;
        NecklaceToggle.GetComponent<Toggle>().isOn = false;
    }
    public void UnequipItem()
    {
        string itmeType = currentToggle.CurrentItemImpliedData.ItemType;
        switch(itmeType)
        {
            case "Weapon":
                PlayerEquipment.Instance.UnequipWeapon();
                break;
            case "Accesorie":
                AccesorieData accesorie = ItemDB.Instance.GetAccesorieData(currentToggle.CurrentItem.ItemCode);
                switch(accesorie.AccesorieType)
                {
                    case "Ring":
                        PlayerEquipment.Instance.UnequipAccesorie_Ring();
                        break;
                    case "Necklace":
                        PlayerEquipment.Instance.UnequipAccesorie_Necklace();
                        break;
                }
                break;
        }
        RefreshPlayerInfoPanel();
        inventoryPanel.RefreshItemTable();
    }
    public bool HasSelectedEquipment()
    {
        if (currentToggle == null)
            return false;
        return true;
    }

    private void EquipmentSelected(EquipmentSelectToggle selectToggle)
    {
        EquipmentPanel.allowSwitchOff = false;
        currentToggle = selectToggle;
        inventoryPanel.DeselectAllItemTableToggles();
        inventoryPanel.ResetItemInteractPanel();
        inventoryPanel.RefreshItemIntroduce(selectToggle.CurrentItem.Name, selectToggle.CurrentItem.Introduce);

        inventoryPanel.ActiveUnEquipBtn();
    }
}
