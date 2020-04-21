using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDropper : MonoBehaviour
{
    // Controller
    public MonsterStat Stat;

    // Data
    private Transform droppedItemPoolObj;
    private DropItemData[] itemDatas;
    private DropGoldData goldData;

    private Dictionary<int, List<DroppedItem>> deactiveDroppedItems;
    private List<DroppedCoin> deactiveDroppedCoins;

    public void Initialize(Transform mobSpawnerTrans)
    {
        CreateDroppedItemPoolObject(mobSpawnerTrans);

        deactiveDroppedItems = new Dictionary<int, List<DroppedItem>>();
        deactiveDroppedCoins = new List<DroppedCoin>();

        itemDatas = Stat.CurrentData.DropItemDatas;
        goldData = Stat.CurrentData.GoldData;

        CreateDropItemPool(2);
        CreateDropCoin(2);
    }
    public void Death()
    {
        // Coin
        int dropCoinAmount = UnityEngine.Random.Range(goldData.MinDropAmount, goldData.MaxDropAmount);
        while (true)
        {
            if (deactiveDroppedCoins.Count > 0)
            {
                deactiveDroppedCoins[0].Drop(dropCoinAmount, transform.position);
                deactiveDroppedCoins.RemoveAt(0);
                break;
            }
            else
                CreateDropCoin(2);
        }

        // Item
        for (int i = 0; i < itemDatas.Length; ++i)
        {
            int dropValue = UnityEngine.Random.Range(1, 100 + 1);
            if (!(dropValue <= itemDatas[i].DropPercentage)) 
                continue;

            int dropAmount = UnityEngine.Random.Range(itemDatas[i].MinDropCount, itemDatas[i].MaxDropCount + 1);
            List<DroppedItem> pool;
            if (!deactiveDroppedItems.TryGetValue(itemDatas[i].ItemCode, out pool))
            {
                Debug.Log($"ItemDropper 오류 : DropItem Dictionary -> {itemDatas[i].ItemCode} Key에 List<DropItem> 이 존재하지 않음, 아이템 Drop 실패");
                continue;
            }
            while (true)
            {
                if (pool.Count > 0)
                {
                    for (int dropIdx = 0; dropIdx < dropAmount; ++dropIdx)
                    {
                        pool[0].Drop(transform.position);
                        pool.RemoveAt(0);
                    }
                    break;
                }
                else
                    CreateDropItemPool(2);
            }
        }
    }
    private void CreateDroppedItemPoolObject(Transform mobSpawnerTrans)
    {
        GameObject pool = new GameObject("DroppedItemPool");
        pool.transform.position = Vector3.zero;
        pool.transform.parent = mobSpawnerTrans;
        droppedItemPoolObj = pool.transform;
    }
    private void CreateDropItemPool(int createMultiple)
    {
        for (int itemIdx = 0; itemIdx < itemDatas.Length; ++itemIdx)
        {
            List<DroppedItem> itemPool;
            if (!deactiveDroppedItems.TryGetValue(itemDatas[itemIdx].ItemCode, out itemPool))
                itemPool = new List<DroppedItem>();
            for (int createIdx = 0; createIdx < itemDatas[itemIdx].MaxDropCount * createMultiple; ++createIdx)
            {
                ItemData data = ItemDB.Instance.GetItemData(itemDatas[itemIdx].ItemCode);
                GameObject foundPrefab = Resources.Load<GameObject>($"Dropped{data.ItemType}/{data.Name}_Dropped");
                DroppedItem newItem = Instantiate(foundPrefab).GetComponent<DroppedItem>();
                newItem.transform.parent = droppedItemPoolObj;
                newItem.gameObject.SetActive(false);
                newItem.Initialize(data, ItemWasCollided);

                itemPool.Add(newItem);
            }
            deactiveDroppedItems.Add(itemDatas[itemIdx].ItemCode, itemPool);
        }
    }

    private void CreateDropCoin(int createCount)
    {
        for (int i = 0; i < createCount; ++i)
        {
            GameObject foundPrefab = Resources.Load<GameObject>($"Object/Gold/{goldData.GoldPrefab}");
            DroppedCoin newCoin = Instantiate(foundPrefab).GetComponent<DroppedCoin>();
            newCoin.transform.parent = droppedItemPoolObj;
            newCoin.gameObject.SetActive(false);
            newCoin.Initialize(CoinWasCollided);

            deactiveDroppedCoins.Add(newCoin);
        }
    }

    // Callback
    private void CoinWasCollided(DroppedCoin coin)
    {
        deactiveDroppedCoins.Add(coin);
    }
    private void ItemWasCollided(DroppedItem item, int key)
    {
        List<DroppedItem> pool;
        if (!deactiveDroppedItems.TryGetValue(key, out pool))
        {
            Debug.Log($"ItemDropper 오류 : DropItem Dictionary -> {key} Key에 List<DropItem> 이 존재하지 않음, 플레이어와 충돌한 DroppedItem Pool에 되돌리기 실패");
            return;
        }
        pool.Add(item);
    }
}
