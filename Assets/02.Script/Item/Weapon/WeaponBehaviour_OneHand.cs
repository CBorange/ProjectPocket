using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponBehaviour_OneHand : MonoBehaviour, IWeaponBehaviour
{
    // Constant
    private const float ONEHANDATTACK_LENGTH = 1.167f;

    // Controller
    private Animator playerAnimator;
    private PlayerAttack_Instant playerAttack;

    // Data
    private Transform playerTransform;
    private WeaponData weaponData;
    private Action attackEndCallback;
    private float animEndTime;

    public void CreateBehaviour(WeaponData weaponData, Action attackEndCallback)
    {
        this.weaponData = weaponData;
        this.attackEndCallback = attackEndCallback;

        playerTransform = GameObject.Find("Player").transform;
        playerAnimator = playerTransform.GetComponent<Animator>();

        // Create ColiderBox
        GameObject newColiderBox = new GameObject("PlayerWeaponColiderBox");
        newColiderBox.transform.parent = PlayerActManager.Instance.transform;
        playerAttack = newColiderBox.AddComponent<PlayerAttack_Instant>();
        playerAttack.Initialize(PlayerActManager.Instance.transform, new Vector3(0, 1, 1), weaponData);
    }
    public void PlayAttack()
    {
        playerAnimator.speed = PlayerStat.Instance.AttackSpeed;
        animEndTime = ONEHANDATTACK_LENGTH / PlayerStat.Instance.AttackSpeed;
        playerAnimator.SetTrigger("Attack");

        Invoke("CallEndCallback", animEndTime);
        Invoke("ExecuteAttack", weaponData.TriggerDelay);
    }
    private void ExecuteAttack()
    {
        playerAttack.Execute();
    }
    private void CallEndCallback()
    {
        attackEndCallback();
    }
    public void ReleaseBehaviour()
    {
        Destroy(playerAttack.gameObject);
    }
}
