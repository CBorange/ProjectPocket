using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel_ItemTable : MonoBehaviour
{
    // UI : ItemToggle
    public Transform contentsView;
    public GameObject selectTogglePrefab;
    private List<ItemSelectToggle> selectTogglePool;

    // UI : ItemIntroduce
    public Text selectedItemName;
    public Text selectedItemIntroduce;
    public Button attachToQuickSlot_Btn;
    public Button useItem_Btn;

    // Data
    private InventoryCategory inventoryCategory;
    private List<ImpliedItemData> currentItemDatas;
    public PlayerInventory playerInventory;

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

    // 내부 Method
    private void CreateSelectTogglePool(int createCount)
    {
        int beforeCapacity = selectTogglePool.Capacity;
        selectTogglePool.Capacity += createCount;
        for (int i = 0; i < createCount; ++i)
        {
            GameObject newSelectToggle = Instantiate(selectTogglePrefab, contentsView);
            newSelectToggle.GetComponent<Toggle>().group = contentsView.GetComponent<ToggleGroup>();

            ItemSelectToggle itemSelectToggle = newSelectToggle.GetComponent<ItemSelectToggle>();
            itemSelectToggle.Initialize(ItemSelected);

            selectTogglePool.Add(itemSelectToggle);
            newSelectToggle.SetActive(false);
        }
    }
    private void RefreshInventoryPanel()
    {
        ResetItemInteractPanel();
        for (int i = 0; i < selectTogglePool.Count; ++i)
        {
            selectTogglePool[i].gameObject.SetActive(false);
        }

        switch (inventoryCategory)
        {
            case InventoryCategory.Weapon:
                currentItemDatas = playerInventory.ImpliedWeaponDataInfos;
                break;
            case InventoryCategory.Accesorie:
                currentItemDatas = playerInventory.ImpliedAccesorieDataInfos;
                break;
            case InventoryCategory.Expendable:
                currentItemDatas = playerInventory.ImpliedExpendableDataInfos;
                break;
            case InventoryCategory.Etc:
                currentItemDatas = playerInventory.ImpliedEtcDataInfos;
                break;
        }
        LoadItemToggle();
    }
    private void ResetItemInteractPanel()
    {
        selectedItemName.text = "아이템 이름";
        selectedItemIntroduce.text = "아이템 설명";
        attachToQuickSlot_Btn.gameObject.SetActive(false);
        useItem_Btn.gameObject.SetActive(false);
    }
    private void LoadItemToggle()
    {
        for (int i = 0; i < currentItemDatas.Count; ++i)
        {
            selectTogglePool[i].Refresh(currentItemDatas[i]);
        }
        if (currentItemDatas.Count > 0)
        {
            selectTogglePool[0].ItemSelected(true);
        }
    }

    // 아이템 토글이 선택되었을 시 Toggle->Panel로 호출되는 Callback
    private void ItemSelected(ItemData selectedItem)
    {
        selectedItemName.gameObject.SetActive(true);
        selectedItemIntroduce.gameObject.SetActive(true);

        if (inventoryCategory == InventoryCategory.Weapon ||
            inventoryCategory == InventoryCategory.Accesorie)
        {
            attachToQuickSlot_Btn.gameObject.SetActive(true);
            useItem_Btn.gameObject.gameObject.SetActive(true);
        }
        selectedItemName.text = selectedItem.Name;
        selectedItemIntroduce.text = selectedItem.Introduce;
    }
}
