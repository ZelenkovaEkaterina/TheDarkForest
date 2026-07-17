using Enemy;
using UnityEngine;

public class SpawnGroup : MonoBehaviour
{
    [SerializeField] private EnemyArray objectList; // Ваш ScriptableObject со списком
    [SerializeField] private int spawnCount;
    [SerializeField] private Transform playerTarget;

    [SerializeField] private GameObject pr;
    
    private SphereCollider spawnArea;

    void Start()
    {
        spawnArea =  GetComponent<SphereCollider>();
        spawnCount = objectList.GetCount();
        //SpawnObjects();
    }
    
    public void SpawnObjects(Transform player)
    {
        if (spawnArea == null || objectList.enemyArray.Count == 0)
        {
            Debug.LogWarning("Не все компоненты настроены!");
            return;
        }
        
        Vector3 center = spawnArea.center + transform.position;
        float radius = spawnArea.radius * Mathf.Max(transform.lossyScale.x, 
            transform.lossyScale.y, 
            transform.lossyScale.z);
        
        pr.GetComponent<EnemyAI>().SetTarget(player);
        pr.GetComponent<EnemyAI>().StartChase();
        
        /*for (int i = 0; i < spawnCount; i++)
        {
            // Выбираем объект из списка
            GameObject prefab = objectList.enemyArray[i];
            //EnemyAI enemyAI = prefab.GetComponent<EnemyAI>();
            
            // Создаем случайную позицию внутри сферы
            Vector3 spawnPosition = GetRandomPointInSphere(center, radius);
            
            // Спавним объект
            //Instantiate(prefab, spawnPosition, Quaternion.identity);
            
            prefab.GetComponent<EnemyAI>().SetTarget(player);
            prefab.GetComponent<EnemyAI>().StartChase();
            
            //EnemyAI spawnedEnemy = Instantiate(prefab, spawnPosition, Quaternion.identity);
            
        }*/
        
        
    }
    
    private Vector3 GetRandomPointInSphere(Vector3 center, float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere;
        float randomDistance = Random.Range(0f, radius);
        return center + randomDirection * randomDistance;
    }
}
