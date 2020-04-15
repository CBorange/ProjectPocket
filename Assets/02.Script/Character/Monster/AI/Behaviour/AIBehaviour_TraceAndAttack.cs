using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AIMachineLibrary;
using UnityEngine.AI;

public enum AI_TraceAndAttack_State
{
    SearchPlayer,
    TracePlayer,
    ReturnSpawnCoord
};
public class AIBehaviour_TraceAndAttack : MonoBehaviour
{
    // Controller
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
    private Transform player;

    // Parameter For AI State
    private bool trackingPlayer;

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
        StartCoroutine("AI_FSM");
    }
    private void SettingAgent()
    {
        NavAgent.speed = Stat.MoveSpeed;
    }
    private IEnumerator AI_FSM()
    {
        while (true)
        {
            yield return StartCoroutine(aiState.ToString());
        }
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

        while (nowMoving && aiState == AI_TraceAndAttack_State.SearchPlayer) 
        {
            yield return new WaitForEndOfFrame();
            if (AgentIsArrivedOnTarget()) 
            {
                nowMoving = false;
                NavAgent.isStopped = true;
                yield return new WaitForSeconds(3f);
            }
        }
    }
    private IEnumerator TracePlayer()
    {
        Debug.Log("Start TracePlayer");
        AttackArea.gameObject.SetActive(true);
        NavAgent.isStopped = false;
        trackingPlayer = true;
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
        while (nowReturing)
        {
            yield return new WaitForEndOfFrame();
            if (AgentIsArrivedOnTarget())
            {
                NavAgent.isStopped = true;
                nowReturing = false;
                aiState = AI_TraceAndAttack_State.SearchPlayer;
            }
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

    }
    private void PlayerExitInAttackArea()
    {
    }
}
