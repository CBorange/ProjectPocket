﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectToggle : MonoBehaviour
{
    public Image ItemImage;
    public Text ItemName;
    public Text NoticeEquipThisText;
    private ImpliedItemData impliedItemData;
    public ImpliedItemData CurrentItemImpliedData
    {
        get { return impliedItemData; }
    }
    private ItemData currentItem;
    public ItemData CurrentItem
    {
        get { return currentItem; }
    }
    private Action<ItemSelectToggle, string> noticeSelected_CALLBACK;

    public void Initialize(Action<ItemSelectToggle, string> selectedNoticeCallback)
    {
        noticeSelected_CALLBACK = selectedNoticeCallback;
    }

    public void Refresh(ImpliedItemData impliedData, bool equiped)
    {
        gameObject.SetActive(true);
        impliedItemData = impliedData;
        currentItem = ItemDB.Instance.GetItemData(impliedData.ItemCode);

        try
        {
            string itemImagePath = $"Image/ItemPreview/{impliedData.ItemType}/{currentItem.Name}";
            ItemImage.sprite = Resources.Load<Sprite>(itemImagePath);
        }
        catch(Exception)
        {
            ItemImage.sprite = null;
        }
        ItemName.text = currentItem.Name;
        NoticeEquipThisText.gameObject.SetActive(equiped);
        gameObject.SetActive(true);
    }
    
    public void ItemSelected(bool selected)
    {
        if (!selected)
            return;
        noticeSelected_CALLBACK(this, impliedItemData.ItemType);
    }
}
