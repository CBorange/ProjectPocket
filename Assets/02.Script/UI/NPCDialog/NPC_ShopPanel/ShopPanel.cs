using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    // Controller
    public ShopPanel_ItemTable itemTable;
    public ShopPanel_InteractPanel interactPanel;

    // UI
    public Toggle[] ItemCategoryToggles;

    // Data
    private string beforeItemCategoryType;
    private NPCData currentNPC;
    public NPCData CurrentNPC
    {
        get { return currentNPC; }
    }

    public void Initialize()
    {
        beforeItemCategoryType = string.Empty;
        itemTable.Initialize();
        interactPanel.Initialize();
    }
    public void OpenPanel(NPCData currentNPC)
    {
        for (int i = 0; i < currentNPC.ShopData.SalesItemTypes.Length; ++i)
        {
            switch(currentNPC.ShopData.SalesItemTypes[i])
            {
                case "Weapon":
                    ItemCategoryToggles[0].gameObject.SetActive(true);
                    break;
                case "Accesorie":
                    ItemCategoryToggles[1].gameObject.SetActive(true);
                    break;
                case "Expendable":
                    ItemCategoryToggles[2].gameObject.SetActive(true);
                    break;
                case "Etc":
                    ItemCategoryToggles[3].gameObject.SetActive(true);
                    break;
            }
        }

        this.currentNPC = currentNPC;
        itemTable.OpenPanel(currentNPC.ShopData);
        interactPanel.OpenPanel();

        gameObject.SetActive(true);
    }
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }

    // Method For Transmit Between ItemTable <-> InteractPanel
    public void ShopItemWasSelected(ShopItem item)
    {
        interactPanel.ShopItemWasSelected(item);
    }

    // Item CategoryToggle Callbacks
    public void SelectCategoryToggle(string type)
    {
        if (beforeItemCategoryType.Equals(type)) 
            return;
        beforeItemCategoryType = type;
        Debug.Log($"ChangedCategory : {type}");
        itemTable.ChangeItemCategory(type);
    }
}
