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
    public float DeathAnimLength;
    public Vector3 SpawnCoord;
    private Action<GameObject> deathCallback;

    // Controller
    public MonsterStat Stat;
    public MonsterPanelController PanelController;
    public Animator MobAnimator;
    public ItemDropper Dropper;
    private MonsterAttackSystem[] attackSystems;
    private IAIBehaviour currentAI;

    public void Initialize(Action<GameObject> deathCallback)
    {
        this.deathCallback = deathCallback;
        Stat.Initialize();
        Dropper.Initialize(transform.parent, Stat.CurrentData.DropItemDatas, Stat.CurrentData.GoldData);
        PanelController.Initialize();
        InitializeAttackSystems();

        currentAI = GetComponent<IAIBehaviour>();
        currentAI.Initialize();
    }
    public void Respawn()
    {
        PanelController.Respawn();
        currentAI.Respawn();
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
            Debug.Log("AttackSystem Instance(현재 몬스터의 공격 패턴 개수)가 1개 이상입니다 ExecuteAttack(int) 오버로드를 사용하세요");
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
            Debug.Log($"ExecuteAttack 예외, index : {index}에 해당하는 AttackSystem이 존재하지 않습니다.");
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
        MobAnimator.SetTrigger("Death");
        PanelController.Death();
        Dropper.Death();
        currentAI.Death();
        PlayerStat.Instance.GainExperience(Stat.CurrentData.Experience);
        Invoke("ExecuteDeathFunction", DeathAnimLength);
    }
    private void ExecuteDeathFunction()
    {
        deathCallback(gameObject);
        gameObject.SetActive(false);
        PlayerQuest.Instance.KilledMonster(MonsterCode);
    }
}
