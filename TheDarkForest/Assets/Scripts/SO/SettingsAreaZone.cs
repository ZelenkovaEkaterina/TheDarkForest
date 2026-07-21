using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "SettingsAreaZone", menuName = "Scriptable Objects/SettingsAreaZone")]
public class SettingsAreaZone : ScriptableObject
{
    public EnemiesZone Settings;
}

[Serializable]
public class EnemiesZone
{
    public ZoneSettings[] Items = new ZoneSettings[3];
}

[Serializable]
public class ZoneSettings
{
    public int Id;
    public int NearEnemiesCount;
    public int RangeEnemiesCount;
}
