using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponBehaviour_OneHand : MonoBehaviour, IWeaponBehaviour
{
    // Constant
    private const float ONEHANDATTACK_LENGTH = 0.833f;

    // Controller
    private Animator playerAnimator;
    private PlayerAttack_Instant playerAttackBox;
    private TrailRenderer trailRenderer;

    // Data
    private Transform playerTransform;
    private WeaponData weaponData;
    private Action attackEnd_SendToAttackController;
    private float animEndTime;

    public void CreateBehaviour(WeaponData weaponData, Action attackEndCallback)
    {
        this.weaponData = weaponData;
        this.attackEnd_SendToAttackController = attackEndCallback;
        trailRenderer = transform.GetChild(0).GetComponent<TrailRenderer>();
        trailRenderer.gameObject.SetActive(false);

        playerTransform = GameObject.Find("Player").transform;
        playerAnimator = playerTransform.GetComponent<Animator>();

        // Create ColiderBox
        GameObject newColiderBox = new GameObject("PlayerWeaponColiderBox");
        newColiderBox.transform.parent = PlayerActManager.Instance.transform;
        playerAttackBox = newColiderBox.AddComponent<PlayerAttack_Instant>();
        playerAttackBox.tag = "Player_WeaponColBox";
        playerAttackBox.Initialize(PlayerActManager.Instance.transform, new Vector3(0, 1, 0.5f), weaponData);
        playerAttackBox.gameObject.SetActive(false);
    }
    public void PlayAttack()
    {
        float attackSpeed = PlayerStat.Instance.GetStat("AttackSpeed");
        playerAnimator.speed = attackSpeed;
        animEndTime = ONEHANDATTACK_LENGTH / attackSpeed;
        playerAnimator.SetTrigger("Attack");
        trailRenderer.gameObject.SetActive(true);

        Invoke("CallEndCallback", animEndTime);
        Invoke("ExecuteAttack", weaponData.TriggerDelay / attackSpeed);
    }
    private void ExecuteAttack()
    {
        playerAttackBox.Execute();
    }
    private void CallEndCallback()
    {
        trailRenderer.gameObject.SetActive(false);
        attackEnd_SendToAttackController();
    }
    public void ReleaseBehaviour()
    {
        Destroy(playerAttackBox.gameObject);
    }
}
