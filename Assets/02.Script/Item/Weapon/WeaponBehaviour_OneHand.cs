using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour_OneHand : IWeaponBehaviour
{
    private Transform playerTransform;
    private BoxCollider coliderBox;
    private Animator playerAnimator;
    private WeaponData weaponData;

    public void CreateBehaviour(WeaponData weaponData)
    {
        this.weaponData = weaponData;

        GameObject newColiderBox = new GameObject("PlayerWeaponColiderBox");
        coliderBox = newColiderBox.AddComponent<BoxCollider>();
        coliderBox.isTrigger = true;
        coliderBox.size = new Vector3(1, 1, weaponData.Range);
        coliderBox.gameObject.SetActive(false);

        playerTransform = GameObject.Find("Player").transform;
    }
    public void PlayAttack()
    {
        Debug.Log(weaponData.Name);
    }
    public void ReleaseBehaviour()
    {
        GameObject.Destroy(coliderBox.gameObject);
    }
}
