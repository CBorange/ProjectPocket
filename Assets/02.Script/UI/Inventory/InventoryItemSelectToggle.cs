using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemSelectToggle : MonoBehaviour
{
    // UI
    public Image ItemImage;
    public Text ItemName;
    public Text NoticeEquipThisText;
    public Text ItemCountText;

    // Data
    private InventoryItem currentItem;
    public InventoryItem CurrentItem
    {
        get { return currentItem; }
    }
    private Action<InventoryItemSelectToggle, string> noticeSelected_CALLBACK;

    public void Initialize(Action<InventoryItemSelectToggle, string> selectedNoticeCallback)
    {
        noticeSelected_CALLBACK = selectedNoticeCallback;
    }

    public void Refresh(InventoryItem item, bool equiped)
    {
        currentItem = item;
        try
        {
            Texture2D itemTexture = AssetBundleCacher.Instance.LoadAndGetAsset("itempreview", $"{currentItem.OriginalItemData.Name}") as Texture2D;
            Sprite itemSprite = Sprite.Create(itemTexture, new Rect(0.0f, 0.0f, itemTexture.width, itemTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            ItemImage.sprite = itemSprite;
        }
        catch(Exception)
        {
            ItemImage.sprite = null;
        }

        ItemName.text = currentItem.OriginalItemData.Name;
        NoticeEquipThisText.gameObject.SetActive(equiped);
        if (currentItem.OriginalItemData.ItemType != "Weapon" && currentItem.OriginalItemData.ItemType != "Accesorie")
        {
            ItemCountText.gameObject.SetActive(true);
            ItemCountText.text = currentItem.ItemCount.ToString();
        }
        else
            ItemCountText.gameObject.SetActive(false);

        gameObject.SetActive(true);
    }
    
    public void ItemSelected(bool selected)
    {
        if (!selected)
            return;
        noticeSelected_CALLBACK(this, currentItem.OriginalItemData.ItemType);
    }
}
