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
    private int[] itemsInSlot;
    public int[] ItemsInSlot
    {
        get { return itemsInSlot; }
    }

    public void Initialize()
    {
        itemsInSlot = UserQuickSlotProvider.Instance.ItemsInSlot;
    }
}
