using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ShopItemToggle : MonoBehaviour
{
    // UI
    public Image ItemImage;
    public Text ItemName;

    // Data
    private ShopItem currentItem;
    public ShopItem CurrentItem
    {
        get { return currentItem; }
    }
    private Action<ShopItem> selectedCallback;

    public void Initialize(Action<ShopItem> callback)
    {
        selectedCallback = callback;
    }
    public void Refresh(ShopItem item)
    {
        this.currentItem = item;
        ItemData data = ItemDB.Instance.GetItemData(currentItem.ItemCode);
        try
        {
            string itemImagePath = $"Image/ItemPreview/{data.ItemType}/{data.Name}";
            ItemImage.sprite = Resources.Load<Sprite>(itemImagePath);
        }
        catch (Exception)
        {
            ItemImage.sprite = null;
        }
        ItemName.text = data.Name;
        gameObject.SetActive(true);
    }
    public void SelectToggle(bool select)
    {
        if (!select)
            return;
        selectedCallback(currentItem);
    }
}
