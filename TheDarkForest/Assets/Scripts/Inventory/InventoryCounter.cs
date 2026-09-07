using System;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryCounter : MonoBehaviour
{
   [SerializeField] private InventoryType  _inventoryType;
   [SerializeField] private GameObject _player;
   [SerializeField] private InventoryDatabase _inventoryDatabase;
   private PlayerMovementComponent _playerMovementComponent;
   

   [SerializeField] private TMP_Text _healCountText;
   [SerializeField] private TMP_Text _manaCountText;
   [SerializeField] private TMP_Text _coinCountText;
   
  // private int _healCount;
  // private int  _manaCount;
  // private int  _coinCount;

   private void Awake()
   {
     _playerMovementComponent = _player.GetComponent<PlayerMovementComponent>();
     
     _inventoryDatabase.IDB.InvDB[0].Count = 0;
     _inventoryDatabase.IDB.InvDB[1].Count = 0;
     _inventoryDatabase.IDB.InvDB[2].Count = 0;

     //_healCount = 0;
     //_manaCount = 0;
     //_coinCount = 0;
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
               _inventoryDatabase.IDB.InvDB[0].Count++;
               _healCountText.text = _inventoryDatabase.IDB.InvDB[0].Count.ToString();
               break;
           case Type.Mana:
               _inventoryDatabase.IDB.InvDB[1].Count++;
               _manaCountText.text = _inventoryDatabase.IDB.InvDB[1].Count.ToString();
               break;
           case Type.Coin:
               _inventoryDatabase.IDB.InvDB[3].Count++;
               _coinCountText.text = _inventoryDatabase.IDB.InvDB[2].Count.ToString();
               break;
       }
   }
}
