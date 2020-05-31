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
            Texture2D tex = AssetBundleCacher.Instance.LoadAndGetAsset("itempreview", data.Name) as Texture2D;
            ItemImage.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
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
