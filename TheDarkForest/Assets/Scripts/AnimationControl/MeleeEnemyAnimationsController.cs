using System;
using System.Collections;
using UnityEngine;

public class MeleeEnemyAnimationsController : MonoBehaviour
{
    private EnemyPatrol _enemyPatrol;
    private EnemyCombatAI _enemyCombat;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _enemyPatrol = GetComponent<EnemyPatrol>();
        _enemyCombat = GetComponent<EnemyCombatAI>();
        
    }

    private void OnEnable()
    {
       
    }

   

    private void FixedUpdate()
    {

        


    }
    
}
