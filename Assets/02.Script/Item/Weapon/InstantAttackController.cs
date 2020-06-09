using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;

public class InstantAttackController : MonoBehaviour
{
    private Transform playerTransform;
    private WeaponData data;
    private float lifeTime;

    public void Execute(float lifeTime)
    {
        Refresh();
        this.lifeTime = lifeTime;
        gameObject.SetActive(true);
        Invoke("TriggerOver", lifeTime / PlayerStat.Instance.GetStat("AttackSpeed"));
    }
    public void Initialize(Transform playerTrans, WeaponData data)
    {
        playerTransform = playerTrans;
        this.data = data;
    }
    private void Refresh()
    {
        transform.localScale = new Vector3(1.2f, 1.5f, data.Range);
        transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y + 1, playerTransform.position.z)
            + (playerTransform.forward * data.Range / 2);
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
