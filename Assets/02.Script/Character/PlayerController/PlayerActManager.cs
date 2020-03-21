using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActManager : MonoBehaviour
{
    private IWeaponBehaviour equipedWeaponBehaviour;

    public void AttackOnEquipmentWeapon()
    {
        equipedWeaponBehaviour.PlayAttack();
    }
}
