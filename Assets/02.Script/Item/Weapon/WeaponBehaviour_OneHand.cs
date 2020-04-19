using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponBehaviour_OneHand : MonoBehaviour, IWeaponBehaviour
{
    private Transform playerTransform;
    private Animator playerAnimator;
    private WeaponData weaponData;
    private Action attackEndCallback;
    private PlayerAttack_Instant playerAttack;

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
        playerAttack.Initialize(PlayerActManager.Instance.transform, new Vector3(1.5f, 2, weaponData.Range), new Vector3(0, 1, 1), weaponData.AttackPoint);
    }
    public void PlayAttack()
    {
        playerAnimator.SetTrigger("OneHand_Attack");
        playerAttack.Execute();
    }
    public void ReleaseBehaviour()
    {
        Destroy(playerAttack.gameObject);
    }
}
