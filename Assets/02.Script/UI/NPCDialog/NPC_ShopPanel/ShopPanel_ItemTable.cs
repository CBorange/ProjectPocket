using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel_ItemTable : MonoBehaviour
{
    // Controller
    public ShopPanel MainPanel;

    // UI
    public ToggleGroup ItemToggleGroup;
    public Text ShopTitleText;

    // Data
    public GameObject ItemTogglePrefab;
    private Dictionary<string, ShopItem[]> itemSetByType;
    private List<ShopItemToggle> itemTogglePool;
    private ShopItem[] currentItems;
    private string currentItemCategory;
    private ShopItem selectedItem;
    private ShopInfo shopInfo;

    public void Initialize()
    {
        itemSetByType = new Dictionary<string, ShopItem[]>();
        itemTogglePool = new List<ShopItemToggle>();

        CreateItemToggles(30);
    }
    public void OpenPanel(ShopInfo shopInfo)
    {
        this.shopInfo = shopInfo;
        itemSetByType.Clear();
        for (int typeIdx = 0; typeIdx < shopInfo.SalesItemTypes.Length; ++typeIdx)
        {
            switch(shopInfo.SalesItemTypes[typeIdx])
            {
                case "Weapon":
                    itemSetByType.Add("Weapon", shopInfo.WeaponItems);
                    break;
                case "Expendable":
                    itemSetByType.Add("Expendable", shopInfo.ExpendableItems);
                    break;
                case "Accesorie":
                    itemSetByType.Add("Accesorie", shopInfo.AccesorieItems);
                    break;
                case "Etc":
                    itemSetByType.Add("Etc", shopInfo.EtcItems);
                    break;
            }
        }
    }
    public void ClosePanel()
    {

    }

    private void CreateItemToggles(int count)
    {
        for (int i = 0; i < count; ++i)
        {
            GameObject newToggle = Instantiate(ItemTogglePrefab, ItemToggleGroup.transform);
            newToggle.GetComponent<Toggle>().group = ItemToggleGroup;
            ShopItemToggle shopItem = newToggle.GetComponent<ShopItemToggle>();
            shopItem.Initialize(ItemSelected);

            itemTogglePool.Add(shopItem);
            newToggle.gameObject.SetActive(false);
        }
    }
    private void RefreshTable()
    {
        string npcName = MainPanel.CurrentNPC.Name;
        string itemType = UIText_Util.Instance.GetKorItemTypeByEng(currentItemCategory);
        ShopTitleText.text = $"[{npcName}] 상점 : {itemType}";

        for (int i = 0; i < itemTogglePool.Count; ++i)
        {
            itemTogglePool[i].GetComponent<Toggle>().isOn = false;
            itemTogglePool[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < currentItems.Length; ++i)
            itemTogglePool[i].Refresh(currentItems[i]);
        if (currentItems.Length > 0)
        {
            itemTogglePool[0].GetComponent<Toggle>().isOn = true;
        }
    }
    // Callback
    public void ChangeItemCategory(string type)
    {
        ShopItem[] foundItems = null;
        if (!itemSetByType.TryGetValue(type, out foundItems))
        {
            Debug.Log($"{type}종류의 아이템이 현재 NPC의 ShopData에 없습니다.");
            return;
        }
        currentItems = foundItems;
        currentItemCategory = type;
        RefreshTable();
    }
    private void ItemSelected(ShopItem selectedItem)
    {
        this.selectedItem = selectedItem;
        MainPanel.ShopItemWasSelected(selectedItem);
    }
}
