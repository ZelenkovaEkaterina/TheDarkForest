using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    
    [SerializeField] public GameObject enemyPrefab;
    [SerializeField] public int poolInitialSize = 20;
    
    public GameObjectPool EnemyPool { get; private set; }
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
        EnemyPool = new GameObjectPool(enemyPrefab, poolInitialSize, transform);
        Debug.Log($"Пул создан с {poolInitialSize} объектами");
    }
}
