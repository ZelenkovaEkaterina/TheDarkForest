using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
   public event Action<Transform> OnChase;
   public event Action OnTargetDead;
   

   public void SetTarget(Transform target)
   {
      OnChase?.Invoke(target);
   }

   public void SetDeadTarget()
   {
      OnTargetDead?.Invoke();
   }
}
