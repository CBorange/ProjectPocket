using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIMachineLibrary;
using UnityEngine.AI;

public enum AI_TraceAndAttack_State
{
    SearchPlayer,
    TracePlayer,
    ReturnSpawnCoord,
    AttackPlayer
};
public class AIBehaviour_TraceAndAttack : MonoBehaviour
{
    // Controller
    public Animator MobAnimator;
    public MonsterController Controller;
    public MonsterStat Stat;
    public MonsterSearchColider CognitionArea;
    public MonsterSearchColider AttackArea;
    public NavMeshAgent NavAgent;

    // Parameter For Behaviour
    private AI_TraceAndAttack_State aiState;

    // Data
    public float TargetIdentifyRange;
    public float RandomMoveRange;
    public float AttackActionLength;
    private Transform player;

    // Parameter For AI State
    private bool trackingPlayer;
    private bool attackingPlayer;

    private void Awake()
    {
        Controller.Initialize();

        CognitionArea.Initiailize(PlayerEnterInCognitionArea, PlayerExitInCognitionArea, TargetIdentifyRange);
        AttackArea.Initiailize(PlayerEnterInAttackArea, PlayerExitInAttackArea, Stat.CurrentData.AttackRange);

        SettingAgent();
        StartAI();
    }
    private void StartAI()
    {
        aiState = AI_TraceAndAttack_State.SearchPlayer;
        StartCoroutine("SearchPlayer");
    }
    private void SettingAgent()
    {
        NavAgent.speed = Stat.MoveSpeed;
    }
    // Behaviours
    private IEnumerator SearchPlayer()
    {
        Debug.Log("SearchPlayer");
        bool nowMoving = true;
        CognitionArea.gameObject.SetActive(true);
        Vector3 randMoveVec = new Vector3(Random.Range(-RandomMoveRange, RandomMoveRange), 0, Random.Range(-RandomMoveRange, RandomMoveRange));
        Vector3 newDest = transform.position + randMoveVec;
        NavAgent.isStopped = false;
        NavAgent.SetDestination(newDest);
        MobAnimator.SetBool("Move", true);

        while (nowMoving && aiState == AI_TraceAndAttack_State.SearchPlayer) 
        {
            yield return new WaitForEndOfFrame();
            if (AgentIsArrivedOnTarget()) 
            {
                nowMoving = false;
                NavAgent.isStopped = true;
                MobAnimator.SetBool("Move", false);
                yield return new WaitForSeconds(3f);
                if (aiState == AI_TraceAndAttack_State.SearchPlayer)
                    StartCoroutine("SearchPlayer");
            }
        }
    }
    private IEnumerator TracePlayer()
    {
        Debug.Log("Start TracePlayer");
        AttackArea.gameObject.SetActive(true);
        NavAgent.isStopped = false;
        trackingPlayer = true;
        MobAnimator.SetBool("Move", true);

        while (trackingPlayer)
        {
            yield return new WaitForEndOfFrame();
            if (AgentIsArrivedOnTarget())
            {
                NavAgent.isStopped = true;
            }
            NavAgent.SetDestination(player.position);
        }
    }
    private IEnumerator ReturnSpawnCoord()
    {
        Debug.Log($"Start ReturnSpawnCoord : {Controller.SpawnCoord}");

        bool nowReturing = true;
        CognitionArea.gameObject.SetActive(false);
        AttackArea.gameObject.SetActive(false);
        NavAgent.isStopped = false;
        NavAgent.SetDestination(Controller.SpawnCoord);
        MobAnimator.SetBool("Move", true);

        while (nowReturing)
        {
            yield return new WaitForEndOfFrame();
            if (AgentIsArrivedOnTarget())
            {
                NavAgent.isStopped = true;
                nowReturing = false;
                StartCoroutine("SearchPlayer");
            }
        }
    }
    private IEnumerator AttackPlayer()
    {
        Debug.Log("Start AttackPlayer");
        NavAgent.isStopped = true;
        attackingPlayer = true;
        MobAnimator.SetBool("Attack", true);
        while (attackingPlayer)
        {
            Controller.ExecuteAttack();
            yield return new WaitForSeconds(AttackActionLength);
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

    // Callback Call by Monster Cognition Colider
    private void PlayerEnterInCognitionArea(Transform player)
    {
        this.player = player;
        StartCoroutine("TracePlayer");
        aiState = AI_TraceAndAttack_State.TracePlayer;
    }
    private void PlayerExitInCognitionArea()
    {
        trackingPlayer = false;
        StartCoroutine("ReturnSpawnCoord");
        aiState = AI_TraceAndAttack_State.ReturnSpawnCoord;
    }
    private void PlayerEnterInAttackArea(Transform player)
    {
        this.player = player;
        StartCoroutine("AttackPlayer");
        aiState = AI_TraceAndAttack_State.AttackPlayer;
    }
    private void PlayerExitInAttackArea()
    {
        MobAnimator.SetBool("Attack", false);
        StartCoroutine("TracePlayer");
        attackingPlayer = false;
    }
}
