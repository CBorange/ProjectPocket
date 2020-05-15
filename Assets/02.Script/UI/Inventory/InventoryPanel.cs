﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum InventoryCategory
{
    Weapon,
    Accesorie,
    Expendable,
    Etc
};
public class InventoryPanel : MonoBehaviour
{
    // Inventory UI 기능 단위
    public InventoryPanel_ItemTable itemTable;
    public InventoryPanel_PlayerInfo playerInfo;

    // UI
    public Toggle[] itemCategoryToggles;

    // UI : ItemIntroduce
    public Transform ItemStatPanelGroup;
    public GameObject InventoryItemStatPrefab;
    private InventoryItemStat[] ItemStatPanels;

    public Text selectedItemName;
    public Text selectedItemIntroduce;
    public Button attachToQuickSlot_Btn;
    public Button useItem_Btn;
    public Button unequip_Btn;

    // Data
    private int beforeCategoryType = -1;
    public void Initialize()
    {
        CreateItemStatPanelPool();
        itemTable.Initialize();
        playerInfo.Initialize();
    }
    // Open/Close Panel
    public void OpenPanel()
    {
        if (PlayerActManager.Instance.CurrentBehaviour == CharacterBehaviour.Gathering)
            return;
        // UI 초기화
        gameObject.SetActive(true);

        itemCategoryToggles[0].isOn = true;
        RefreshPlayerInfoPanel();
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }


    // Methods For Transmit Between ItemTable <-> EquipmentPanel
    public void RefreshPlayerInfoPanel()
    {
        playerInfo.RefreshPlayerInfoPanel();
    }
    public void RefreshItemTable()
    {
        itemTable.RefreshInventoryPanel();
    }
    public bool HasSelectedEquipment()
    {
        return playerInfo.HasSelectedEquipment();
    }
    public void DeselectAllItemTableToggles()
    {
        itemTable.DeselectAllToggle();
    }
    public void DeselectAllEquipmentToggles()
    {
        playerInfo.DeselectAllToggle();
    }

    // Item Interact Panel Config
    private void CreateItemStatPanelPool()
    {
        ItemStatPanels = new InventoryItemStat[12];
        for (int i = 0; i < 12; ++i)
        {
            GameObject newPanel = Instantiate(InventoryItemStatPrefab);
            newPanel.transform.parent = ItemStatPanelGroup;
            ItemStatPanels[i] = newPanel.GetComponent<InventoryItemStat>();
            newPanel.gameObject.SetActive(false);
        }
    }
    public void ResetItemInteractPanel()
    {
        for (int i = 0; i < 12; ++i)
            ItemStatPanels[i].gameObject.SetActive(false);
        selectedItemName.text = "";
        selectedItemIntroduce.text = "";

        attachToQuickSlot_Btn.gameObject.SetActive(false);
        useItem_Btn.gameObject.SetActive(false);
        unequip_Btn.gameObject.SetActive(false);
    }
    private void RefreshDefaultItemInfo(string itemName, string itemIntroduce)
    {
        for (int i = 0; i < 12; ++i)
            ItemStatPanels[i].gameObject.SetActive(false);
        selectedItemName.text = itemName;
        string linebreakText = itemIntroduce.Replace(';', '\n');
        selectedItemIntroduce.text = linebreakText;
    }
    public void RefreshItemIntroduce(string itemName, string itemIntroduce)
    {
        RefreshDefaultItemInfo(itemName, itemIntroduce);
    }
    public void RefreshItemIntroduce(string itemName, string itemIntroduce, StatAdditional[] itemStats) 
    {
        RefreshDefaultItemInfo(itemName, itemIntroduce);
        for (int i = 0; i < itemStats.Length; ++i)
            ItemStatPanels[i].Refresh(UIText_Util.Instance.GetKorStatByEng(itemStats[i].StatName), itemStats[i].StatValue.ToString());
    }
    public void RefreshItemIntroduce(string itemName, string itemIntroduce, ExpendableEffect[] effects)
    {
        RefreshDefaultItemInfo(itemName, itemIntroduce);
        for (int i = 0; i < effects.Length; ++i)
            ItemStatPanels[i].Refresh(UIText_Util.Instance.GetKorStatByEng(effects[i].StatName), effects[i].StatAmount.ToString());
    }

    // Active Button Method
    public void ActiveQuickSlotBtn()
    {
        attachToQuickSlot_Btn.gameObject.SetActive(true);
    }
    public void ActiveUseItemBtn()
    {
        useItem_Btn.gameObject.gameObject.SetActive(true);
    }
    public void ActiveUnEquipBtn()
    {
        unequip_Btn.gameObject.SetActive(true);
    }

    // 인벤토리 카테고리 변경 토글 Property Changed Callback
    public void ChangeCategory(int type)
    {
        if (beforeCategoryType == type)
            return;
        beforeCategoryType = type;
        itemTable.Change_ItemCategory((InventoryCategory)type);
    }
}
