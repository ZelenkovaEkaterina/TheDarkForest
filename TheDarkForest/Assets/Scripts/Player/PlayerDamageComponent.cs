using UnityEngine;

public class PlayerDamageComponent : MonoBehaviour
{
    public void AttackEnemy(GameObject enemy)
    {
        // Находим компонент врага
        EnemyDamageableComponent enemyHealth = enemy.GetComponent<EnemyDamageableComponent>();
        
        if (enemyHealth != null)
        {
            // Наносим 10 урона от игрока
            enemyHealth.TakeDamage(10, this.gameObject);
            
            // Проверяем, умер ли враг
            if (enemyHealth.IsDead())
            {
                Destroy(enemy);  // удаляем врага
                // или проигрываем анимацию смерти
            }
        }
    }
}
