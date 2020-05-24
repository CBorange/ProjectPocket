using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserQuickSlotProvider
{
    // Singleton
    private UserQuickSlotProvider() { }
    private static UserQuickSlotProvider instance;
    public static UserQuickSlotProvider Instance
    {
        get
        {
            if (instance == null)
                instance = new UserQuickSlotProvider();
            return instance;
        }
    }

    // Data
    private int[] itemsInSlot;
    public int[] ItemsInSlot
    {
        get { return itemsInSlot; }
    }

    public void Initialize(int slot_0, int slot_1, int slot_2, int slot_3, int slot_4)
    {
        itemsInSlot = new int[5];
        itemsInSlot[0] = slot_0;
        itemsInSlot[1] = slot_1;
        itemsInSlot[2] = slot_2;
        itemsInSlot[3] = slot_3;
        itemsInSlot[4] = slot_4;
    }

    public void Save_PlayerQuickSlot_UpdateServerDB()
    {
        for (int i = 0; i < 5; ++i)
            itemsInSlot[i] = PlayerQuickSlot.Instance.ItemsInSlot[i];
        DBConnector.Instance.Save_PlayerQuickSlot();
    }
}
