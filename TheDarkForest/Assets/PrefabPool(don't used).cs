using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

public class PrefabPool<T> where T : MonoBehaviour
{
    private readonly T _prefab;
    private readonly Transform _parent;
    private readonly Queue<T> _avialable = new Queue<T>();

    private readonly Func<T, T> _factoryOverride;

    public PrefabPool(T prefab, int initializeSize, Transform parent = null, Func<T, T> factoryOverride = null)
    {
        _prefab = prefab;
        _parent = parent;
        _factoryOverride = factoryOverride;

        for(int i = 0; i < initializeSize; i++)
        {
            T obj = CreateNew();
            obj.gameObject.SetActive(false);
            _avialable.Enqueue(obj);
        }
    }

    public T Get()
    {
        T obj = _avialable.Count > 0 ? _avialable.Dequeue() : CreateNew();

        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Return(T obj)
    {
        obj.gameObject.SetActive(false);

        if(_parent)
            obj.transform.SetParent(_parent);

        _avialable.Enqueue(obj);    
    }

    private T CreateNew()
    {
        if(_factoryOverride != null)
            return _factoryOverride(_prefab);

        T instance = Object.Instantiate(_prefab, _parent);
        instance.gameObject.SetActive(false);
        return instance;    
    }
}
