using UnityEngine;

[CreateAssetMenu(fileName = "EnemyPatrolSettings", menuName = "GameSettings/EnemyPatrolSettings")]
public class EnemyPatrolSettings : ScriptableObject
{
    [Header("Патруль")]
    public float patrolRadius = 15f;          // Радиус зоны
    public float waitTimeAtPoint = 2.5f;      // Стоим в точке 2.5 сек перед уходом
    public float patrolSpeed = 1.5f;          // Скорость ходьбы
    
    [Header("Агро")]
    public float aggroRange = 20f;            // Радиус триггера (должен совпадать с коллайдером)
    public float chaseSpeed = 3.5f;           // Скорость бега
    public float attackRadius = 1.8f;         // С какого расстояния бьем
    public float attackCooldown = 1.2f;       // Пауза между ударами
    public float loseTargetDelay = 4f;        // Через сколько секунд забываем игрока после выхода
    
    [Header("Распределение в бою")]
    public float surroundRadius = 2.5f;       // Радиус круга окружения
    public float randomOffset = 0.7f;         // Разброс позиций (чтобы не стояли ровно)
}
