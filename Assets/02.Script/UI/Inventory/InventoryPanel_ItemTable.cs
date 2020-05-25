using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel_ItemTable : MonoBehaviour
{
    // Controller
    public QuickSlotPanel quickSlotPanel;
    public InventoryPanel_SellItem sellItemPanel;
    public InventoryPanel inventoryPanel;

    // UI : ItemToggle
    public ToggleGroup contentsView;
    public GameObject selectTogglePrefab;
    private List<InventoryItemSelectToggle> selectTogglePool;

    // UI : ItemPrice
    public Text ItemSellPrice;

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
        ItemSellPrice.text = string.Empty;
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
        if (!inventoryPanel.HasSelectedEquipment())
        {
            inventoryPanel.ResetItemInteractPanel();
            if (currentItems.Count > 0)
            {
                selectTogglePool[0].GetComponent<Toggle>().isOn = true;
                selectTogglePool[0].ItemSelected(true);
            }
        }
    }
    public void SellItem()
    {
        if (PlayerActManager.Instance.CurrentBehaviour != CharacterBehaviour.Idle)
            return;
        sellItemPanel.OpenPanel(selectedItemToggle.CurrentItem.OriginalItemData);
    }
    public void UseItem()
    {
        if (PlayerActManager.Instance.CurrentBehaviour != CharacterBehaviour.Idle)
            return;
        ItemData data = selectedItemToggle.CurrentItem.OriginalItemData;
        switch (data.ItemType)
        {
            case "Weapon":
                PlayerEquipment.Instance.EquipWeapon(data.ItemCode);
                break;
            case "Accesorie":
                AccesorieData accesorieData = ItemDB.Instance.GetAccesorieData(data.ItemCode);

                if (accesorieData.AccesorieType.Equals("Ring"))
                    PlayerEquipment.Instance.EquipAccesorie_Ring(accesorieData);
                else if (accesorieData.AccesorieType.Equals("Necklace"))
                    PlayerEquipment.Instance.EquipAccesorie_Necklace(accesorieData);
                break;
            case "Expendable":
                ExpendableData expendableData = ItemDB.Instance.GetExpendableData(data.ItemCode);
                PlayerBuffer.Instance.ApplyStatEffectByExpendable(expendableData);
                PlayerInventory.Instance.RemoveItemFromInventory(expendableData.ItemCode, 1);
                quickSlotPanel.Refresh();
                break;
        }
        quickSlotPanel.Refresh();
        inventoryPanel.RefreshPlayerInfoPanel();
        RefreshInventoryPanel();
    }
    public void AttachItemToQuickSlot()
    {
        quickSlotPanel.OpenPanel_ModeAttach(selectedItemToggle.CurrentItem);
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

        ItemData item = selectToggle.CurrentItem.OriginalItemData;
        inventoryPanel.ActiveSellItemBtn();
        ItemSellPrice.text = $"판매 가격 : 개당 <color=yellow>{item.SellPrice}</color> 원";
        switch (itemType)
        {
            case "Weapon":
                WeaponData weapon = ItemDB.Instance.GetWeaponData(item.ItemCode);
                inventoryPanel.RefreshItemIntroduce(item.Name, item.Introduce, weapon.WeaponStat);
                inventoryPanel.ActiveQuickSlotBtn();
                inventoryPanel.ActiveUseItemBtn();
                break;
            case "Accesorie":
                AccesorieData accesorie = ItemDB.Instance.GetAccesorieData(item.ItemCode);
                inventoryPanel.RefreshItemIntroduce(item.Name, item.Introduce, accesorie.AccesorieStat);
                inventoryPanel.ActiveUseItemBtn();
                break;
            case "Expendable":
                ExpendableData expendable = ItemDB.Instance.GetExpendableData(item.ItemCode);
                inventoryPanel.RefreshItemIntroduce(item.Name, item.Introduce, expendable.Effects);
                inventoryPanel.ActiveQuickSlotBtn();
                inventoryPanel.ActiveUseItemBtn();
                break;
            case "Etc":
                inventoryPanel.RefreshItemIntroduce(item.Name, item.Introduce);
                break;
        }
    }
}
