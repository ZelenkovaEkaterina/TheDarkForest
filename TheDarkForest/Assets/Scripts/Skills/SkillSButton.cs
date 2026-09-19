using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Skills
{
    public enum SkillButtonMode { Buy, Use }
    public class SkillSButton : MonoBehaviour
    {
        [SerializeField] private SkillManager _manager;
        [SerializeField] private SkillButtonMode _mode;
        [SerializeField] private int _index;

        [SerializeField] private Button _button;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _costText;    // цена / мана
        [SerializeField] private Color _dim = new Color(0.4f, 0.4f, 0.4f, 0.7f);

        private void Start()
        {
            if (_manager == null) _manager = FindObjectOfType<SkillManager>();

            var s = _manager.Data.Skills[_index];
            _icon.sprite = s.Icon;
            _costText.text = _mode == SkillButtonMode.Buy
                ? s.GoldCost.ToString()
                : s.ManaCost.ToString();

            _button.onClick.AddListener(OnClick);
            _manager.OnChanged += Refresh;
            Refresh();
        }

        private void OnDestroy()
        {
            if (_manager != null) _manager.OnChanged -= Refresh;
        }

        private void OnClick()
        {
            if (_mode == SkillButtonMode.Buy) _manager.TryLearn(_index);
            else _manager.TryUse(_index);
        }

        private void Refresh()
        {
            bool learned = _manager.IsLearned(_index);

            // тусклая иконка, если не куплено
            _icon.color = learned ? Color.white : _dim;

            if (_mode == SkillButtonMode.Buy)
            {
                _button.interactable = !learned;    // купить можно один раз
            }
            else
            {
                bool active = _manager.IsActive(_index);
                _button.interactable = learned && !active;   // юзать пока не активно
            }
        }
    }
}