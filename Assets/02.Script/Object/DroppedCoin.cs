using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DroppedCoin : MonoBehaviour
{
    public Rigidbody MyBody;
    private int coinAmount;
    private Action<DroppedCoin> collisionCallback;

    public void Initialize(Action<DroppedCoin> collisionCallback)
    {
        this.collisionCallback = collisionCallback;
    }
    public void Drop(int coinAmount, Vector3 deathPos)
    {
        gameObject.SetActive(true);
        transform.position = new Vector3(deathPos.x, deathPos.y + 1, deathPos.z);
        this.coinAmount = coinAmount;

        Vector3 forceVec = Vector3.up * 4f;
        forceVec.x = UnityEngine.Random.Range(0.3f, 1f);
        forceVec.z = UnityEngine.Random.Range(0.3f, 1f);
        MyBody.AddForce(forceVec, ForceMode.Impulse);
        Invoke("ReleaseByTime", 30f);
    }
    private void ReleaseByTime()
    {
        if (!gameObject.activeSelf)
            return;
        gameObject.SetActive(false);
        collisionCallback(this);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag.Equals("PLAYER"))
        {
            PlayerStat.Instance.AddGold(coinAmount);
            gameObject.SetActive(false);
            PlayerActManager.Instance.GetItem();
            collisionCallback(this);
        }
    }
}
