using System;
using UnityEngine;
using UnityEngine.AI;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private InventoryDatabase _inventory;
    [SerializeField] private CheckpointSettings _checkpointSettings;
    private NavMeshAgent _navMeshAgent;
    
    
    [SerializeField] private MenuController _menuController;

    private void Awake()
    {
        _navMeshAgent = _player.GetComponent<NavMeshAgent>();
    }

    public void Respawn()
    {
        _navMeshAgent.Warp(_checkpointSettings.CheckpointParams.TransitionPosition);
        _player.GetComponent<PlayerHealthComponent>().ResetHealth();

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
        
        _menuController.RestartGame();
    }
}
