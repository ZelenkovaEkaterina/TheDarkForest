using UnityEngine;
using System.Collections.Generic;
using Enemy;

public class MobGroup : MonoBehaviour
{
    [SerializeField] private List<EnemyAI> mobs = new List<EnemyAI>();
    //[SerializeField] private EnemyArray mobs;
    
    private bool _isActive = false;

    /// <summary>
    /// Инициализация группы. Вызывается один раз при создании/спавне.
    /// </summary>

    public void ActivateGroup(Transform playerTransform)
    {
        // Проверка на наличие игрока обязательна, так как мы больше не ищем его сами
        if (_isActive || playerTransform == null)
        {
            Debug.LogWarning("[MobGroup] Активация невозможна: группа активна или нет цели.");
            return;
        }

        _isActive = true;
        
        foreach (var mob in mobs)
        {
            mob.SetTarget(playerTransform);
            mob.StartChase();
        }
    }

    public bool IsActive => _isActive;

    // Хелпер для динамического добавления мобов
   /* public void RegisterMob(Mob mob)
    {
        mobs.Add(mob);
    }*/
}
