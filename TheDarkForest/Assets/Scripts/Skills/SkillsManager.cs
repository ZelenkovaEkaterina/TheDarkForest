using System;
using UnityEngine;
using UnityEngine.UI;

public class SkillsManager : MonoBehaviour
{
    public event Action<Type> OnLootUse;
    [SerializeField] private GameObject _player;
    [SerializeField] private SkillsSettings _settingsSkills;
    [SerializeField] private InventoryDatabase _inventoryBase;

    [SerializeField] private Button[] BuyBow;
    
    [SerializeField] private Button[] UseBow;

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

        foreach (var btn in UseBow)
        {
            btn.interactable = true;
        }
    }

    public void BuySkills(SkillItem skill)
    {
        if (_inventoryBase.IDB.InvDB[2].Count >= _settingsSkills.SkillsSettingsData.SkilItem[skill.SkillItemID].CoinCost)
        {
            BuyBow[skill.SkillItemID].interactable = false;
            _settingsSkills.SkillsSettingsData.SkilItem[skill.SkillItemID].BIsActiveBuy = true;
            _inventoryBase.IDB.InvDB[2].Count -= _settingsSkills.SkillsSettingsData.SkilItem[skill.SkillItemID].CoinCost;
            OnLootUse?.Invoke(Type.Coin);
        }
    }

    public void UseSkill(SkillItem skill)
    {
        if (_settingsSkills.SkillsSettingsData.SkilItem[skill.SkillItemID].BIsActiveBuy)
        {
            if (UseBow[skill.SkillItemID].interactable)
            {
                UseBow[skill.SkillItemID].interactable = false;
                return;
            }
            Debug.Log("111");
            UseBow[skill.SkillItemID].interactable = true;
        }
    }
}

