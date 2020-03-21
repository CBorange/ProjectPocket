using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponBehaviourCollection
{
    public class WeaponBehaviour_OneHand : IWeaponBehaviour
    {
        private Animator playerAnimator;
        private float attackPoint;
        private float attackSpeed;
        private float range;

        public WeaponBehaviour_OneHand(Animator playerAnimator, float attackPoint, float attackSpeed, float range)
        {
            this.playerAnimator = playerAnimator;
            this.attackPoint = attackPoint;
            this.attackSpeed = attackSpeed;
            this.range = range;
        }
        public void PlayAttack()
        {

        }
    }
}
