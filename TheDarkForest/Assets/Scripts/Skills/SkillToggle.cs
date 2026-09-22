using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SkillToggle : MonoBehaviour
{
    [SerializeField] private int _skillIndex;
    [SerializeField] private SkillsManager _manager;

    private Toggle _toggle;

    public int SkillIndex => _skillIndex;

    private void Awake()
    {
        _toggle = GetComponent<Toggle>();
        _toggle.onValueChanged.AddListener(OnChanged);
    }

    private void OnDestroy()
    {
        _toggle.onValueChanged.RemoveListener(OnChanged);
    }

    private void OnChanged(bool isOn)
    {
        if (isOn && !_manager.CanUse(_skillIndex))
        {
            _toggle.SetIsOnWithoutNotify(false);
            return;
        }

        _manager.OnToggleChanged(_skillIndex, isOn);
    }
}
