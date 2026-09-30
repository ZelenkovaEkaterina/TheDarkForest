using System;
using UnityEngine;

public class GolemAnimationController : MonoBehaviour
{
   private GolemAI _golemAI;
   
   private Animator _animator;
   
   private void Awake()
   {
      _golemAI = GetComponent<GolemAI>();
      _animator = GetComponent<Animator>();
      //_animator.speed = 0.5f;
   }

   private void OnEnable()
   {
      _golemAI.OnStateChange += GolemStateHander;
   }

   private void OnDisable()
   {
      _golemAI.OnStateChange -= GolemStateHander;
   }

   private void GolemStateHander(GolemState state)
   {
      switch (state)
      {
         case GolemState.Attack:
            _animator.SetBool("Attack", true);
            break;
         case GolemState.Idle:
            _animator.SetBool("Attack", false);
            break;
      }
   }
}
