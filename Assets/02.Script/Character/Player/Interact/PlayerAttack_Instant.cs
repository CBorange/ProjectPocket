using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack_Instant : MonoBehaviour
{
    private Transform playerTransform;
    private Vector3 colliderSize;
    private Vector3 colliderPosition;
    private float attackPoint;
    private float attackSpeed;

    public void Execute()
    {
        Refresh();
        gameObject.SetActive(true);
    }
    public void Initialize(Transform playerTrans, Vector3 colSize, Vector3 colPos, float attackPoint)
    {
        playerTransform = playerTrans;
        colliderSize = colSize;
        colliderPosition = colPos;
        this.attackPoint = attackPoint;

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
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Monster"))
        {
            other.GetComponent<MonsterStat>().GetDamage(attackPoint);
            gameObject.SetActive(false);
        }
    }
}
