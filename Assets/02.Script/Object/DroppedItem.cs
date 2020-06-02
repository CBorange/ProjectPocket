using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DroppedItem : MonoBehaviour
{
    public Rigidbody MyBody;
    private ItemData currentItem;
    private Action<DroppedItem, int> collisionCallback;

    public void Initialize(ItemData data, Action<DroppedItem, int> collisionCallback)
    {
        this.collisionCallback = collisionCallback;
        currentItem = data;
    }
    public void Drop(Vector3 deathPos)
    {
        gameObject.SetActive(true);
        transform.position = new Vector3(deathPos.x, deathPos.y + 3, deathPos.z);

        Vector3 forceVec = Vector3.up * 3f;
        forceVec.x = UnityEngine.Random.Range(0.1f, 0.3f);
        forceVec.z = UnityEngine.Random.Range(0.1f, 0.3f);
        MyBody.AddForce(forceVec, ForceMode.Impulse);
        Invoke("ReleaseByTime", 30f);
    }
    private void ReleaseByTime()
    {
        if (!gameObject.activeSelf)
            return;
        gameObject.SetActive(false);
        collisionCallback(this, currentItem.ItemCode);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("PLAYER"))
        {
            InventoryItem inventoryItem = new InventoryItem(currentItem, 1);
            PlayerInventory.Instance.AddItemToInventory(inventoryItem);
            gameObject.SetActive(false);
            PlayerActManager.Instance.GetItem();
            collisionCallback(this, currentItem.ItemCode);
        }
    }
}
