using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharacterStat
{
    // Max(Original) Stat
    float Origin_MoveSpeed { get; }
    float Origin_JumpSpeed { get; }
    float Origin_MaxHealthPoint { get; }
    float Origin_MaxShieldPoint { get; }
    float Origin_AttackPoint { get; }
    float Origin_AttackSpeed { get; }

    // Current Stat
    float MoveSpeed { get;}
    float JumpSpeed { get; }
    float MaxHealthPoint { get; }
    float HealthPoint { get; }
    float ShieldPoint { get;}
    float AttackPoint { get;}
    float AttackSpeed { get;}
}
