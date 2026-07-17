using System.Collections.Generic;
using UnityEngine;

public class GameObjectPool
{
    private readonly GameObject _prefab;
    private readonly Transform _parent;
    private readonly Queue<GameObject> _available = new Queue<GameObject>();

    public GameObjectPool(GameObject prefab, int initialSize, Transform parent = null)
    {
        _prefab = prefab;
        _parent = parent;

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Object.Instantiate(prefab, parent);
            obj.SetActive(false);
            _available.Enqueue(obj);
        }
    }

    public GameObject Get()
    {
        GameObject obj = _available.Count > 0 ? _available.Dequeue() : Object.Instantiate(_prefab, _parent);
        obj.SetActive(true);
        return obj;
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
