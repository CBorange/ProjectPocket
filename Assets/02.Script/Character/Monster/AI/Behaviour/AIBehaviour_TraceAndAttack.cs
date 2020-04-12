using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIMachineLibrary;
using UnityEngine.AI;

public class AIBehaviour_TraceAndAttack : MonoBehaviour
{
    // Controller
    public AIMachine Machine;
    public MonsterController Controller;
    public MonsterStat Stat;
    public MonsterSearchColider CognitionArea;
    public MonsterSearchColider AttackArea;

    public NavMeshAgent NavAgent;

    // Parameter For Behaviour
    public float TargetIdentifyRange;
    public float RandomMoveRange;
    // Data
    private Transform player;
    

    private void Start()
    {
        CognitionArea.Initiailize(PlayerEnterInCognitionArea, PlayerExitInCognitionArea, TargetIdentifyRange);
        AttackArea.Initiailize(PlayerEnterInAttackArea, PlayerExitInAttackArea, Stat.CurrentData.AttackRange);

        Machine.Initialize();
        SettingMachine();
    }
    private void SettingMachine()
    {
        Machine.AddAction("SearchPlayer", new AIAction(IE_Cycle_SearchPlayer(), false, "SearchPlayer"));
        Machine.SetEntryAction("SearchPlayer");
        Machine.StartAI();
    }

    // Callback Call by Monster Cognition Colider
    private void PlayerEnterInCognitionArea(Transform player)
    {
        this.player = player;
    
    }
    private void PlayerExitInCognitionArea()
    {

    }
    private void PlayerEnterInAttackArea(Transform player)
    {
        this.player = player;
    }
    private void PlayerExitInAttackArea()
    {
    }

    // Behaviours
    private IEnumerator IE_Cycle_SearchPlayer()
    {
        CognitionArea.gameObject.SetActive(true);
        while (true)
        {
            Vector3 randMoveVec = new Vector3(Random.Range(-RandomMoveRange, RandomMoveRange), 0, Random.Range(-RandomMoveRange, RandomMoveRange));
            Vector3 newDest = transform.position + randMoveVec;
            NavAgent.isStopped = false;
            NavAgent.SetDestination(newDest);
            while (true)
            {
                if ((!NavAgent.pathPending) &&
                    (NavAgent.remainingDistance <= NavAgent.stoppingDistance) &&
                    (!NavAgent.hasPath || NavAgent.velocity.sqrMagnitude == 0f)) 
                {
                    NavAgent.isStopped = true;
                    yield return new WaitForSeconds(3f);
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
