using System;
using UnityEngine;
using UnityEngine.AI;

public class TransitionHandler : MonoBehaviour
{
    [SerializeField] private Transform _startTransform;
    [SerializeField] private CheckpointSettings _checkpointSettings;
    [SerializeField] private SkillsSettings _skills;
    [SerializeField] private InventoryDatabase _inventoryDatabase;
    [SerializeField] private Transform _transition;
    [SerializeField] private Transform _chests;
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _checkpointSettings.CheckpointParams.TransitionPosition = _startTransform.position;
        _checkpointSettings.CheckpointParams.QuantityHealBottle = 0;
        _checkpointSettings.CheckpointParams.QuantityManaBottle = 0;
        _checkpointSettings.CheckpointParams.QuantityCoin = 0;
        
        foreach (var item in _skills.SkillsSettingsData.SkilItem)
        {
            _checkpointSettings.CheckpointParams.SkillsBuy[item.BowId] = false;
        }
        
        foreach (Transform chest in _chests)
        {
            int id = chest.GetComponent<LootItem>().Id;
            _checkpointSettings.CheckpointParams.ChestUse[id] = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<NavMeshAgent>().ResetPath();
            other.GetComponent<NavMeshAgent>().Warp(_transition.position);
            _audioSource.Play();

            if (_checkpointSettings == null || _inventoryDatabase == null) return;
            _checkpointSettings.CheckpointParams.TransitionPosition = _transition.position;

            foreach (var item in _inventoryDatabase.IDB.InvDB)
            {
                switch (item.Type)
                {
                    case Type.Heal:
                        _checkpointSettings.CheckpointParams.QuantityHealBottle = item.Count;
                        break;
                    case Type.Mana:
                        _checkpointSettings.CheckpointParams.QuantityManaBottle = item.Count;
                        break;
                    case Type.Coin:
                        _checkpointSettings.CheckpointParams.QuantityCoin = item.Count;
                        break;
                }
            }

            foreach (var item in _skills.SkillsSettingsData.SkilItem)
            {
                _checkpointSettings.CheckpointParams.SkillsBuy[item.BowId] = item.BIsActiveBuy;
            }
            
            foreach (Transform chest in _chests)
            {
                int id = chest.GetComponent<LootItem>().Id;
                bool l = chest.GetComponent<LootItem>().Looting;
                _checkpointSettings.CheckpointParams.ChestUse[id] = l;
            }
        }
    }
}
