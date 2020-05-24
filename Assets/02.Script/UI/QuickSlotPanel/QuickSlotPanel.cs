using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class QuickSlotPanel : MonoBehaviour
{
    // UI
    public GameObject AttachGuide;
    public QuickSlotButton[] SlotButtons;

    // Data
    private InventoryItem waitToAttachItem;

    public void Initialize()
    {
        for (int i = 0; i < SlotButtons.Length; ++i)
            SlotButtons[i].Initialize(this);
    }
    public void Refresh()
    {
        for (int i = 0; i < SlotButtons.Length; ++i)
            SlotButtons[i].Refresh();
    }
    public void FoundSameItemAndEmpty(int itemCode)
    {
        for (int i = 0; i < SlotButtons.Length; ++i)
        {
            if (SlotButtons[i].SlotItem != null &&
                SlotButtons[i].SlotItem.OriginalItemData.ItemCode == itemCode)
            {
                PlayerQuickSlot.Instance.ItemsInSlot[i] = 0;
                SlotButtons[i].Refresh();
            }
        }
    }
    public void OpenPanel_ModeAttach(InventoryItem attachItem)
    {
        waitToAttachItem = attachItem;
        AttachGuide.gameObject.SetActive(true);
        transform.SetSiblingIndex(3);

        for (int i = 0; i < SlotButtons.Length; ++i)
            SlotButtons[i].ChangeModeToAttach(waitToAttachItem);
    }
    public void CancelAttachItem()
    {
        AttachGuide.gameObject.SetActive(false);
        transform.SetSiblingIndex(2);

        for (int i = 0; i < SlotButtons.Length; ++i)
            SlotButtons[i].ChangeModeToUseItem();
    }
}
