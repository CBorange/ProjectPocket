using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterAttack_Projectile : MonoBehaviour
{
    // Data
    private Vector3 colliderRotation;
    private Vector3 colliderPosition;
    private Vector3 colliderSize;
    private float attackPoint;
    private float velocity;

    private void FixedUpdate()
    {
        Move();
    }
    public void Initialize(Type colType, Vector3 colSize, Vector3 colRotation, Vector3 shotPosition,
        float attackPoint, float velocity)
    {
        gameObject.AddComponent(colType);
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
        colliderSize = colSize;
        colliderRotation = colRotation;
        colliderPosition = shotPosition;
        this.attackPoint = attackPoint;
        this.velocity = velocity;

        Refresh();
        gameObject.SetActive(false);
    }
    private void Refresh()
    {
        transform.localScale = colliderSize;
        transform.localPosition = colliderPosition;
        transform.rotation = Quaternion.Euler(colliderRotation);
    }
    public void Execute()
    {
        Refresh();
        gameObject.SetActive(true);
    }
    private void Move()
    {
        transform.Translate(transform.forward * velocity * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("PLAYER"))
        {
            other.GetComponent<PlayerActManager>().GetDamage(attackPoint);
            gameObject.SetActive(false);
        }
    }
}
