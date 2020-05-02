using System.Collections;
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
    public Text selectedItemName;
    public Text selectedItemIntroduce;
    public Button attachToQuickSlot_Btn;
    public Button useItem_Btn;
    public Button unequip_Btn;

    // Data
    private int beforeCategoryType = -1;
    public void Initialize()
    {
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

    // Public Method
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
    public void ResetItemInteractPanel()
    {
        selectedItemName.text = "아이템 이름";
        selectedItemIntroduce.text = "아이템 설명";

        attachToQuickSlot_Btn.gameObject.SetActive(false);
        useItem_Btn.gameObject.SetActive(false);
        unequip_Btn.gameObject.SetActive(false);
    }
    public void RefreshItemIntroduce(string itemName, string itemIntroduce)
    {
        selectedItemName.text = itemName;
        string linebreakText = itemIntroduce.Replace(';', '\n');
        selectedItemIntroduce.text = linebreakText;
    }
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
