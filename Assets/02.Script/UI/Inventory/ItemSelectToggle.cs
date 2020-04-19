using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectToggle : MonoBehaviour
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
    private Action<ItemSelectToggle, string> noticeSelected_CALLBACK;

    public void Initialize(Action<ItemSelectToggle, string> selectedNoticeCallback)
    {
        noticeSelected_CALLBACK = selectedNoticeCallback;
    }

    public void Refresh(InventoryItem item, bool equiped)
    {
        currentItem = item;
        try
        {
            string itemImagePath = $"Image/ItemPreview/{currentItem.OriginalItemData.ItemType}/{currentItem.OriginalItemData.Name}";
            ItemImage.sprite = Resources.Load<Sprite>(itemImagePath);
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
