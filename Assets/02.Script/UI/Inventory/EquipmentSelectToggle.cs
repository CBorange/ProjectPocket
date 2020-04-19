using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSelectToggle : MonoBehaviour
{
    public Image ItemImage;
    public Text ItemName;
    private ItemData currentItem;
    public ItemData CurrentItem
    {
        get { return currentItem; }
    }
    private Action<EquipmentSelectToggle> noticeSelected_CALLBACK;

    public void Initialize(Action<EquipmentSelectToggle> selectedNoticeCallback)
    {
        noticeSelected_CALLBACK = selectedNoticeCallback;
    }
    public void Refresh(ItemData item)
    {
        if (item == null)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        currentItem = item;

        try
        {
            string itemImagePath = $"Image/ItemPreview/{currentItem.ItemType}/{currentItem.Name}";
            ItemImage.sprite = Resources.Load<Sprite>(itemImagePath);
        }
        catch (Exception)
        {
            ItemImage.sprite = null;
        }
        ItemName.text = currentItem.Name;

        gameObject.SetActive(true);
    }

    public void EquipmentSelected(bool selected)
    {
        if (!selected)
            return;
        noticeSelected_CALLBACK(this);
    }
}
