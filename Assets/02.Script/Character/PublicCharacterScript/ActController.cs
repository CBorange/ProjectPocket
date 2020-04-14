using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ActController
{
    void ExecuteAttack();
    void EndAttack();
    void CharacterDeath();
    void GetDamage();
}
