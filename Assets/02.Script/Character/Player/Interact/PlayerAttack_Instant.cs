using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerAttack_Instant : MonoBehaviour
{
    private Transform playerTransform;
    private Vector3 colliderSize;
    private Vector3 colliderPosition;
    private WeaponData data;

    public void Execute()
    {
        Refresh();
        gameObject.SetActive(true);
        Invoke("TriggerOver", data.TriggerHold);
    }
    public void Initialize(Transform playerTrans, Vector3 colPos, WeaponData data)
    {
        playerTransform = playerTrans;
        this.data = data;

        colliderSize = new Vector3(1, 2, data.Range);
        colliderPosition = colPos;

        gameObject.AddComponent<BoxCollider>();
        GetComponent<BoxCollider>().isTrigger = true;
    }
    private void Refresh()
    {
        transform.localScale = colliderSize;
        transform.localPosition = colliderPosition;
        transform.rotation = playerTransform.rotation;
        return;
    }
    private void TriggerOver()
    {
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Monster"))
        {
            other.GetComponent<MonsterStat>().GetDamage(PlayerStat.Instance.AttackPoint);
            gameObject.SetActive(false);
        }
    }
}
