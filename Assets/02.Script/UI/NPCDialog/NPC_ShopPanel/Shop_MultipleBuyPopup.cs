﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Shop_MultipleBuyPopup : MonoBehaviour
{
    // Controller
    public AlertPopup AlertPopup;
    public ShopPanel_InteractPanel InteractPanel;

    //UI
    public Image ItemImage;
    public Text ItemName;
    public Text ItemIntroduce;
    public Text PricePerSingleText;
    public Text PriceFinalText;
    public Text PlayerGoldText;
    public InputField PurchaseNumInputField;

    // Data
    private int purchaseNum;
    private ShopItem shopItemInfo;
    private ItemData itemData;

    public void OpenPopup(ShopItem shopItem, ItemData itemData)
    {
        gameObject.SetActive(true);

        this.shopItemInfo = shopItem;
        this.itemData = itemData;
        RefreshItemInfo();

        purchaseNum = 0;
        PurchaseNumInputField.text = purchaseNum.ToString();
        RefreshPriceInfo();
    }
    private void RefreshItemInfo()
    {
        try
        {
            Texture2D tex = AssetBundleCacher.Instance.LoadAndGetAsset("itempreview", itemData.Name) as Texture2D;
            ItemImage.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        catch (Exception)
        {
            ItemImage.sprite = null;
        }
        ItemName.text = itemData.Name;
        ItemIntroduce.text = itemData.Introduce;
    }
    private void RefreshPriceInfo()
    {
        PricePerSingleText.text = shopItemInfo.Price.ToString();
        PriceFinalText.text = (shopItemInfo.Price * purchaseNum).ToString();
        PlayerGoldText.text = PlayerStat.Instance.GetStat("Gold").ToString();
    }
    public void ChangedPurchaseNum(string num)
    {
        int result = 0;
        if (int.TryParse(num, out result))
            purchaseNum = result;
        else
            purchaseNum = 0;
        RefreshPriceInfo();
    }
    public void CanclePurchase()
    {
        gameObject.SetActive(false);
    }
    public void AcceptPurchase()
    {
        if (purchaseNum <= 0)
        {
            AlertPopup.RefreshToAlert("구매할 개수 가 유효하지 않습니다.");
            AlertPopup.OpenPopup(1.0f);
            return;
        }
        int finalPrice = shopItemInfo.Price * purchaseNum;
        if (finalPrice > PlayerStat.Instance.GetStat("Gold"))
        {
            AlertPopup.RefreshToAlert("소지금이 부족합니다!");
            AlertPopup.OpenPopup(1.0f);
            return;
        }
        InteractPanel.BuyCurrentItem(purchaseNum);
        gameObject.SetActive(false);
    }
}
