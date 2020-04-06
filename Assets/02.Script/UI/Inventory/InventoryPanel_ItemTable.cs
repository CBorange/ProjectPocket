using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel_ItemTable : MonoBehaviour
{
    // Panel
    public InventoryPanel inventoryPanel;

    // UI : ItemToggle
    public ToggleGroup contentsView;
    public GameObject selectTogglePrefab;
    private List<ItemSelectToggle> selectTogglePool;

    // Data
    private InventoryCategory inventoryCategory;
    private List<ImpliedItemData> currentItemDatas;
    private ItemSelectToggle selectedItemToggle;

    // 외부호출 Method
    public void Initialize()
    {
        selectTogglePool = new List<ItemSelectToggle>();
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
        {
            selectTogglePool[i].GetComponent<Toggle>().isOn = false;
        }
    }
    public void RefreshInventoryPanel()
    {
        if (!inventoryPanel.HasSelectedEquipment())
            inventoryPanel.ResetItemInteractPanel();
        for (int i = 0; i < selectTogglePool.Count; ++i)
        {
            selectTogglePool[i].gameObject.SetActive(false);
        }

        switch (inventoryCategory)
        {
            case InventoryCategory.Weapon:
                currentItemDatas = PlayerInventory.Instance.ImpliedWeaponDataInfos;
                break;
            case InventoryCategory.Accesorie:
                currentItemDatas = PlayerInventory.Instance.ImpliedAccesorieDataInfos;
                break;
            case InventoryCategory.Expendable:
                currentItemDatas = PlayerInventory.Instance.ImpliedExpendableDataInfos;
                break;
            case InventoryCategory.Etc:
                currentItemDatas = PlayerInventory.Instance.ImpliedEtcDataInfos;
                break;
        }
        LoadItemToggle();
    }
    public void AttachItemToQuickSlot()
    {

    }
    public void UseItem()
    {
        switch(selectedItemToggle.CurrentItemImpliedData.ItemType)
        {
            case "Weapon":
                PlayerEquipment.Instance.EquipWeapon(selectedItemToggle.CurrentItemImpliedData.ItemCode);
                break;
            case "Accesorie":
                AccesorieData accesorieData = ItemDB.Instance.GetAccesorieData(selectedItemToggle.CurrentItemImpliedData.ItemCode);

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

            ItemSelectToggle itemSelectToggle = newSelectToggle.GetComponent<ItemSelectToggle>();
            itemSelectToggle.Initialize(ItemSelected);

            selectTogglePool.Add(itemSelectToggle);
            newSelectToggle.SetActive(false);
        }
    }
    private void LoadItemToggle()
    {
        for (int i = 0; i < currentItemDatas.Count; ++i)
        {
            if (currentItemDatas[i].ItemType.Equals("Weapon"))
            {
                if (PlayerEquipment.Instance.EquipedWeapon != null &&
                    PlayerEquipment.Instance.EquipedWeapon.ItemCode == currentItemDatas[i].ItemCode)
                    selectTogglePool[i].Refresh(currentItemDatas[i], true);
                else
                    selectTogglePool[i].Refresh(currentItemDatas[i], false);
            }
            else if (currentItemDatas[i].ItemType.Equals("Accesorie"))
            {
                AccesorieData accesorie = ItemDB.Instance.GetAccesorieData(currentItemDatas[i].ItemCode);
                switch(accesorie.AccesorieType)
                {
                    case "Ring":
                        if (PlayerEquipment.Instance.EquipedRing != null &&
                            PlayerEquipment.Instance.EquipedRing.ItemCode == currentItemDatas[i].ItemCode)
                            selectTogglePool[i].Refresh(currentItemDatas[i], true);
                        else
                            selectTogglePool[i].Refresh(currentItemDatas[i], false);
                        break;
                    case "Necklace":
                        if (PlayerEquipment.Instance.EquipedNecklace != null &&
                            PlayerEquipment.Instance.EquipedNecklace.ItemCode == currentItemDatas[i].ItemCode)
                            selectTogglePool[i].Refresh(currentItemDatas[i], true);
                        else
                            selectTogglePool[i].Refresh(currentItemDatas[i], false);
                        break;
                }
            }
            else
                selectTogglePool[i].Refresh(currentItemDatas[i], false);
        }
    }

    // 아이템 토글이 선택되었을 시 Toggle->Panel로 호출되는 Callback
    private void ItemSelected(ItemSelectToggle selectToggle, string itemType)
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

        inventoryPanel.RefreshItemIntroduce(selectToggle.CurrentItem.Name, selectToggle.CurrentItem.Introduce);
    }
}
