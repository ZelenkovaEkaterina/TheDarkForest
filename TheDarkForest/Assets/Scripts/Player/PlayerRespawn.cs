using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private CheckpointManager _checkpointManager;
    [SerializeField] private CharacterController _controller;
    [SerializeField] private HealthSystem _health;
    [SerializeField] private Vector3 _fallbackSpawn;

    public void Respawn()
    {
        Vector3 pos = _checkpointManager.HasCheckpoint ? _checkpointManager.Position : _fallbackSpawn;

        if (_controller != null) _controller.enabled = false;
        transform.position = pos;
        if (_controller != null) _controller.enabled = true;

        _health.ResetHealth();
    }
}
