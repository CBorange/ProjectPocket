using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIMachineLibrary;
using MonsterAttackBehaviour;
using System;

public class MonsterController : MonoBehaviour, IActController
{
    // Data
    public int MonsterCode;
    public Vector3 SpawnCoord;

    // Controller
    public MonsterStat Stat;
    public MonsterPanelController PanelController;
    private MonsterAttackSystem[] attackSystems;

    public void Initialize()
    {
        Stat.Initialize();
        PanelController.Initialize();
        InitializeAttackSystems();
    }
    private void InitializeAttackSystems()
    {
        MonsterPatternJSON patternJSON = MonsterDB.Instance.GetMonsterPattern(MonsterCode);
        MonsterAttackPattern[] patterns = patternJSON.AttackPatterns;
        attackSystems = new MonsterAttackSystem[patterns.Length];

        GameObject AttackSystemObj = new GameObject("AttackSystems");
        AttackSystemObj.transform.parent = transform;
        AttackSystemObj.transform.localPosition = Vector3.zero;
        for (int i = 0; i < patterns.Length; ++i)
        {
            GameObject newSystem = new GameObject($"System_{i}");
            newSystem.transform.parent = AttackSystemObj.transform;
            newSystem.transform.localPosition = Vector3.zero;
            newSystem.AddComponent<MonsterAttackSystem>();
            MonsterAttackSystem newSystemComponent = newSystem.GetComponent<MonsterAttackSystem>();
            newSystemComponent.Initialize(patterns[i]);
            attackSystems[i] = newSystemComponent;
        }
    }

    // Method For Attack
    public void ExecuteAttack()
    {
        if (attackSystems.Length > 1)
            Debug.Log("AttackSystem Instance가 1개 이상입니다 ExecuteAttack(int) 오버로드를 사용하세요");
        attackSystems[0].ExecuteAttack();
    }
    public void ExecuteAttack(int index)
    {
        try
        {
            attackSystems[index].ExecuteAttack();
        }
        catch(Exception)
        {
            Debug.Log($"ExecuteAttack 예외, index 유효하지 않습니다.");
        }
    }
    public void EndAttack()
    {

    }
    // Method Communicate With MonsterStat
    public void GetDamage(float ap)
    {
        Stat.GetDamage(ap);
    }
    public void CharacterDeath()
    {
        
    }
}
