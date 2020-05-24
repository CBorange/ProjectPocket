using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class InventoryPanel_SellItem : MonoBehaviour
{
    // Controller
    public QuickSlotPanel QuickSlot_Panel;
    public InventoryPanel_ItemTable Inventory_ItemTable;
    public AlertPopup AlertPopup;

    // UI
    public Text AlertText;
    public Transform SellNumInputPanel;
    public InputField SellNumInputField;

    // Data
    private int sellNum;
    private ItemData itemData;

    public void OpenPanel(ItemData itemData)
    {
        this.itemData = itemData;

        sellNum = 1;
        RefreshPanelByItemType();
        RefreshAlertText();
        gameObject.SetActive(true);
    }

    private void RefreshAlertText()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("<color=red>경고!</color>");
        builder.AppendLine($"[<color=orange>{itemData.Name}</color>] 아이템을");
        builder.AppendLine($"<color=yellow>{(itemData.SellPrice * sellNum)}</color> 원 에 판매하려 하고있습니다.");
        builder.AppendLine($"판매한 아이템은 되돌릴 수 없습니다!");
        builder.AppendLine($"계속하시겠습니까?");
        AlertText.text = builder.ToString();
    }
    private void RefreshPanelByItemType()
    {
        if (itemData.ItemType.Equals("Expendable") || itemData.ItemType.Equals("Etc"))
            SellNumInputPanel.gameObject.SetActive(true);
        else
            SellNumInputPanel.gameObject.SetActive(false);
    }

    public void ChangedSellNum(string num)
    {
        int result = 0;
        if (int.TryParse(num, out result))
            sellNum = result;
        else
            sellNum = 0;
        RefreshAlertText();
    }

    public void CancleSell()
    {
        gameObject.SetActive(false);
    }
    public void AcceptSell()
    {
        if (sellNum <= 0)
        {
            AlertPopup.RefreshToAlert("<color=red>판매할 개수 가 유효하지 않습니다.</color>");
            AlertPopup.OpenPopup(1.0f);
            return;
        }
        if (PlayerInventory.Instance.GetItem(itemData.ItemCode).ItemCount < sellNum)
        {
            AlertPopup.RefreshToAlert("<color=red>아이템 소지개수보다 판매 개수가 더 많습니다.</color>");
            AlertPopup.OpenPopup(1.0f);
            return;
        }
        PlayerInventory.Instance.RemoveItemFromInventory(itemData.ItemCode, sellNum);
        PlayerStat.Instance.AddGold(itemData.SellPrice * sellNum);
        gameObject.SetActive(false);

        Inventory_ItemTable.RefreshInventoryPanel();
        QuickSlot_Panel.Refresh();
        AlertPopup.RefreshToAlert("판매가 완료되었습니다!");
        AlertPopup.OpenPopup(1.0f);
    }
}
