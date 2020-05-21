using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Http.Headers;

public class WeaponBehaviour_OneHand : MonoBehaviour, IWeaponBehaviour
{
    // Attack Animations
    private const float CAN_LINKAGE_ATTACK_TIME = 1f;
    private float elapsedTimeSinceAttack = 0f;
    private int attackAnimIndex = 0;
    private bool checkElapsedTime = false;
    private AnimationClip[] attackAnimClips;

    // Controller
    private Animator playerAnimator;
    private AnimatorOverrideController animatorOverrideController;
    private PlayerAttack_Instant playerAttackBox;
    private TrailRenderer trailRenderer;

    // Data
    private Transform playerTransform;
    private WeaponData weaponData;
    private Action attackEnd_SendToAttackController;
    private float animEndTime;

    public void CreateBehaviour(WeaponData weaponData, Action attackEndCallback)
    {
        this.weaponData = weaponData;
        this.attackEnd_SendToAttackController = attackEndCallback;
        trailRenderer = transform.GetChild(0).GetComponent<TrailRenderer>();
        trailRenderer.gameObject.SetActive(false);

        playerTransform = GameObject.Find("Player").transform;
        playerAnimator = playerTransform.GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(playerAnimator.runtimeAnimatorController);
        playerAnimator.runtimeAnimatorController = animatorOverrideController;

        // Create ColiderBox
        GameObject newColiderBox = new GameObject("PlayerWeaponColiderBox");
        newColiderBox.transform.parent = PlayerActManager.Instance.transform;
        playerAttackBox = newColiderBox.AddComponent<PlayerAttack_Instant>();
        playerAttackBox.tag = "Player_WeaponColBox";
        playerAttackBox.Initialize(PlayerActManager.Instance.transform, new Vector3(0, 1, (weaponData.Range / 2)), weaponData);
        playerAttackBox.gameObject.SetActive(false);

        // Load Attack AnimationClips
        attackAnimClips = new AnimationClip[4];
        attackAnimClips[0] = Resources.Load<AnimationClip>("Animations/MeleeAttack_Downward");
        attackAnimClips[1] = Resources.Load<AnimationClip>("Animations/MeleeAttack_Horizontal");
        attackAnimClips[2] = Resources.Load<AnimationClip>("Animations/MeleeAttack_BackHand");
        attackAnimClips[3] = Resources.Load<AnimationClip>("Animations/MeleeAttack_360High");
    }
    public void PlayAttack()
    {
        ChangeAttackAnimByIndex();
        float attackSpeed = PlayerStat.Instance.GetStat("AttackSpeed");
        playerAnimator.speed = attackSpeed;
        animEndTime = (attackAnimClips[attackAnimIndex].length / 2) / attackSpeed;
        playerAnimator.SetTrigger("Attack");
        trailRenderer.gameObject.SetActive(true);

        Invoke("ExecuteAttack", weaponData.TriggerDelay / attackSpeed);
        Invoke("SendEndAttack", animEndTime);

        if (elapsedTimeSinceAttack < CAN_LINKAGE_ATTACK_TIME)
        {
            checkElapsedTime = false;
            attackAnimIndex += 1;
            if (attackAnimIndex > 3)
                attackAnimIndex = 0;
        }
    }
    private IEnumerator IE_WaitLinkageAttackTime()
    {
        elapsedTimeSinceAttack = 0f;
        while (checkElapsedTime)
        {
            elapsedTimeSinceAttack += Time.deltaTime;
            if (elapsedTimeSinceAttack > CAN_LINKAGE_ATTACK_TIME)
            {
                attackAnimIndex = 0;
                checkElapsedTime = false;
                yield break;
            }
            yield return null;
        }
    }
    private void ChangeAttackAnimByIndex()
    {
        animatorOverrideController["Attack1"] = attackAnimClips[attackAnimIndex];
    }
    private void ExecuteAttack()
    {
        playerAttackBox.Execute();
    }
    private void SendEndAttack()
    {
        checkElapsedTime = true;
        StartCoroutine(IE_WaitLinkageAttackTime());

        trailRenderer.gameObject.SetActive(false);
        attackEnd_SendToAttackController();
    }
    public void ReleaseBehaviour()
    {
        Destroy(playerAttackBox.gameObject);
    }
}
