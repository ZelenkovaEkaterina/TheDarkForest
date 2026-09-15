using System;
using UnityEngine;

namespace Player
{
    public class PlayerInteraction : MonoBehaviour
    {
        public event Action<Type> OnLoot;
        
        private PlayerMovementComponent _movement;
        private Interactable _pending;
        private float _range;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovementComponent>();
        }

        public void RequestInteract(Interactable obj)
        {
            if (obj == null || !obj.IsAvailable) return;

            _pending = obj;
            _range = obj.InteractRange;
            
            Vector3 approach = obj.transform.position - 
                               (obj.transform.position - transform.position).normalized * _range;
            approach.y = obj.transform.position.y;
            _movement.MoveTo(approach);
            _movement.SetState(PlayerState.Interact);
            
            if (obj is LootItem loot)
                loot.OnLooted += HandleLooted;
            
        }

        public void Cancel()
        {
            if (_pending is LootItem loot)
                loot.OnLooted -= HandleLooted;   
            
            _pending = null;
            _movement.Stop();
            _movement.SetState(PlayerState.Idle);
        }

        private void Update()
        {
            if (_pending == null) return;

            float dist = Vector3.Distance(transform.position, _pending.transform.position);
            if (dist <= _range + 0.3f)
            {
                _pending.Interact(gameObject);
                _pending = null;
                _movement.Stop();
                _movement.SetState(PlayerState.Idle);
            }
        }
        
        private void HandleLooted(Type type, int amount)
        {
            if (_pending is LootItem loot)
                loot.OnLooted -= HandleLooted;

            OnLoot?.Invoke(type);
        }
    }
}