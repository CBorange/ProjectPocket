using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;

public class ShopPanel_InteractPanel : MonoBehaviour
{
    // Controller
    public AlertPopup AlertPopup;
    public Shop_MultipleBuyPopup MultipleBuyPopup;

    // UI
    public Image ItemImage;
    public Text ItemName;
    public Text ItemIntroduce;
    public Text ItemStatTitle;
    public Text ItemStat;
    public Text ItemPrice;
    public Text PlayerGold;
    public Button BuySingleBtn;
    public GameObject BuyMultipleBtns;

    // Data
    private ShopItem currentShopItemInfo;
    private ItemData currentItemData;

    public void Initialize()
    {

    }
    public void OpenPanel()
    {
        BuyMultipleBtns.gameObject.SetActive(false);
    }
    public void ClosePanel()
    {

    }

    private void RefreshPanel()
    {
        BuyMultipleBtns.gameObject.SetActive(false);
        try
        {
            Texture2D tex = AssetBundleCacher.Instance.LoadAndGetAsset("itempreview", currentItemData.Name) as Texture2D;
            ItemImage.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
        }
        catch (Exception)
        {
            ItemImage.sprite = null;
        }
        ItemName.text = currentItemData.Name;
        ItemIntroduce.text = currentItemData.Introduce;

        StringBuilder builder = new StringBuilder();
        switch(currentItemData.ItemType)
        {
            case "Weapon":
                WeaponData weapon = ItemDB.Instance.GetWeaponData(currentItemData.ItemCode);
                StatAdditional[] weaponStat = weapon.WeaponStat;
                for (int i = 1; i < weaponStat.Length + 1; ++i)
                {
                    builder.Append($"{UIText_Util.Instance.GetKorStatByEng(weaponStat[i - 1].StatName)} +{weaponStat[i - 1].StatValue}");
                    if (i % 2 != 0)
                        builder.Append("            ");
                    else
                        builder.AppendLine();
                }
                ItemStat.text = builder.ToString();
                break;
            case "Accesorie":
                AccesorieData accesorie = ItemDB.Instance.GetAccesorieData(currentItemData.ItemCode);
                StatAdditional[] accesorieStat = accesorie.AccesorieStat;
                for (int i = 1; i < accesorieStat.Length + 1; ++i)
                {
                    builder.Append($"{UIText_Util.Instance.GetKorStatByEng(accesorieStat[i - 1].StatName)} +{accesorieStat[i - 1].StatValue}");
                    if (i % 2 != 0)
                        builder.Append("            ");
                    else
                        builder.AppendLine();
                }
                ItemStat.text = builder.ToString();
                break;
            case "Expendable":
                BuyMultipleBtns.gameObject.SetActive(true);
                ExpendableData expendable = ItemDB.Instance.GetExpendableData(currentItemData.ItemCode);
                ExpendableEffect[] effects = expendable.Effects;
                for (int i = 0; i < effects.Length; ++i)
                {
                    string duration = string.Empty;
                    if (effects[i].EffectDuration == 0)
                        duration = "<color=cyan>즉시 발동</color>";
                    else
                        duration = $"<color=green>{effects[i].EffectDuration.ToString()}초 지속</color>";
                    builder.Append($"{UIText_Util.Instance.GetKorStatByEng(effects[i].StatName)} +{effects[i].StatAmount}            {duration}");
                    builder.AppendLine();
                }
                ItemStat.text = builder.ToString();
                break;
            case "Etc":
                BuyMultipleBtns.gameObject.SetActive(true);
                ItemStatTitle.gameObject.SetActive(false);
                ItemStat.gameObject.SetActive(false);
                break;
        }
        ItemPrice.text = $"{currentShopItemInfo.Price} 원";
        PlayerGold.text = $"소지금 : {PlayerStat.Instance.GetStat("Gold")} 원";
    }
    // UI Event Callback
    public void ShopItemWasSelected(ShopItem item)
    {
        currentShopItemInfo = item;
        currentItemData = ItemDB.Instance.GetItemData(item.ItemCode);

        RefreshPanel();
    }
    public void BuyCurrentItem(int count)
    {
        int finalPrice = currentShopItemInfo.Price * count;
        if (finalPrice <= PlayerStat.Instance.GetStat("Gold"))
        {
            AlertPopup.RefreshToBuyItem(currentItemData.Name, count.ToString(), currentShopItemInfo.Price.ToString());
            AlertPopup.OpenPopup(2.0f);
            PlayerInventory.Instance.AddItemToInventory(new InventoryItem(currentItemData, count));
            PlayerStat.Instance.RemoveGold(finalPrice);
            RefreshPanel();
        }
        else
        {
            AlertPopup.RefreshToAlert("소지금이 부족합니다!");
            AlertPopup.OpenPopup(1.0f);
        }
    }
    public void OpenBuyMultitpleItemPanel()
    {
        MultipleBuyPopup.OpenPopup(currentShopItemInfo, currentItemData);
    }
}
