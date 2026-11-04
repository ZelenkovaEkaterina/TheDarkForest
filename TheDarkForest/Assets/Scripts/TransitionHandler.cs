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
    private AudioSource _audioSource;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _checkpointSettings.CheckpointParams.TransitionPosition = _startTransform.position;
        _checkpointSettings.CheckpointParams.QuantityHealBottle = 0;
        _checkpointSettings.CheckpointParams.QuantityManaBottle = 0;
        _checkpointSettings.CheckpointParams.QuantityCoin = 0;
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
            
            /*_checkpointSettings.CheckpointParams.QuantityHealth =
                other.GetComponent<PlayerHealthComponent>().CurrentHealth;
            _checkpointSettings.CheckpointParams.QuantityMana = 
                other.GetComponent<PlayerManaComponent>().CurrentMana;*/

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
                switch (item.BowId)
                {
                    case 0:
                        _checkpointSettings.CheckpointParams.ActiveSkill1 = item.BIsActiveBuy;
                        break;
                    case 1:
                        _checkpointSettings.CheckpointParams.ActiveSkill2 = item.BIsActiveBuy;
                        break;
                    case 2:
                        _checkpointSettings.CheckpointParams.ActiveSkill3 = item.BIsActiveBuy;
                        break;
                }
            }
            
            /*_checkpointSettings.CheckpointParams.QuantityHealBottle = _inventoryDatabase.IDB.InvDB[0].Count;
            _checkpointSettings.CheckpointParams.QuantityManaBottle = _inventoryDatabase.IDB.InvDB[1].Count;
            _checkpointSettings.CheckpointParams.QuantityCoin = _inventoryDatabase.IDB.InvDB[2].Count;*/
        }
    }
}
