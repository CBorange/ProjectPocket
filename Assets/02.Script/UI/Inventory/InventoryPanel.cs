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
public class InventoryPanel : MonoBehaviour, UIPanel
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

    public void Initialize()
    {
        itemTable.Initialize();
        playerInfo.Initialize();
    }
    // Open/Close Panel
    public void OpenPanel()
    {
        // UI 초기화
        gameObject.SetActive(true);

        itemCategoryToggles[0].isOn = true;
        ChangeCategoryToWeapon(true);
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
    }
    public void RefreshItemIntroduce(string itemName, string itemIntroduce)
    {
        selectedItemName.text = itemName;
        string linebreakText = itemIntroduce.Replace(';', '\n');
        selectedItemIntroduce.text = linebreakText;
    }
    public void ActiveQuickSlotBtn(Action attachQuickslotFunction)
    {
        attachToQuickSlot_Btn.gameObject.SetActive(true);

        attachToQuickSlot_Btn.onClick.RemoveAllListeners();
        attachToQuickSlot_Btn.onClick.AddListener(new UnityEngine.Events.UnityAction(attachQuickslotFunction));
    }
    public void ActiveUseItemBtn(Action useItemFunction)
    {
        useItem_Btn.gameObject.gameObject.SetActive(true);

        useItem_Btn.onClick.RemoveAllListeners();
        useItem_Btn.onClick.AddListener(new UnityEngine.Events.UnityAction(useItemFunction));
    }

    // 인벤토리 카테고리 변경 토글 Property Changed Callback
    public void ChangeCategoryToWeapon(bool selected)
    {
        if (!selected)
            return;
        itemTable.Change_ItemCategory(InventoryCategory.Weapon);
    }
    public void ChangeCategoryToAccesorie(bool selected)
    {
        if (!selected)
            return;
        itemTable.Change_ItemCategory(InventoryCategory.Accesorie);
    }
    public void ChangeCategoryToExpendable(bool selected)
    {
        if (!selected)
            return;
        itemTable.Change_ItemCategory(InventoryCategory.Expendable);
    }
    public void ChangeCategoryToEtc(bool selected)
    {
        if (!selected)
            return;
        itemTable.Change_ItemCategory(InventoryCategory.Etc);
    }
}
