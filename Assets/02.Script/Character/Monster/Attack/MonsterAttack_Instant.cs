using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterAttack_Instant : MonoBehaviour
{
    // Data
    private Vector3 colliderRotation;
    private Vector3 colliderPosition;
    private Vector3 colliderSize;
    private float attackPoint;
    private float triggerHoldTime;
    private Transform mobTransform;
    private bool colliderRotationIsParallel;

    public void Initialize(Type colType, Vector3 colSize, string colRotation, Vector3 colPosition,
        float attackPoint, float triggerHoldTime, Transform mob)
    {
        gameObject.AddComponent(colType);
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
        colliderSize = colSize;

        Vector3 colRot = Vector3.zero;
        if (colRotation.Equals("Parallel"))
            colliderRotationIsParallel = true;
        else
        {
            string[] rotSTR = colRotation.Split(',');
            colRot.x = float.Parse(rotSTR[0]);
            colRot.y = float.Parse(rotSTR[1]);
            colRot.z = float.Parse(rotSTR[2]);
        }

        mobTransform = mob;
        colliderRotation = colRot;
        colliderPosition = colPosition;
        this.attackPoint = attackPoint;
        this.triggerHoldTime = triggerHoldTime;
        this.mobTransform = mob;

        Refresh();
        gameObject.SetActive(false);
    }
    private void Refresh()
    {
        transform.localScale = colliderSize;
        transform.localPosition = colliderPosition;
        if (!colliderRotationIsParallel)
            transform.rotation = Quaternion.Euler(colliderRotation);
        else
            transform.rotation = mobTransform.rotation;
    }
    public void Execute()
    {
        Refresh();
        gameObject.SetActive(true);
        StartCoroutine(IE_ExecuteTrigger());
    }
    private IEnumerator IE_ExecuteTrigger()
    {
        yield return new WaitForSeconds(triggerHoldTime);
        gameObject.SetActive(false);
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
