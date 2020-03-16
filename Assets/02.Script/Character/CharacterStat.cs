using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface CharacterStat
{
    // Max(Original) Stat
    float MaxMoveSpeed { get; set; }
    float MaxJumpSpeed { get; set; }
    float MaxHealthPoint { get; set; }
    float MaxShieldPoint { get; set; }

    // Current Stat
    float MoveSpeed { get; set;}
    float JumpSpeed { get; set; }
    float HealthPoint { get; set; }
    float ShieldPoint { get; set; }
}
