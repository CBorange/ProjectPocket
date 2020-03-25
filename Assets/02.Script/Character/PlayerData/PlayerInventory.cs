using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
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

    private void Start()
    {
        if (!DBConnector.Instance.LoadUserInventory().Equals("Success"))
            Debug.Log("유저인벤토리 로드 에러");
        if (!DBConnector.Instance.LoadUserEquipment(UserInfoProvider.Instance.UserAccount).Equals("Success"))
            Debug.Log("유저인벤토리 로드 에러");
        if (!DBConnector.Instance.LoadItemDB().Equals("Success"))
            Debug.Log("아이템DB 로드 에러");

        impliedWeaponDataInfos = new List<ImpliedItemData>();
        impliedAccesorieDataInfos = new List<ImpliedItemData>();
        impliedExpendableDataInfos = new List<ImpliedItemData>();
        impliedEtcDataInfos = new List<ImpliedItemData>();

        ImpliedItemData[] inventoryItems = UserInventoryProvider.Instance.ImplitedItemDatas;
        for (int i = 0; i < inventoryItems.Length; ++i)
        {
            switch(inventoryItems[i].ItemType)
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
