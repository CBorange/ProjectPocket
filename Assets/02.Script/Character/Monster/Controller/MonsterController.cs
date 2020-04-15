using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIMachineLibrary;

public class MonsterController : MonoBehaviour, ActController
{
    // Data
    public int MonsterCode;
    public Vector3 SpawnCoord;

    // Controller
    public MonsterStat Stat;
    public MonsterPanelController PanelController;
    public MonsterAttackSystem AttackSystem;

    public void Initialize()
    {
        Stat.Initialize();
        PanelController.Initialize();
        AttackSystem.Initialize();
    }

    // Method For Attack
    public void ExecuteAttack()
    {

    }
    public void ExecuteAttack(int index)
    { 

    }
    public void EndAttack()
    {

    }
    // Method Communicate With MonsterStat
    public void GetDamage()
    {

    }
    public void CharacterDeath()
    {
        
    }
}
