using System;
using UnityEngine;

public class PlayerDamageComponent : MonoBehaviour
{
    public event Action<EnemyState> OnDead;
    public void AttackEnemy(GameObject enemy)
    {
        EnemyDamageableComponent enemyHealth = enemy.GetComponent<EnemyDamageableComponent>();
        
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(10, this.gameObject);
        }
    }
}
