using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MonsterAttackBehaviour
{
    [System.Serializable]
    public class MonsterPatternJSON
    {
        public int MonsterCode;
        public MonsterAttackPattern[] AttackPatterns;
    }
    [System.Serializable]
    public class MonsterAttackPattern // 패턴 하나에 해당
    {
        public string[] PatternProgressOrders;
        // Instant
        public int InstantAttackCount;
        public InstantAttackData[] InstantAttackDatas;

        // Projectile
        public int ProjectileAttackCount;
        public ProjectileAttackData[] ProjectileAttackDatas;
    }

    [System.Serializable]
    public class InstantAttackData
    {
        public string ColliderType;
        public Vector3 ColliderSize;
        public string ColliderRotation;
        public Vector3 ColliderPosition;
        public float AttackPoint;
        public float TriggerHoldTime;
    }

    [System.Serializable]
    public class ProjectileAttackData
    {
        public string ColliderType;
        public Vector3 ColliderSize;
        public string ColliderRotation;
        public Vector3 ShotPosition;
        public float AttackPoint;
        public float Velocity;
    }
}
