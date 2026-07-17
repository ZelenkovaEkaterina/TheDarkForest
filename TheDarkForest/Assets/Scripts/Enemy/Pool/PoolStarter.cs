using UnityEngine;

public class PoolStarter : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    private GameObjectPool _bulletPool;

    void Awake()
    {
        _bulletPool = new GameObjectPool(_bulletPrefab, 20, transform);
    }
}
