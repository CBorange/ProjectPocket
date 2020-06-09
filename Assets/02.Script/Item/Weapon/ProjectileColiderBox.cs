using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ProjectileColiderBox : MonoBehaviour
{
    private Rigidbody myRigidBody;
    private TrailRenderer trailRenderer;
    private Action<ProjectileColiderBox> returnToPoolCallback;

    public void Initialize(Color waveColor, Action<ProjectileColiderBox> returnCallback)
    {
        myRigidBody = GetComponent<Rigidbody>();
        trailRenderer = transform.GetChild(0).GetComponent<TrailRenderer>();
        trailRenderer.startColor = waveColor;
        returnToPoolCallback = returnCallback;

        gameObject.SetActive(false);
    }
    public void Fire(Vector3 dir, float velocity)
    {
        gameObject.SetActive(true);
        myRigidBody.velocity = Vector3.zero;
        myRigidBody.AddForce(dir * velocity, ForceMode.Impulse);
        Invoke("ReturnToPool", 10f);
    }
    public void ForceReturnToPool()
    {
        if (!gameObject.activeSelf)
            return;
        ReturnToPool();
    }
    private void ReturnToPool()
    {
        gameObject.SetActive(false);
        returnToPoolCallback(this);
    }
    public void Refresh(Transform playerTrans)
    {
        transform.localScale = new Vector3(1.2f, 1.5f, 2);
        transform.position = new Vector3(playerTrans.position.x, playerTrans.position.y + 1, playerTrans.position.z) + (playerTrans.forward * 1);
        transform.rotation = playerTrans.rotation;

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Monster"))
        {
            other.GetComponent<MonsterController>().GetDamage(PlayerStat.Instance.GetStat("AttackPoint"));
            ReturnToPool();
        }
    }
}
