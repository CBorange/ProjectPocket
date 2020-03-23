using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    // UI
    public Toggle[] itemCategoryToggles;

    public void Initialize()
    {
        itemTable.Initialize();
    }
    public void OpenInventoryPanel()
    {
        // UI 초기화
        gameObject.SetActive(true);
        itemCategoryToggles[0].isOn = true;
        ChangeCategoryToWeapon(true);
    }
    public void CloseInventoryPanel()
    {
        gameObject.SetActive(false);
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
