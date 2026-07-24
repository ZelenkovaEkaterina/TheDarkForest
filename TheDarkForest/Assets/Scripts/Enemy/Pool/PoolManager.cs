using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] public GameObject enemyPrefab;
    [SerializeField] public int poolInitialSize = 20;

    private GameObjectPool _enemyPool;
    public GameObjectPool EnemyPool => _enemyPool;
    
    private void Awake()
    {
        _enemyPool = new GameObjectPool(enemyPrefab, poolInitialSize, transform);
        Debug.Log($"Пул создан с {poolInitialSize} объектами");
    }
}
