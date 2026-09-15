using System;
using Player;
using UnityEngine;

public class LootItem : Interactable
{
   [SerializeField] private Type _lootType;
   [SerializeField] private int _amount = 1;

   public event Action<Type, int> OnLooted;

   public Type LootType => _lootType;

   protected override void OnInteract(GameObject actor)
   {
      OnLooted?.Invoke(_lootType, _amount);
      gameObject.SetActive(false);   // как у тебя в старом коде
   }
}
