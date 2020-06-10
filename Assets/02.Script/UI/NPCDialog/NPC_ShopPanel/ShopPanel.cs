using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    // Controller
    public ShopPanel_ItemTable itemTable;
    public ShopPanel_InteractPanel interactPanel;
    public AlertPopup alertPopup;

    // UI
    public ShopCategoryToggle[] CategoryToggles;

    // Data
    private NPCData currentNPC;
    public NPCData CurrentNPC
    {
        get { return currentNPC; }
    }

    public void Initialize()
    {
        for (int i = 0; i < CategoryToggles.Length; ++i)
            CategoryToggles[i].Initialize(SelectCategoryToggle);

        itemTable.Initialize();
        interactPanel.Initialize();
    }
    public void OpenPanel(NPCData currentNPC)
    {
        this.currentNPC = currentNPC;
        itemTable.OpenPanel(currentNPC.ShopData);
        interactPanel.OpenPanel();

        for (int i = 0; i < CategoryToggles.Length; ++i)
            CategoryToggles[i].gameObject.SetActive(false);

        for (int i = 0; i < currentNPC.ShopData.SalesItemTypes.Length; ++i)
        {
            CategoryToggles[i].Refresh(currentNPC.ShopData.SalesItemTypes[i]);
        }
        if (currentNPC.ShopData.SalesItemTypes.Length > 0)
        {
            CategoryToggles[0].GetComponent<Toggle>().isOn = true;
            SelectCategoryToggle(currentNPC.ShopData.SalesItemTypes[0]);
        }

        gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
        alertPopup.ClosePopup();
    }

    // Method For Transmit Between ItemTable <-> InteractPanel
    public void ShopItemWasSelected(ShopItem item)
    {
        interactPanel.ShopItemWasSelected(item);
    }

    // Item CategoryToggle Callbacks
    public void SelectCategoryToggle(string type)
    {
        itemTable.ChangeItemCategory(type);
    }
}
