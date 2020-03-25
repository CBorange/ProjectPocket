using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class ItemSelectToggle : MonoBehaviour
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
    private Action<ItemSelectToggle, string> noticeSelectedToPanel_CALLBACK;

    public void Refresh(ImpliedItemData impliedData)
    {
        impliedItemData = impliedData;

        currentItem = null;
        switch(impliedData.ItemType)
        {
            case "Weapon":
                currentItem = ItemDB.Instance.GetWeaponData(impliedData.ItemCode);
                break;
            case "Accesorie":
                currentItem = ItemDB.Instance.GetAccesorieData(impliedData.ItemCode);
                break;
            case "Expendable":
                currentItem = ItemDB.Instance.GetExpendableData(impliedData.ItemCode);
                break;
            case "Etc":
                currentItem = ItemDB.Instance.GetEtcData(impliedData.ItemCode);
                break;
        }

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

        gameObject.SetActive(true);
    }
    public void Initialize(Action<ItemSelectToggle, string> selectedNoticeCallback)
    {
        noticeSelectedToPanel_CALLBACK = selectedNoticeCallback;
    }
    public void ItemSelected(bool selected)
    {
        if (!selected)
            return;
        noticeSelectedToPanel_CALLBACK(this, impliedItemData.ItemType);
    }
}
