using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AttackStrategy : MonoBehaviour
{
    // Attack Animations
    protected const float CAN_LINKAGE_ATTACK_TIME = 1f;
    protected float elapsedTimeSinceAttack = 0f;
    protected int attackAnimIndex = 0;
    protected bool checkElapsedTime = false;
    protected float animEndTime;
    protected AnimationClip[] attackAnimClips;

    // Data
    protected WeaponData currentData;
    protected Action attackEnd_SendToAttackController;

    // Controller
    protected SoundManager mySoundManager;
    protected Animator playerAnimator;
    protected AnimatorOverrideController animatorOverrideController;

    // private method
    protected abstract void LoadAttackAnims();
    protected abstract IEnumerator IE_WaitLinkageAttackTime();
    // public method
    public abstract void Initialize(WeaponData weaponData, Action attackEndCallback);
    public abstract void ReleaseAttackStrategy();
    public abstract void PlayAttack();
}
