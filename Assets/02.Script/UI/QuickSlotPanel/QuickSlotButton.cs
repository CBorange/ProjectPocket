using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;

public enum QuickSlotMode
{
    AttachItem,
    UseItem
};
public class QuickSlotButton : MonoBehaviour
{
    // Controller
    public QuickSlotPanel SlotPanel;
    private Button myButton;

    // UI
    public Image SlotBG;
    public GameObject ItemGraphics;
    public Image ItemImage;
    public Text ItemCountText;
    public Text ItemName;
    public Text EquipedText;

    // Data
    public int SlotIndex;
    private QuickSlotMode currentMode;
    private InventoryItem slotItem;
    public InventoryItem SlotItem
    {
        get { return slotItem; }
        set { slotItem = value; }
    }
    private InventoryItem waitAttachItem;


    public void Initialize(QuickSlotPanel parentPanel)
    {
        SlotPanel = parentPanel;
        currentMode = QuickSlotMode.UseItem;
        myButton = GetComponent<Button>();
        Refresh();
    }
    public void ChangeModeToAttach(InventoryItem attachItem)
    {
        currentMode = QuickSlotMode.AttachItem;
        waitAttachItem = attachItem;
    }
    public void ChangeModeToUseItem()
    {
        currentMode = QuickSlotMode.UseItem;
        waitAttachItem = null;
    }
    public void Refresh()
    {
        slotItem = PlayerInventory.Instance.GetItem(PlayerQuickSlot.Instance.ItemsInSlot[SlotIndex]);
        if (slotItem == null)
        {
            myButton.targetGraphic = SlotBG;
            ItemGraphics.gameObject.SetActive(false);
            return;
        }
        if (slotItem.ItemCount == 0)
        {
            myButton.targetGraphic = SlotBG;
            slotItem = null;
            ItemGraphics.gameObject.SetActive(false);
            return;
        }
        else
        {
            myButton.targetGraphic = ItemImage;
            ItemGraphics.gameObject.SetActive(true);
            try
            {
                string itemImagePath = $"Image/ItemPreview/{slotItem.OriginalItemData.ItemType}/{slotItem.OriginalItemData.Name}";
                ItemImage.sprite = Resources.Load<Sprite>(itemImagePath);
            }
            catch (Exception)
            {
                ItemImage.sprite = null;
            }
            ItemName.text = slotItem.OriginalItemData.Name;
            ItemCountText.text = slotItem.ItemCount.ToString();

            if (slotItem.OriginalItemData.ItemType.Equals("Expendable") ||
                slotItem.OriginalItemData.ItemType.Equals("Etc"))
            {
                ItemCountText.gameObject.SetActive(true);
                EquipedText.gameObject.SetActive(false);
            }
            else
            {
                ItemCountText.gameObject.SetActive(false);
                if (PlayerEquipment.Instance.HasEquipedItem(slotItem.OriginalItemData.ItemCode))
                    EquipedText.gameObject.SetActive(true);
                else
                    EquipedText.gameObject.SetActive(false);
            }
        }
    }

    public void SelectSlot()
    {
        switch(currentMode)
        {
            case QuickSlotMode.AttachItem:
                SlotPanel.FoundSameItemAndEmpty(waitAttachItem.OriginalItemData.ItemCode);
                slotItem = waitAttachItem;
                PlayerQuickSlot.Instance.ItemsInSlot[SlotIndex] = slotItem.OriginalItemData.ItemCode;
                SlotPanel.CancelAttachItem();
                Refresh();
                break;
            case QuickSlotMode.UseItem:
                if (slotItem == null)
                    return;
                ItemData data = slotItem.OriginalItemData;
                switch (data.ItemType)
                {
                    case "Weapon":
                        PlayerEquipment.Instance.EquipWeapon(data.ItemCode);
                        break;
                    case "Accesorie":
                        AccesorieData accesorieData = ItemDB.Instance.GetAccesorieData(data.ItemCode);

                        if (accesorieData.AccesorieType.Equals("Ring"))
                            PlayerEquipment.Instance.EquipAccesorie_Ring(accesorieData);
                        else if (accesorieData.AccesorieType.Equals("Necklace"))
                            PlayerEquipment.Instance.EquipAccesorie_Necklace(accesorieData);
                        break;
                    case "Expendable":
                        ExpendableData expendableData = ItemDB.Instance.GetExpendableData(data.ItemCode);
                        PlayerBuffer.Instance.ApplyStatEffectByExpendable(expendableData);
                        PlayerInventory.Instance.RemoveItemFromInventory(expendableData.ItemCode, 1);
                        break;
                }
                SlotPanel.Refresh();
                break;
        }
    }
}
