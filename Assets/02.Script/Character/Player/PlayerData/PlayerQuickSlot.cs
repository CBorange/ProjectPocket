using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerQuickSlot : MonoBehaviour
{
    #region Singleton
    private static PlayerQuickSlot instance;
    public static PlayerQuickSlot Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<PlayerQuickSlot>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("PlayerQuickSlot").AddComponent<PlayerQuickSlot>();
                    instance = newSingleton;
                }
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private void Awake()
    {
        var objs = FindObjectsOfType<PlayerQuickSlot>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion

    // Data
    private InventoryItem[] itemsInSlot;
    public InventoryItem[] ItemsInSlot
    {
        get { return itemsInSlot; }
    }

    public void Initialize()
    {
        int[] itemCodes = UserQuickSlotProvider.Instance.ItemsInSlot;

        itemsInSlot = new InventoryItem[5];
        for (int i = 0; i < 5; ++i)
        {
            if (itemCodes[i] == 0)
                itemsInSlot[i] = null;
            else
                itemsInSlot[i] = PlayerInventory.Instance.GetItem(itemCodes[i]);
        }
    }
}
