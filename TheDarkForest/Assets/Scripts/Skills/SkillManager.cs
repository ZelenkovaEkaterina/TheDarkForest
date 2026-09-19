using System;
using UnityEngine;

namespace Skills
{
    public class SkillManager : MonoBehaviour
    {
        public event Action OnChanged;

        [SerializeField] private SkillData _data;
        [SerializeField] private PlayerManaComponent _mana;
        [SerializeField] private GameObject _effectSpawnPoint;   // куда цеплять VFX

        private bool[] _learned;
        private float[] _activeUntil;      // время, до которого скилл "горит"
        private GameObject[] _activeFx;

        public SkillData Data => _data;
        public int Count => _data.Skills.Length;

        private void Awake()
        {
            _learned = new bool[_data.Skills.Length];
            _activeUntil = new float[_data.Skills.Length];
            _activeFx = new GameObject[_data.Skills.Length];
        }

        // ---------- Покупка ----------

        public bool IsLearned(int i) => _learned[i];

        public bool TryLearn(int i)
        {
            if (_learned[i]) return false;
            // если есть инвентарь — проверь золото здесь
            _learned[i] = true;
            OnChanged?.Invoke();
            return true;
        }

        // ---------- Использование ----------

        public bool IsActive(int i) => Time.time < _activeUntil[i];
        public float ActiveRemaining(int i) => Mathf.Max(0f, _activeUntil[i] - Time.time);

        public bool TryUse(int i)
        {
            if (!_learned[i]) return false;

            var s = _data.Skills[i];

            // мана
            if (_mana != null && !_mana.TrySpend(s.ManaCost)) return false;

            // включаем на Duration секунд
            _activeUntil[i] = Time.time + s.Duration;

            // VFX
            if (s.EffectPrefab != null && _effectSpawnPoint != null)
            {
                if (_activeFx[i] != null) Destroy(_activeFx[i]);
                _activeFx[i] = Instantiate(s.EffectPrefab, _effectSpawnPoint.transform);
            }

            OnChanged?.Invoke();
            return true;
        }

        /// <summary>Суммарный бонусный урон от всех активных скиллов.</summary>
        public int GetBonusDamage()
        {
            int sum = 0;
            for (int i = 0; i < _data.Skills.Length; i++)
                if (IsActive(i)) sum += _data.Skills[i].BonusDamage;
            return sum;
        }

        private void Update()
        {
            // гасим истёкшие скиллы
            for (int i = 0; i < _data.Skills.Length; i++)
            {
                if (_activeUntil[i] > 0f && Time.time >= _activeUntil[i])
                {
                    _activeUntil[i] = 0f;
                    if (_activeFx[i] != null) { Destroy(_activeFx[i]); _activeFx[i] = null; }
                    OnChanged?.Invoke();
                }
            }
        }
    }
}