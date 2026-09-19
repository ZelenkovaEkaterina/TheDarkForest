using System;
using UnityEngine;

namespace Skills
{

    [CreateAssetMenu(menuName = "Scriptable Objects/Skill", fileName = "SkillSettings")]
    public class SkillData : ScriptableObject
    {
        public SkillEntry[] Skills;
    }
    
    [Serializable]
    public class SkillEntry
    {
        public string Name = "Skill";
        public Sprite Icon;
        public int GoldCost = 100;      // стоимость покупки
        public int ManaCost = 10;    // стоимость использования
        public int BonusDamage = 5;     // на сколько прибавить урона
        public float Duration = 8f;     // на сколько секунд
        public GameObject EffectPrefab; // VFX, который проиграется
    }
}