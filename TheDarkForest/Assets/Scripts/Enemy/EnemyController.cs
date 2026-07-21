using System;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
   public event Action<Transform> OnChase;

   public void SetTarget(Transform target)
   {
      OnChase?.Invoke(target);
   }
}
