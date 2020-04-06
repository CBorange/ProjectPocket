using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentSelectToggle : MonoBehaviour
{
    public Image ItemImage;
    public Text ItemName;
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
    private Action<EquipmentSelectToggle> noticeSelected_CALLBACK;

    public void Initialize(Action<EquipmentSelectToggle> selectedNoticeCallback)
    {
        noticeSelected_CALLBACK = selectedNoticeCallback;
    }
    public void Refresh(ImpliedItemData impliedData)
    {
        if (impliedData == null)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        impliedItemData = impliedData;
        currentItem = ItemDB.Instance.GetItemData(impliedData.ItemCode);

        try
        {
            string itemImagePath = $"Image/ItemPreview/{impliedData.ItemType}/{currentItem.Name}";
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
