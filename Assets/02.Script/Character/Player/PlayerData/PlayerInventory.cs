using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, PlayerRuntimeData
{
    #region Singleton
    private static PlayerInventory instance;
    public static PlayerInventory Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindObjectOfType<PlayerInventory>();
                if (obj != null)
                    instance = obj;
                else
                {
                    var newSingleton = new GameObject("PlayerInventory").AddComponent<PlayerInventory>();
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
        var objs = FindObjectsOfType<PlayerInventory>();
        if (objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }
    }
    #endregion
    // Data
    private List<ImpliedItemData> impliedWeaponDataInfos;
    public List<ImpliedItemData> ImpliedWeaponDataInfos
    {
        get { return impliedWeaponDataInfos; }
    }
    private List<ImpliedItemData> impliedAccesorieDataInfos;
    public List<ImpliedItemData> ImpliedAccesorieDataInfos
    {
        get { return impliedAccesorieDataInfos; }
    }
    private List<ImpliedItemData> impliedExpendableDataInfos;
    public List<ImpliedItemData> ImpliedExpendableDataInfos
    {
        get { return impliedExpendableDataInfos; }
    }
    private List<ImpliedItemData> impliedEtcDataInfos;
    public List<ImpliedItemData> ImpliedEtcDataInfos
    {
        get { return impliedEtcDataInfos; }
    }
    private Dictionary<int, ImpliedItemData> allItems;
    public Dictionary<int, ImpliedItemData> AllItems
    {
        get { return allItems; }
    }

    public void Initialize()
    {
        impliedWeaponDataInfos = new List<ImpliedItemData>();
        impliedAccesorieDataInfos = new List<ImpliedItemData>();
        impliedExpendableDataInfos = new List<ImpliedItemData>();
        impliedEtcDataInfos = new List<ImpliedItemData>();
        allItems = new Dictionary<int, ImpliedItemData>();

        ImpliedItemData[] inventoryItems = UserInventoryProvider.Instance.ImplitedItemDatas;
        if (inventoryItems == null)
            return;                                                                                                       
        for (int i = 0; i < inventoryItems.Length; ++i)
        {
            allItems.Add(inventoryItems[i].ItemCode, inventoryItems[i]);
            switch (inventoryItems[i].ItemType)
            {
                case "Weapon":
                    impliedWeaponDataInfos.Add(inventoryItems[i]);
                    break;
                case "Expendable":
                    impliedExpendableDataInfos.Add(inventoryItems[i]);
                    break;
                case "Accesorie":
                    impliedAccesorieDataInfos.Add(inventoryItems[i]);
                    break;
                case "Etc":
                    impliedEtcDataInfos.Add(inventoryItems[i]);
                    break;
            }
        }
    }
}
