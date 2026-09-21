using System;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "SettingsSkills", menuName = "Scriptable Objects/SettingsSkills")]
public class SkillsSettings : ScriptableObject
{
    public SkillsSettingsData SkillsSettingsData;
}

[Serializable]
public class SkillsSettingsData
{
    public Skills[] SkilItem = new Skills[3];
}

[Serializable]
public class Skills
{
    public int BowId;
    public int ShotBuff;
    public int ManaCost;
    public int CoinCost;
    public bool BIsActiveBuy;
}