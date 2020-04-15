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
    

    private void Awake()
    {
        Controller.Initialize();

        CognitionArea.Initiailize(PlayerEnterInCognitionArea, PlayerExitInCognitionArea, TargetIdentifyRange);
        AttackArea.Initiailize(PlayerEnterInAttackArea, PlayerExitInAttackArea, Stat.CurrentData.AttackRange);

        Machine.Initialize();
        SettingAgent();
        SettingMachine();
    }
    private void SettingAgent()
    {
        NavAgent.speed = Stat.MoveSpeed;
    }
    private void SettingMachine()
    {
        // Add Action/Transition
        Machine.AddAction("SearchPlayer", IE_SearchPlayer(), true);
        Machine.AddAction("TracePlayer", IE_TracePlayer(), false);
        Machine.AddAction("ReturnSpawnCoord", IE_ReturnSpawnCoord(), false);

        Machine.AddTransition("SearchPlayer", "A1", "TracePlayer");
        Machine.AddTransition("TracePlayer", "B1", "ReturnSpawnCoord");
        Machine.AddTransition("ReturnSpawnCoord", "B2", "SearchPlayer");

        Machine.AddBool("EnemyIdentified", false);
        Machine.AddBool("EnemyInRange", false);

        // Connect Transition
        Machine.ConnectBoolOnTransition("SearchPlayer", "EnemyIdentified", true, "A1");
        Machine.ConnectBoolOnTransition("TracePlayer", "EnemyIdentified", false, "B1");

        // Start
        Machine.SetEntryAction("SearchPlayer");
        Machine.StartAI();
    }

    // Callback Call by Monster Cognition Colider
    private void PlayerEnterInCognitionArea(Transform player)
    {
        this.player = player;
        Machine.SetBool("EnemyIdentified", true);
    }
    private void PlayerExitInCognitionArea()
    {
        Machine.SetBool("EnemyIdentified", false);
    }
    private void PlayerEnterInAttackArea(Transform player)
    {
        this.player = player;
        
    }
    private void PlayerExitInAttackArea()
    {
    }

    // Behaviours
    private IEnumerator IE_SearchPlayer()
    {
        Debug.Log("Start SearchPlayer");
        CognitionArea.gameObject.SetActive(true);
        Vector3 randMoveVec = new Vector3(Random.Range(-RandomMoveRange, RandomMoveRange), 0, Random.Range(-RandomMoveRange, RandomMoveRange));
        Vector3 newDest = transform.position + randMoveVec;
        NavAgent.isStopped = false;
        NavAgent.SetDestination(newDest);
        while (true)
        {
            if (AgentIsArrivedOnTarget()) 
            {
                NavAgent.isStopped = true;
                yield return new WaitForSeconds(3f);
                Machine.EndAction(IE_SearchPlayer());
            }
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator IE_TracePlayer()
    {
        Debug.Log("Start TracePlayer");
        AttackArea.gameObject.SetActive(true);
        NavAgent.isStopped = false;
        while (true)
        {
            if (AgentIsArrivedOnTarget())
            {
                NavAgent.isStopped = true;
                Machine.EndAction(IE_TracePlayer());
            }
            NavAgent.SetDestination(player.position);
            yield return new WaitForEndOfFrame();
        }
    }
    private IEnumerator IE_ReturnSpawnCoord()
    {
        Debug.Log($"Start ReturnSpawnCoord : {Controller.SpawnCoord}");
        CognitionArea.gameObject.SetActive(false);
        AttackArea.gameObject.SetActive(false);
        NavAgent.isStopped = false;
        NavAgent.SetDestination(Controller.SpawnCoord);
        while (true)
        {
            if (AgentIsArrivedOnTarget())
            {
                NavAgent.isStopped = true;
                Machine.EndAction(IE_ReturnSpawnCoord());
            }
            yield return new WaitForEndOfFrame();
        }
    }
    private bool AgentIsArrivedOnTarget()
    {
        if ((!NavAgent.pathPending) &&
                (NavAgent.remainingDistance <= NavAgent.stoppingDistance) &&
                (!NavAgent.hasPath || NavAgent.velocity.sqrMagnitude == 0f))
            return true;
        else
            return false;
    }
}
