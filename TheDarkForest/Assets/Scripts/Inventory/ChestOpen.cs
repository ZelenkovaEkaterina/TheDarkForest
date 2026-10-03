using System;
using System.Collections;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

public class ChestOpen : MonoBehaviour
{
   private Animator _animator;
   private LootItem _item;
   
   [SerializeField] private LootItem[] _lootPrefabs;
   [SerializeField] private float _flightTime = 0.5f;
   [SerializeField] private float _flightHeight = 1.5f;
   [SerializeField] private float _spread = 1.5f;

   private void Awake()
   {
      _animator = GetComponent<Animator>();
      _item = GetComponent<LootItem>();
   }

   private void OnEnable()
   {
      _item.OnLooted += OpenChest;
   }

   private void OnDisable()
   {
      _item.OnLooted -= OpenChest;
   }

   private void OpenChest(Type obj, int amount)
   {
      Debug.Log("Opening Chest");
      if (obj == Type.Chest && !_item.Looting)
      {
         _animator.SetTrigger("Open");
         _item.Looting = true;
      }
   }
   
   public void SpawnLoot()
   {
      foreach (var prefab in _lootPrefabs)
      {
         Vector3 start = transform.position + Vector3.up;
         Vector2 offset = Random.insideUnitCircle * _spread;
         Vector3 end = start + new Vector3(offset.x, 0f, offset.y);

         var item = Instantiate(prefab, start, Quaternion.identity);
         StartCoroutine(Fly(item.transform, start, end));
      }
   }

   private IEnumerator Fly(Transform t, Vector3 start, Vector3 end)
   {
      for (float p = 0f; p < 1f; p += Time.deltaTime / _flightTime)
      {
         Vector3 pos = Vector3.Lerp(start, end, p);
         pos.y += _flightHeight * Mathf.Sin(Mathf.PI * p);
         t.position = pos;
         yield return null;
      }

      t.position = end;
   }
}
