using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IActController
{
    void ExecuteAttack();
    void EndAttack();
    void CharacterDeath();
    void GetDamage(float ap);
}
