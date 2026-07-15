using System.Collections.Generic;
using Enemy;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCharacter", menuName = "Scriptable Objects/EnemyArray")]
public class EnemyArray : ScriptableObject
{
    public List<GameObject> enemyArray = new List<GameObject>();
    
    // Методы для управления списком
    public void AddGameObject(GameObject obj)
    {
        if (!enemyArray.Contains(obj))
        {
            enemyArray.Add(obj);
        }
    }
    
    public void RemoveGameObject(GameObject obj)
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
