using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillsManager : MonoBehaviour
{
    public event Action<Type> OnLootUse;
    public event Action<int, bool> OnUseSkill;
    
    [SerializeField] private GameObject _player;
    [SerializeField] private SkillsSettings _settingsSkills;
    [SerializeField] private InventoryDatabase _inventoryBase;

    [SerializeField] private Button[] BuyBow;
    
    [SerializeField] private Toggle[] UseBow;

    private void Awake()
    {
        foreach (var obj in _settingsSkills.SkillsSettingsData.SkilItem)
        {
            obj.BIsActiveBuy = false;
        }
        
        foreach (var btn in BuyBow)
        {
            btn.interactable = true;
        }

        foreach (var t in UseBow)
        {
            t.interactable = false;
            t.isOn = false;
        }
    }
    
    public bool CanUse(int skillIndex)
    {
        return _settingsSkills.SkillsSettingsData.SkilItem[skillIndex].BIsActiveBuy;
    }
    
    public void OnToggleChanged(int skillIndex, bool isOn)
    {
        OnUseSkill?.Invoke(skillIndex,isOn);
    }

    public void BuySkills(SkillItem skill)
    {
        int id = skill.SkillItemID;
        var data = _settingsSkills.SkillsSettingsData.SkilItem[id];

        if (_inventoryBase.IDB.InvDB[2].Count >= data.CoinCost)
        {
            BuyBow[id].interactable = false;
            data.BIsActiveBuy = true;
            _inventoryBase.IDB.InvDB[2].Count -= data.CoinCost;
            
            UseBow[id].interactable = true;

            OnLootUse?.Invoke(Type.Coin);
        }
    }

    public void OffSkills()
    {
        foreach (var t in UseBow) t.SetIsOnWithoutNotify(false);
        OnUseSkill?.Invoke(-1, false);
    }
}

