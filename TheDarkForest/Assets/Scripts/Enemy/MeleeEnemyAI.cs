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
        protected override void Awake()
        {
            base.Awake();
            attackRange = 2f;
        }
        protected override void PerformAttack()
        {
            Debug.Log("бьют");
            // Здесь вызывается настоящая атака (например, через EnemyController)
        }
    }
}