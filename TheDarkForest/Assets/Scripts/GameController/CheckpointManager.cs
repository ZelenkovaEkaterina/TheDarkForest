using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Checkpoint", menuName = "Scriptable Objects/Checkpoint")]
public class CheckpointManager : ScriptableObject
{
    public event Action<Vector3> OnCheckpointSet;

    public Vector3 Position { get; private set; }
    public bool HasCheckpoint { get; private set; }

    public void Set(Vector3 position)
    {
        Position = position;
        HasCheckpoint = true;
        OnCheckpointSet?.Invoke(position);
    }
    
    public void Reset()
    {
        Position = default;
        HasCheckpoint = false;
    }
}
