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
    }
    private void OnEnable()
    {
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
        NavAgent.velocity = Vector3.zero;
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
        AttackArea.gameObject.SetActive(true);
        NavAgent.isStopped = false;
        trackingPlayer = true;
        MobAnimator.SetBool("Move", true);

        while (trackingPlayer && aiState == AI_TraceAndAttack_State.TracePlayer)
        {
            yield return new WaitForEndOfFrame();
            if (aiState != AI_TraceAndAttack_State.TracePlayer)
                yield break;
            NavAgent.SetDestination(player.position);
            if (AgentIsArrivedOnTarget())
            {
                NavAgent.isStopped = true;
            }
        }
    }
    private IEnumerator ReturnSpawnCoord()
    {
        bool nowReturing = true;
        CognitionArea.gameObject.SetActive(false);
        AttackArea.gameObject.SetActive(false);
        NavAgent.isStopped = false;
        NavAgent.velocity = Vector3.zero;
        NavAgent.SetDestination(Controller.SpawnCoord);
        MobAnimator.SetBool("Move", true);

        while (nowReturing)
        {
            yield return new WaitForEndOfFrame();
            if (AgentIsArrivedOnTarget())
            {
                NavAgent.isStopped = true;
                nowReturing = false;
                aiState = AI_TraceAndAttack_State.SearchPlayer;
                StartCoroutine("SearchPlayer");
            }
        }
    }
    private IEnumerator AttackPlayer()
    {
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
                (!NavAgent.hasPath))
            return true;
        else
            return false;
    }

    // Callback Call by Monster Cognition Colider
    private void PlayerEnterInCognitionArea(Transform player)
    {
        this.player = player;
        aiState = AI_TraceAndAttack_State.TracePlayer;
        StartCoroutine("TracePlayer");
    }
    private void PlayerExitInCognitionArea()
    {
        trackingPlayer = false;
        NavAgent.isStopped = true;
        aiState = AI_TraceAndAttack_State.ReturnSpawnCoord;
        StartCoroutine("ReturnSpawnCoord");
        
    }
    private void PlayerEnterInAttackArea(Transform player)
    {
        this.player = player;
        aiState = AI_TraceAndAttack_State.AttackPlayer;
        StartCoroutine("AttackPlayer");
    }
    private void PlayerExitInAttackArea()
    {
        attackingPlayer = false;
        MobAnimator.SetBool("Attack", false);
        aiState = AI_TraceAndAttack_State.TracePlayer;
        StartCoroutine("TracePlayer");
    }
}
