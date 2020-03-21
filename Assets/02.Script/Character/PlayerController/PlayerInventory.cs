using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private List<ItemData> weaponItems;
    private List<ItemData> accesorieItems;
    private List<ItemData> expandableItems;
    private List<ItemData> etcItems;

    private void Start()
    {
        DBConnector.Instance.LoadUserInventory();
        weaponItems = new List<ItemData>();
        accesorieItems = new List<ItemData>();
        expandableItems = new List<ItemData>();
        etcItems = new List<ItemData>();

        ItemData[] dbItems = UserInventoryProvider.Instance.itemsInInventory;
        for (int i = 0; i < dbItems.Length; ++i)
        {
            switch(dbItems[i].ItemType)
            {
                case "Weapon":
                    weaponItems.Add(dbItems[i]);
                    break;
                case "Expandable":
                    expandableItems.Add(dbItems[i]);
                    break;
                case "Accesorie":
                    accesorieItems.Add(dbItems[i]);
                    break;
                case "Etc":
                    etcItems.Add(dbItems[i]);
                    break;
            }
        }
    }
}
