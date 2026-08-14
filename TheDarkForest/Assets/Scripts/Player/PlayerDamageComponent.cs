using System;
using UnityEngine;

public class PlayerDamageComponent : MonoBehaviour
{
    public event Action<EnemyState> OnDead;

    private int _damage = 10;
    public void AttackEnemy(GameObject enemy)
    {
        EnemyDamageableComponent enemyHealth = enemy.GetComponent<EnemyDamageableComponent>();
        
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(_damage, this.gameObject);
        }
    }
}
