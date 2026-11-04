using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CheckpointSettings", menuName = "Scriptable Objects/CheckpointSettings")]
public class CheckpointSettings : ScriptableObject
{
    public CheckpointParams CheckpointParams;
}
[Serializable]
public class CheckpointParams
{
    public Vector3 TransitionPosition;
    public int QuantityHealBottle;
    public int QuantityManaBottle;
    public int QuantityCoin;
    public bool ActiveSkill1;
    public bool ActiveSkill2;
    public bool ActiveSkill3;
}