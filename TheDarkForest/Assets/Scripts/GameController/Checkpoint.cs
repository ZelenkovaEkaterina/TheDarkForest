using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private CheckpointManager _checkpointManager;
    [SerializeField] private Transform _spawnPoint;

    private bool _activated;

    private void OnTriggerEnter(Collider other)
    {
        if (_activated || !other.CompareTag("Player")) return;

        _activated = true;
        Vector3 pos = _spawnPoint != null ? _spawnPoint.position : transform.position;
        _checkpointManager.Set(pos);
    }
}
