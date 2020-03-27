using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface IWeaponBehaviour
{
    void CreateBehaviour(WeaponData weaponData, Action attackEndCallback);
    void PlayAttack();
    void ReleaseBehaviour();
}
