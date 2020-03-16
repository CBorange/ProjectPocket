using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour, CharacterStat
{
    // Character Max Stat
    private float maxMoveSpeed;
    public float MaxMoveSpeed
    {
        get { return maxMoveSpeed; }
        set { maxMoveSpeed = value; }
    }

    private float maxJumpSpeed;
    public float MaxJumpSpeed
    {
        get { return maxJumpSpeed; }
        set { maxJumpSpeed = value; }
    }

    private float maxHealthPoint;
    public float MaxHealthPoint
    {
        get { return maxHealthPoint; }
        set { maxHealthPoint = value; }
    }

    private float maxShieldPoint;
    public float MaxShieldPoint
    {
        get { return maxShieldPoint; }
        set { maxShieldPoint = value; }
    }

    // Character Current Stat
    private float moveSpeed;
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set { moveSpeed = value; }
    }

    private float jumpSpeed;
    public float JumpSpeed
    {
        get { return jumpSpeed; }
        set { jumpSpeed = value; }
    }

    private float healthPoint;
    public float HealthPoint
    {
        get { return healthPoint; }
        set { healthPoint = value; }
    }

    private float shieldPoint;
    public float ShieldPoint
    {
        get { return shieldPoint; }
        set { shieldPoint = value; }
    }

    private void Start()
    {
        MoveSpeed = 5f;
    }
}
