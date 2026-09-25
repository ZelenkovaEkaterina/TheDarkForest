using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class GameObjectPool
{
    private readonly GameObject _prefab;
    private readonly Transform _parent;
    private readonly Queue<GameObject> _available = new Queue<GameObject>();

    public GameObjectPool(GameObject prefab, int initialSize, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;

        InstantiatePool(initialSize, prefab,  parent);
    }

    private void InstantiatePool(int  poolInitialSize, GameObject poolPrefab, Transform poolParent)
    {
        for (int i = 0; i < poolInitialSize; i++)
        {
            GameObject obj = Object.Instantiate(poolPrefab, poolParent);
            obj.SetActive(false);
            _available.Enqueue(obj);
        }
    }

    /*public GameObject Get()
    {
        GameObject obj = _available.Count > 0 ? _available.Dequeue() : Object.Instantiate(_prefab, _parent);
        obj.SetActive(true);
        return obj;
    }*/
    
    public GameObject Get(Vector3? position = null, Quaternion? rotation = null)
    {
        GameObject obj = _available.Count > 0 ? _available.Dequeue() : Object.Instantiate(_prefab, _parent);

        if (position.HasValue)  obj.transform.position = position.Value;
        if (rotation.HasValue)  obj.transform.rotation = rotation.Value;

        obj.SetActive(true);   // теперь агент появится уже в нужной точке
        return obj;
    }
    
    /*public List<GameObject> GetEnemy(int count)
    {
        List<GameObject> objects = new List<GameObject>(count);
        for (int i = 0; i < count; i++)
        {
            objects.Add(Get());
        }
        return objects;
    }*/
    
    public List<GameObject> GetEnemy(int count, Func<Vector3> positionProvider = null)
    {
        List<GameObject> objects = new List<GameObject>(count);
        for (int i = 0; i < count; i++)
        {
            Vector3? pos = positionProvider != null ? positionProvider() : (Vector3?)null;
            objects.Add(Get(pos));
        }
        return objects;
    }

    public void Return(GameObject obj)
    {
        if (obj == null) return;
        
        obj.SetActive(false);
        if (_parent != null)
            obj.transform.SetParent(_parent);
        
        _available.Enqueue(obj);
    }

    public int Count => _available.Count;
}
