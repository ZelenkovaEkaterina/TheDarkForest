using System;
using System.Collections.Generic;
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
    public List<bool> SkillsBuy;
}