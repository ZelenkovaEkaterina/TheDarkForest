using System.Collections.Generic;
using Enemy;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyCharacter", menuName = "Scriptable Objects/EnemyArray")]
public class EnemyArray : MonoBehaviour //посмотреть что такое Zenject
{
    public List<GameObject> enemyArray = new List<GameObject>();
    
    public int GetCount()
    {
        return enemyArray.Count;
    }
}
