using System;
using Player;
using UnityEngine;

public class LootItem : Interactable
{
   [SerializeField] private Type _lootType;
   [SerializeField] private int _amount = 1;

   [SerializeField] private AudioSource _audioSource;
   
   public event Action<Type, int> OnLooted;

   public Type LootType => _lootType;
   public bool Looting = false;

   protected override void OnInteract(GameObject actor)
   {
      OnLooted?.Invoke(_lootType, _amount);
      if (_audioSource != null && _audioSource.clip != null)
      {
         _audioSource.Play();
         
         var collider = GetComponent<Collider>();
         if (collider != null) collider.enabled = false;
         
         foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = false;
         
         if (_lootType != Type.Chest)
         {
            Destroy(gameObject, _audioSource.clip.length);
         }
      }
      else
      {
         if (_lootType != Type.Chest) gameObject.SetActive(false);
      }
   }
}
