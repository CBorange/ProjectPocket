using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum InventoryCategory
{
    Weapon,
    Accesorie,
    Expandable,
    Etc
};
public class InventoryPanel : MonoBehaviour
{
    // UI
    public Transform contentsView;
    public GameObject selectTogglePrefab;
    private List<ItemSelectToggle> selectTogglePool;

    // Data
    private InventoryCategory inventoryCategory;
    public PlayerInventory playerInventory;

    private void Start()
    {
        selectTogglePool = new List<ItemSelectToggle>();
        selectTogglePool.Capacity = 0;
        CreateSelectTogglePool(30);
    }
    private void CreateSelectTogglePool(int createCount)
    {
        int beforeCapacity = selectTogglePool.Capacity;
        selectTogglePool.Capacity += createCount;
        for (int i = 0; i < createCount; ++i)
        {
            GameObject newSelectToggle = Instantiate(selectTogglePrefab, contentsView);
            newSelectToggle.GetComponent<Toggle>().group = contentsView.GetComponent<ToggleGroup>();

            ItemSelectToggle itemSelectToggle = newSelectToggle.GetComponent<ItemSelectToggle>();
            itemSelectToggle.IndexInContentView = beforeCapacity + i;
            selectTogglePool.Add(newSelectToggle.GetComponent<ItemSelectToggle>());
            newSelectToggle.SetActive(false);
        }
    }
    private void RefreshInventoryPanel()
    {
        switch(inventoryCategory)
        {
            case InventoryCategory.Weapon:
                break;
            case InventoryCategory.Accesorie:
                break;
            case InventoryCategory.Expandable:
                break;
            case InventoryCategory.Etc:
                break;
        }
    }
    public void OpenInventoryPanel()
    {
        gameObject.SetActive(true);
        RefreshInventoryPanel();
    }

    // Inventory Category Change Callback
    public void ChangeCategoryToWeapon(bool selected)
    {
        if (!selected)
            return;
        inventoryCategory = InventoryCategory.Weapon;
    }
    public void ChangeCategoryToAccesorie(bool selected)
    {
        if (!selected)
            return;
        inventoryCategory = InventoryCategory.Accesorie;
    }
    public void ChangeCategoryToExpandable(bool selected)
    {
        if (!selected)
            return;
        inventoryCategory = InventoryCategory.Expandable;
    }
    public void ChangeCategoryToEtc(bool selected)
    {
        if (!selected)
            return;
        inventoryCategory = InventoryCategory.Etc;
    }
}
