using UnityEngine;

public interface IPoolable
{
    void OnSpawn(Vector3 target);
    void OnDespawn();
}
