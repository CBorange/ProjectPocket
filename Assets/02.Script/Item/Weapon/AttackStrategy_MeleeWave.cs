using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackStrategy_MeleeWave : AttackStrategy
{
    // Controller
    private InstantAttackController instantAttack;
    private ProjectileAttackController projectileAttack;
    private TrailRenderer bladeTrailRenderer;

    // Data
    private const float INSTANT_EXECUTE_REFERENCE_DELAY = 0.4f;
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
        bladeTrailRenderer = transform.GetChild(0).GetComponent<TrailRenderer>();
        bladeTrailRenderer.gameObject.SetActive(false);

        playerAnimator = GameObject.Find("Player").transform.GetComponent<Animator>();
        animatorOverrideController = new AnimatorOverrideController(playerAnimator.runtimeAnimatorController);
        playerAnimator.runtimeAnimatorController = animatorOverrideController;
        mySoundManager = transform.GetChild(1).GetComponent<SoundManager>();

        CreateInstantAttackBox();
        CreateWaveProjectilePool();
    }
    private void CreateInstantAttackBox()
    {
        // Create InstantAttack Box
        GameObject instantAttackPrefab = AssetBundleCacher.Instance.LoadAndGetAsset("weapon", "InstantAttackColBox") as GameObject;
        instantAttack = Instantiate(instantAttackPrefab).GetComponent<InstantAttackController>();
        instantAttack.Initialize(PlayerActManager.Instance.transform, currentData);
        instantAttack.gameObject.SetActive(false);
    }
    private void CreateWaveProjectilePool()
    {
        GameObject wavePool = new GameObject("WavePool");
        projectileAttack = wavePool.AddComponent<ProjectileAttackController>();
        projectileAttack.Initialize(PlayerActManager.Instance.transform, currentData, bladeTrailRenderer.startColor);
    }
    public override void PlayAttack()
    {
        animatorOverrideController["MeleeAttack_Downward"] = attackAnimClips[attackAnimIndex];
        float attackSpeed = PlayerStat.Instance.GetStat("AttackSpeed");
        playerAnimator.speed = attackSpeed;
        animEndTime = (attackAnimClips[attackAnimIndex].length / 2) / attackSpeed;
        playerAnimator.SetTrigger("Attack");
        bladeTrailRenderer.gameObject.SetActive(true);

        executeDelay = INSTANT_EXECUTE_REFERENCE_DELAY / attackSpeed;
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
        projectileAttack.Execute();
    }
    private void SendEndAttack()
    {
        checkElapsedTime = true;
        StartCoroutine(IE_WaitLinkageAttackTime());

        playerAnimator.speed = 1;
        bladeTrailRenderer.gameObject.SetActive(false);
        attackEnd_SendToAttackController();
    }
    public override void ReleaseAttackStrategy()
    {
        Destroy(instantAttack.gameObject);
    }
}
