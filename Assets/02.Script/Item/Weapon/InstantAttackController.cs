using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;

public class InstantAttackController : MonoBehaviour
{
    private Transform playerTransform;
    private Vector3 colliderSize;
    private Vector3 colliderPosition;
    private WeaponData data;
    private float lifeTime;

    public void Execute(float lifeTime)
    {
        Refresh();
        this.lifeTime = lifeTime;
        gameObject.SetActive(true);
        Invoke("TriggerOver", lifeTime / PlayerStat.Instance.GetStat("AttackSpeed"));
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
            other.GetComponent<MonsterController>().GetDamage(PlayerStat.Instance.GetStat("AttackPoint"));
        }
    }
}
