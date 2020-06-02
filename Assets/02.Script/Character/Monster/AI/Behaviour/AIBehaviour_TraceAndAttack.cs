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
    AttackPlayer,
    Death
};
public class AIBehaviour_TraceAndAttack : MonoBehaviour, IAIBehaviour
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
    private bool readyToNextSearch;
    private bool trackingPlayer;
    private bool attackingPlayer;

    public void Initialize()
    {
        CognitionArea.Initiailize(PlayerEnterInCognitionArea, PlayerExitInCognitionArea, TargetIdentifyRange);
        AttackArea.Initiailize(PlayerEnterInAttackArea, PlayerExitInAttackArea, Stat.CurrentData.AttackRange);

        SettingAgent();
    }
    public void Respawn()
    {
        StartAI();
    }
    public void Death()
    {
        aiState = AI_TraceAndAttack_State.Death;
    }
    private void StartAI()
    {
        aiState = AI_TraceAndAttack_State.SearchPlayer;
        StartCoroutine("FSM");
    }
    private void SettingAgent()
    {
        NavAgent.speed = Stat.MoveSpeed;
    }
    private IEnumerator FSM()
    {
        while (true)
        {
            yield return StartCoroutine(aiState.ToString());
        }
    }
    // Behaviours
    private IEnumerator SearchPlayer()
    {
        bool nowMoving = true;
        CognitionArea.gameObject.SetActive(true);
        Vector3 randMoveVec = new Vector3(Random.Range(-RandomMoveRange, RandomMoveRange), 0, Random.Range(-RandomMoveRange, RandomMoveRange));
        Vector3 newDest = transform.position + randMoveVec;
        NavAgent.SetDestination(newDest);
        MobAnimator.SetBool("Move", true);

        while (nowMoving && aiState == AI_TraceAndAttack_State.SearchPlayer) 
        {
            yield return new WaitForEndOfFrame();
            if (aiState != AI_TraceAndAttack_State.SearchPlayer)
                yield break;
            if (AgentIsArrivedOnTarget()) 
            {
                nowMoving = false;
                MobAnimator.SetBool("Move", false);

                readyToNextSearch = false;
                Invoke("WaitTimeForNextSearch", 3f);
                yield return new WaitUntil(() => readyToNextSearch);
            }
        }
    }
    private void WaitTimeForNextSearch()
    {
        readyToNextSearch = true;
    }
    private IEnumerator TracePlayer()
    {
        AttackArea.gameObject.SetActive(true);
        trackingPlayer = true;
        MobAnimator.SetBool("Move", true);

        while (trackingPlayer && aiState == AI_TraceAndAttack_State.TracePlayer)
        {
            yield return new WaitForEndOfFrame();
            if (aiState != AI_TraceAndAttack_State.TracePlayer)
                yield break;
            NavAgent.SetDestination(player.position);
        }
    }
    private IEnumerator ReturnSpawnCoord()
    {
        bool nowReturing = true;
        CognitionArea.gameObject.SetActive(false);
        AttackArea.gameObject.SetActive(false);
        NavAgent.SetDestination(Controller.SpawnCoord);
        MobAnimator.SetBool("Move", true);

        while (nowReturing && aiState == AI_TraceAndAttack_State.ReturnSpawnCoord)
        {
            yield return new WaitForEndOfFrame();
            if (aiState != AI_TraceAndAttack_State.ReturnSpawnCoord)
                yield break;
            if (AgentIsArrivedOnTarget())
            {
                nowReturing = false;
                aiState = AI_TraceAndAttack_State.SearchPlayer;
            }
        }
    }
    private IEnumerator AttackPlayer()
    {
        attackingPlayer = true;
        while (attackingPlayer)
        {
            NavAgent.isStopped = true;
            NavAgent.ResetPath();
            NavAgent.velocity = Vector3.zero;
            MobAnimator.SetBool("Move", false);
            if (aiState == AI_TraceAndAttack_State.Death)
                yield break;
            MobAnimator.SetBool("Attack", true);
            Controller.ExecuteAttack();
            yield return new WaitForSeconds(AttackActionLength);
            if (aiState == AI_TraceAndAttack_State.AttackPlayer)
            {
                MobAnimator.SetBool("Attack", false);
            }
        }
        NavAgent.isStopped = false;
    }
    private bool AgentIsArrivedOnTarget()
    {
        if ((NavAgent.remainingDistance <= NavAgent.stoppingDistance))
            return true;
        else
            return false;
    }

    // Callback
    private void PlayerEnterInCognitionArea(Transform player)
    {
        this.player = player;
        readyToNextSearch = true;
        aiState = AI_TraceAndAttack_State.TracePlayer;
    }
    private void PlayerExitInCognitionArea()
    {
        trackingPlayer = false;
        aiState = AI_TraceAndAttack_State.ReturnSpawnCoord;
        
    }
    private void PlayerEnterInAttackArea(Transform player)
    {
        this.player = player;
        aiState = AI_TraceAndAttack_State.AttackPlayer;
    }
    private void PlayerExitInAttackArea()
    {
        attackingPlayer = false;
        MobAnimator.SetBool("Attack", false);
        aiState = AI_TraceAndAttack_State.TracePlayer;
    }
}
