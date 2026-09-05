using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCounter : MonoBehaviour
{
   [SerializeField] private InventoryType  _inventoryType;
   [SerializeField] private GameObject _player;
   private PlayerMovementComponent _playerMovementComponent;

   [SerializeField] private TMP_Text _healCountText;
   [SerializeField] private TMP_Text _manaCountText;
   [SerializeField] private TMP_Text _coinCountText;
   
   private int _healCount;
   private int  _manaCount;
   private int  _coinCount;

   private void Awake()
   {
     _playerMovementComponent = _player.GetComponent<PlayerMovementComponent>();

     _healCount = 0;
     _manaCount = 0;
     _coinCount = 0;
   }

   private void OnEnable()
   {
       _playerMovementComponent.OnLoot += LootCount;
   }

   private void OnDisable()
   {
       _playerMovementComponent.OnLoot -= LootCount;
   }

   private void LootCount(Type type)
   {
       switch (type)
       {
           case Type.Heal:
               _healCount++;
               _healCountText.text = _healCount.ToString();
               break;
           case Type.Mana:
               _manaCount++;
               _manaCountText.text = _manaCount.ToString();
               break;
           case Type.Coin:
               _coinCount++;
               _coinCountText.text = _coinCount.ToString();
               break;
       }
   }
}
