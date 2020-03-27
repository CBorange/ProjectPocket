using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class WeaponBehaviour_OneHand : MonoBehaviour, IWeaponBehaviour
{
    private Transform playerTransform;
    private Animator playerAnimator;
    private BoxCollider coliderBox;
    private WeaponData weaponData;
    private Action attackEndCallback;

    public void CreateBehaviour(WeaponData weaponData, Action attackEndCallback)
    {
        this.weaponData = weaponData;
        this.attackEndCallback = attackEndCallback;

        playerTransform = GameObject.Find("Player").transform;
        playerAnimator = playerTransform.GetComponent<Animator>();

        // Create ColiderBox
        GameObject newColiderBox = new GameObject("PlayerWeaponColiderBox");
        coliderBox = newColiderBox.AddComponent<BoxCollider>();
        coliderBox.isTrigger = true;
        coliderBox.size = new Vector3(1, 2, weaponData.Range);
        coliderBox.gameObject.SetActive(false);
        coliderBox.tag = "Player_WeaponColBox";
    }
    public void PlayAttack()
    {
        playerAnimator.SetTrigger("OneHand_Attack");
        coliderBox.transform.rotation = playerTransform.rotation;
        coliderBox.transform.position = new Vector3(playerTransform.position.x, 1.45f, playerTransform.position.z) + (playerTransform.forward * 0.8f);
        coliderBox.gameObject.SetActive(true);
        StartCoroutine(IE_WaitAttackTime());
    }
    private IEnumerator IE_WaitAttackTime()
    {
        yield return new WaitForSeconds(0.75f);
        coliderBox.gameObject.SetActive(false);
        attackEndCallback();
    }
    public void ReleaseBehaviour()
    {
        Destroy(coliderBox.gameObject);
    }
}
