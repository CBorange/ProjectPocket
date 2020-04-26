using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel_ItemTable : MonoBehaviour
{
    // Controller
    public InventoryPanel inventoryPanel;

    // UI : ItemToggle
    public ToggleGroup contentsView;
    public GameObject selectTogglePrefab;
    private List<InventoryItemSelectToggle> selectTogglePool;

    // Data
    private InventoryCategory inventoryCategory;
    private List<InventoryItem> currentItems;
    private InventoryItemSelectToggle selectedItemToggle;

    // 외부호출 Method
    public void Initialize()
    {
        currentItems = new List<InventoryItem>();
        selectTogglePool = new List<InventoryItemSelectToggle>();
        selectTogglePool.Capacity = 0;
        CreateSelectTogglePool(30);
    }
    public void Change_ItemCategory(InventoryCategory category)
    {
        inventoryCategory = category;
        RefreshInventoryPanel();
    }
    public void DeselectAllToggle()
    {
        contentsView.allowSwitchOff = true;
        for (int i = 0; i < selectTogglePool.Count; ++i)
            selectTogglePool[i].GetComponent<Toggle>().isOn = false;
    }
    public void RefreshInventoryPanel()
    {
        if (!inventoryPanel.HasSelectedEquipment())
        {
            inventoryPanel.ResetItemInteractPanel();
            DeselectAllToggle();
        }
        for (int i = 0; i < selectTogglePool.Count; ++i)
        {
            selectTogglePool[i].gameObject.SetActive(false);
        }

        currentItems.Clear();
        switch (inventoryCategory)
        {
            case InventoryCategory.Weapon:
                foreach (var kvp in PlayerInventory.Instance.WeaponItems)
                    currentItems.Add(kvp.Value);
                break;
            case InventoryCategory.Accesorie:
                foreach (var kvp in PlayerInventory.Instance.AccesorieItems)
                    currentItems.Add(kvp.Value);
                break;
            case InventoryCategory.Expendable:
                foreach (var kvp in PlayerInventory.Instance.ExpendableItems)
                    currentItems.Add(kvp.Value);
                break;
            case InventoryCategory.Etc:
                foreach (var kvp in PlayerInventory.Instance.EtcItems)
                    currentItems.Add(kvp.Value);
                break;
        }
        LoadItemToggle();
    }
    public void AttachItemToQuickSlot()
    {

    }
    public void UseItem()
    {
        switch(selectedItemToggle.CurrentItem.OriginalItemData.ItemType)
        {
            case "Weapon":
                PlayerEquipment.Instance.EquipWeapon(selectedItemToggle.CurrentItem.OriginalItemData.ItemCode);
                break;
            case "Accesorie":
                AccesorieData accesorieData = ItemDB.Instance.GetAccesorieData(selectedItemToggle.CurrentItem.OriginalItemData.ItemCode);

                if (accesorieData.AccesorieType.Equals("Ring"))
                    PlayerEquipment.Instance.EquipAccesorie_Ring(accesorieData);
                else if (accesorieData.AccesorieType.Equals("Necklace"))
                    PlayerEquipment.Instance.EquipAccesorie_Necklace(accesorieData);
                break;
        }
        inventoryPanel.RefreshPlayerInfoPanel();
        RefreshInventoryPanel();
    }

    // 내부 Method
    private void CreateSelectTogglePool(int createCount)
    {
        selectTogglePool.Capacity += createCount;
        for (int i = 0; i < createCount; ++i)
        {
            GameObject newSelectToggle = Instantiate(selectTogglePrefab, contentsView.transform);
            newSelectToggle.GetComponent<Toggle>().group = contentsView.GetComponent<ToggleGroup>();

            InventoryItemSelectToggle itemSelectToggle = newSelectToggle.GetComponent<InventoryItemSelectToggle>();
            itemSelectToggle.Initialize(ItemSelected);

            selectTogglePool.Add(itemSelectToggle);
            newSelectToggle.SetActive(false);
        }
    }
    private void LoadItemToggle()
    {
        for (int i = 0; i < currentItems.Count; ++i)
        {
            ItemData data = currentItems[i].OriginalItemData;
            if (data.ItemType.Equals("Weapon"))
            {
                if (PlayerEquipment.Instance.EquipedWeapon != null &&
                    PlayerEquipment.Instance.EquipedWeapon.ItemCode == data.ItemCode)
                    selectTogglePool[i].Refresh(currentItems[i], true);
                else
                    selectTogglePool[i].Refresh(currentItems[i], false);
            }
            else if (data.ItemType.Equals("Accesorie"))
            {
                AccesorieData accesorie = ItemDB.Instance.GetAccesorieData(data.ItemCode);
                switch(accesorie.AccesorieType)
                {
                    case "Ring":
                        if (PlayerEquipment.Instance.EquipedRing != null &&
                            PlayerEquipment.Instance.EquipedRing.ItemCode == data.ItemCode)
                            selectTogglePool[i].Refresh(currentItems[i], true);
                        else
                            selectTogglePool[i].Refresh(currentItems[i], false);
                        break;
                    case "Necklace":
                        if (PlayerEquipment.Instance.EquipedNecklace != null &&
                            PlayerEquipment.Instance.EquipedNecklace.ItemCode == data.ItemCode)
                            selectTogglePool[i].Refresh(currentItems[i], true);
                        else
                            selectTogglePool[i].Refresh(currentItems[i], false);
                        break;
                }
            }
            else
                selectTogglePool[i].Refresh(currentItems[i], false);
        }
    }

    // 아이템 토글이 선택되었을 시 Toggle->Panel로 호출되는 Callback
    private void ItemSelected(InventoryItemSelectToggle selectToggle, string itemType)
    {
        contentsView.allowSwitchOff = false;
        inventoryPanel.DeselectAllEquipmentToggles();
        inventoryPanel.ResetItemInteractPanel();
        selectedItemToggle = selectToggle;

        if (itemType.Equals("Weapon") || itemType.Equals("Expendable"))
        {
            inventoryPanel.ActiveQuickSlotBtn();
            inventoryPanel.ActiveUseItemBtn();
        }
        else if (itemType.Equals("Accesorie"))
            inventoryPanel.ActiveUseItemBtn();

        inventoryPanel.RefreshItemIntroduce(selectToggle.CurrentItem.OriginalItemData.Name,
            selectToggle.CurrentItem.OriginalItemData.Introduce);
    }
}
