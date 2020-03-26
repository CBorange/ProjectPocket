using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWeaponBehaviour
{
    void CreateBehaviour(WeaponData weaponData);
    void PlayAttack();
    void ReleaseBehaviour();
}
