using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharacterStat
{
    // Max(Original) Stat
    float Origin_MoveSpeed { get; }
    float Origin_JumpSpeed { get; }
    float Max_HealthPoint { get; }
    float Origin_ShieldPoint { get; }
    float Origin_AttackPoint { get; }
    float Origin_AttackSpeed { get; }

    // Current Stat
    float MoveSpeed { get;}
    float JumpSpeed { get; }
    float HealthPoint { get; }
    float ShieldPoint { get;}
    float AttackPoint { get;}
    float AttackSpeed { get;}
}
