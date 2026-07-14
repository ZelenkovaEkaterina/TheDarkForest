using System.Collections.Generic;
using Enemy;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCharacter", menuName = "Scriptable Objects/EnemyArray")]
public class EnemyArray : ScriptableObject
{
    public List<EnemyAI> enemyArray = new List<EnemyAI>();
    
    // Методы для управления списком
    public void AddGameObject(EnemyAI obj)
    {
        if (!enemyArray.Contains(obj))
        {
            enemyArray.Add(obj);
        }
    }
    
    public void RemoveGameObject(EnemyAI obj)
    {
        enemyArray.Remove(obj);
    }
    
    public void ClearList()
    {
        enemyArray.Clear();
    }
    
    public int GetCount()
    {
        return enemyArray.Count;
    }
}
