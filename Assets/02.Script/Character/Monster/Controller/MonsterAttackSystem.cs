using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonsterAttackBehaviour;
using System;

public class MonsterAttackSystem : MonoBehaviour    // 패턴 한개 구현
{
    public class ExecuteInfo
    {
        public float ExecuteDelay;
        public Action StartAction;
        public ExecuteInfo() { }
        public ExecuteInfo(float delay, Action act)
        {
            ExecuteDelay = delay;
            StartAction = act;
        }
    }
    private MonsterAttackPattern attackPattern;
    private MonsterAttack_Instant[] instantAttacks;
    private MonsterAttack_Projectile[] projectileAttacks;
    private List<ExecuteInfo> executeSequence;

    public void Initialize(MonsterAttackPattern pattern)
    {
        attackPattern = pattern;

        CreateInstantAttackObj();
        CreateProjectileAttackObj();
        InitializeExecuteSequence();
    }
    private void CreateInstantAttackObj()
    {
        InstantAttackData[] datas = attackPattern.InstantAttackDatas;
        instantAttacks = new MonsterAttack_Instant[datas.Length];
        for (int i = 0; i < datas.Length; ++i)
        {
            GameObject newCollider = new GameObject($"InstantAttackObj_{i}");
            instantAttacks[i] = newCollider.AddComponent<MonsterAttack_Instant>();
            newCollider.transform.parent = transform;

            Type colliderType = null;
            switch(datas[i].ColliderType)
            {
                case "BoxCollider":
                    colliderType = typeof(BoxCollider);
                    break;
                case "SphereCollider":
                    colliderType = typeof(SphereCollider);
                    break;
                case "CapsuleCollider":
                    colliderType = typeof(CapsuleCollider);
                    break;
            }
            if (colliderType == null)
            {
                Debug.Log($"몬스터 InstantAttackObj 생성 중 오류 : {datas[i].ColliderType} 존재하지 않음");
                return;
            }
            
            instantAttacks[i].Initialize(colliderType, datas[i].ColliderSize, datas[i].ColliderRotation, datas[i].ColliderPosition,
                datas[i].AttackPoint, datas[i].TriggerHoldTime, transform);
        }
    }
    private void CreateProjectileAttackObj()
    {
        ProjectileAttackData[] datas = attackPattern.ProjectileAttackDatas;
        projectileAttacks = new MonsterAttack_Projectile[datas.Length];
        for (int i = 0; i < datas.Length; ++i)
        {
            GameObject newCollider = new GameObject($"ProjectileAttackObj_{i}");
            projectileAttacks[i] = newCollider.AddComponent<MonsterAttack_Projectile>();
            newCollider.transform.parent = transform;

            Type colliderType = Type.GetType(datas[i].ColliderType);
            if (colliderType == null)
            {
                Debug.Log($"몬스터 ProjectileAttackObj 생성 중 오류 : {datas[i].ColliderType} 존재하지 않음");
                return;
            }
            Vector3 colRot = Vector3.zero;
            if (datas[i].ColliderRotation.Equals("Parallel"))
                colRot = transform.rotation.eulerAngles;
            else
            {
                string[] rotSTR = datas[i].ColliderRotation.Split(',');
                colRot.x = float.Parse(rotSTR[0]);
                colRot.y = float.Parse(rotSTR[1]);
                colRot.z = float.Parse(rotSTR[2]);
            }
            projectileAttacks[i].Initialize(colliderType, datas[i].ColliderSize, colRot, datas[i].ShotPosition,
                datas[i].AttackPoint, datas[i].Velocity);
        }
    }
    private void InitializeExecuteSequence()
    {
        executeSequence = new List<ExecuteInfo>();
        string[] orders = attackPattern.PatternProgressOrders;
        for (int i = 0; i < orders.Length; ++i)
        {
            ExecuteInfo newOrder = new ExecuteInfo();
            string[] splitedInfo = orders[i].Split(',');
            string patternType = splitedInfo[0];
            int patternIndex = int.Parse(splitedInfo[1]);
            float executeDelay = float.Parse(splitedInfo[2]);

            if (patternType.Equals("Instant"))
                newOrder.StartAction = instantAttacks[patternIndex].Execute;
            else if (patternType.Equals("Projectile"))
                newOrder.StartAction = projectileAttacks[patternIndex].Execute;
            newOrder.ExecuteDelay = executeDelay;

            executeSequence.Add(newOrder);
        }
    }

    public void ExecuteAttack()
    {
        StartCoroutine(IE_ExecutePatternByOrder());
    }
    private IEnumerator IE_ExecutePatternByOrder()
    {
        Transform monsterRoot = transform.parent.parent;
        switch (attackPattern.MonsterRotation)
        {
            case "LookPlayer":
                monsterRoot.LookAt(PlayerActManager.Instance.transform.position);
                monsterRoot.transform.rotation = Quaternion.Euler(0, monsterRoot.transform.rotation.eulerAngles.y, 0);
                break;
            default:
                string[] splitedRot = attackPattern.MonsterRotation.Split(',');
                Vector3 newRot = new Vector3(float.Parse(splitedRot[0]), float.Parse(splitedRot[1]), float.Parse(splitedRot[2]));
                monsterRoot.transform.rotation = Quaternion.Euler(newRot);
                break;
        }
        for (int i = 0; i < executeSequence.Count; ++i)
        {
            yield return new WaitForSeconds(executeSequence[i].ExecuteDelay);
            executeSequence[i].StartAction();
        }
    }
}
