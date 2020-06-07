using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackStrategy_MeleeInstant : AttackStrategy
{
    // Controller
    private InstantAttackController instantAttack;
    private TrailRenderer trailRenderer;

    // Data
    private const float EXECUTE_REFERENCE_DELAY = 0.4f;
    private const float INSTANT_REFERENCE_LIFETIME = 0.3f;
    private float executeDelay;
    private float attackLifeTime;

    protected override void LoadAttackAnims()
    {
        attackAnimClips = new AnimationClip[4];

        AssetBundle bundle = AssetBundleCacher.Instance.LoadAndGetBundle("animation");
        attackAnimClips[0] = bundle.LoadAsset("MeleeAttack_Downward") as AnimationClip;
        attackAnimClips[1] = bundle.LoadAsset("MeleeAttack_Horizontal") as AnimationClip;
        attackAnimClips[2] = bundle.LoadAsset("MeleeAttack_BackHand") as AnimationClip;
        attackAnimClips[3] = bundle.LoadAsset("MeleeAttack_360High") as AnimationClip;
    }
    protected override IEnumerator IE_WaitLinkageAttackTime()
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
    public override void Initialize(WeaponData weaponData, Action attackEndCallback)
    {
        // Base Init
        LoadAttackAnims();

        currentData = weaponData;
        attackEnd_SendToAttackController = attackEndCallback;
        trailRenderer = transform.GetChild(0).GetComponent<TrailRenderer>();
        trailRenderer.gameObject.SetActive(false);

        playerAnimator = GameObject.Find("Player").transform.GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(playerAnimator.runtimeAnimatorController);
        playerAnimator.runtimeAnimatorController = animatorOverrideController;
        mySoundManager = transform.GetChild(1).GetComponent<SoundManager>();

        // Create InstantAttack Box
        GameObject newColiderBox = new GameObject("PlayerInstantAttack");
        newColiderBox.transform.parent = PlayerActManager.Instance.transform;
        instantAttack = newColiderBox.AddComponent<InstantAttackController>();
        instantAttack.Initialize(PlayerActManager.Instance.transform, new Vector3(0, 1, (weaponData.Range / 2)), weaponData);
        instantAttack.gameObject.SetActive(false);
    }
    public override void PlayAttack()
    {
        animatorOverrideController["MeleeAttack_Downward"] = attackAnimClips[attackAnimIndex];
        float attackSpeed = PlayerStat.Instance.GetStat("AttackSpeed");
        playerAnimator.speed = attackSpeed;
        animEndTime = (attackAnimClips[attackAnimIndex].length / 2) / attackSpeed;
        playerAnimator.SetTrigger("Attack");
        trailRenderer.gameObject.SetActive(true);

        executeDelay = EXECUTE_REFERENCE_DELAY / attackSpeed;
        attackLifeTime = INSTANT_REFERENCE_LIFETIME / attackSpeed;

        Invoke("ExecuteAttack", executeDelay / attackSpeed);
        Invoke("SendEndAttack", animEndTime);

        if (elapsedTimeSinceAttack < CAN_LINKAGE_ATTACK_TIME)
        {
            checkElapsedTime = false;
            attackAnimIndex += 1;
            if (attackAnimIndex > attackAnimClips.Length - 1)
                attackAnimIndex = 0;
        }
    }
    private void ExecuteAttack()
    {
        mySoundManager.PlayOneShot("WeaponSwing");
        instantAttack.Execute(attackLifeTime);
    }
    private void SendEndAttack()
    {
        checkElapsedTime = true;
        StartCoroutine(IE_WaitLinkageAttackTime());

        playerAnimator.speed = 1;
        trailRenderer.gameObject.SetActive(false);
        attackEnd_SendToAttackController();
    }
    public override void ReleaseAttackStrategy()
    {
        Destroy(instantAttack.gameObject);
    }
}
