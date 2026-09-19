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
        
        private float _interactTimer;
        [SerializeField] private float _interactDuration = 0.8f;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovementComponent>();
        }

        public void RequestInteract(Interactable obj)
        {
            if (obj == null || !obj.IsAvailable) return;

            // ★ на всякий случай отписываемся от предыдущего
            if (_pending is LootItem oldLoot)
                oldLoot.OnLooted -= HandleLooted;

            _pending = obj;
            _range = obj.InteractRange;
            _interactTimer = 0f;  

            Vector3 dir = obj.transform.position - transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.001f) dir = -transform.forward;

            Vector3 approach = obj.transform.position - dir.normalized * _range;
            approach.y = transform.position.y;               // ★ Y — от игрока, а не от предмета

            _movement.MoveTo(approach);                      // ★ SetState убран — им займётся Movement

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
            if (_pending == null)
            {
                // если мы в процессе проигрывания Interact — ждём таймер
                if (_movement.State == PlayerState.Interact)
                {
                    _interactTimer -= Time.deltaTime;
                    if (_interactTimer <= 0f)
                        _movement.SetState(PlayerState.Idle);
                }
                return;
            }

            float dist = Vector3.Distance(transform.position, _pending.transform.position);
            if (dist <= _range + 0.3f)
            {
                _movement.Stop();
                _movement.SetState(PlayerState.Idle);
                _interactTimer = _interactDuration;

                _pending.Interact(gameObject);    // лут выключается, OnLooted → OnLoot
                _pending = null;
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