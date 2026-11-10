using System;
using Player;
using UnityEngine;
using UnityEngine.AI;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private SkillsSettings _skills;
    [SerializeField] private InventoryDatabase _inventory;
    [SerializeField] private CheckpointSettings _checkpointSettings;
    [SerializeField] private Transform _chests;
    private NavMeshAgent _navMeshAgent;
    
    
    [SerializeField] private MenuController _menuController;
    [SerializeField] private InventoryCounter _inventoryController;
    [SerializeField] private SkillsManager _skillsManager;
    [SerializeField] private EnemyHealthComponent _golemHealth;
    
    private void Awake()
    {
        _navMeshAgent = _player.GetComponent<NavMeshAgent>();
    }

    public void Respawn()
    {
        _navMeshAgent.Warp(_checkpointSettings.CheckpointParams.TransitionPosition);
        _player.GetComponent<PlayerHealthComponent>().ResetHealth();
        _player.GetComponent<PlayerMovementComponent>().SetState(PlayerState.Idle);
        _player.GetComponent<Animator>().SetTrigger("Respawn");

        foreach (var item in _inventory.IDB.InvDB)
        {
            switch (item.Type)
            {
                case Type.Heal:
                    item.Count = _checkpointSettings.CheckpointParams.QuantityHealBottle;
                    break;
                case Type.Mana:
                    item.Count = _checkpointSettings.CheckpointParams.QuantityManaBottle;
                    break;
                case Type.Coin:
                    item.Count = _checkpointSettings.CheckpointParams.QuantityCoin;
                    break;
            }
        }

        foreach (var item in _skills.SkillsSettingsData.SkilItem)
        {
            item.BIsActiveBuy = _checkpointSettings.CheckpointParams.SkillsBuy[item.BowId];
        }
        
        foreach (Transform chest in _chests)
        {
            int id = chest.GetComponent<LootItem>().Id;
            
            chest.GetComponent<LootItem>().Looting = _checkpointSettings.CheckpointParams.ChestUse[id];
        }
        
        _menuController.RestartGame();
        _inventoryController.UpdateInventory();
        _skillsManager.OffSkills();
        _golemHealth.ResetHealth();
    }
}
