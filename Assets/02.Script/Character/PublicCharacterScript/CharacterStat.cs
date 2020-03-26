using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharacterStat
{
    // Max(Original) Stat
    float Origin_MoveSpeed { get; set; }
    float Origin_JumpSpeed { get; set; }
    float Max_HealthPoint { get; set; }
    float Origin_ShieldPoint { get; set; }
    float Origin_AttackPoint { get; set; }
    float Origin_AttackSpeed { get; set; }

    // Current Stat
    float MoveSpeed { get; set;}
    float JumpSpeed { get; set; }
    float HealthPoint { get; set; }
    float ShieldPoint { get; set; }
    float AttackPoint { get; set; }
    float AttackSpeed { get; set; }
}
