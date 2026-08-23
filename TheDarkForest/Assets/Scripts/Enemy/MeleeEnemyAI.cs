using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Enemy
{
    [RequireComponent(typeof(EnemyController))]
   
    public class MeleeEnemyAI : EnemyCombatAI
    {
        private EnemyWeapon _enemyWeapon;
        private int _damageAmount = 5;
        private float _waitAttack = 3;
        protected override void Awake()
        {
            base.Awake();
            attackRange = 2f;
        }

        private void Start()
        {
            _enemyWeapon = GetComponent<EnemyWeapon>();
        }

        protected override void PerformAttack()
        {
            Debug.Log("бьют");
            
            if (Target == null) return;

            IDamageable targetComponent = Target.GetComponent<IDamageable>();
            if (targetComponent != null)
            {
                _enemyWeapon.DealDamage(targetComponent, _damageAmount);
            }
        }

        private IEnumerator WaitAttack()
        {
            yield return new WaitForSeconds(_waitAttack);
            PerformAttack();
        }
    }
}