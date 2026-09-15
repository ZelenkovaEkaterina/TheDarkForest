using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{ 
    public class PlayerInputRouter : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private LayerMask _targetMask;

        private PlayerMovementComponent _movement;
        private PlayerCombat _combat;
        private PlayerInteraction _interaction;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovementComponent>();
            _combat = GetComponent<PlayerCombat>();
            _interaction = GetComponent<PlayerInteraction>();

            if (_mainCamera == null) _mainCamera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                HandleLeftClick();
        }

        private void HandleLeftClick()
        {
            if (_mainCamera == null) return;
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hitTarget, Mathf.Infinity, _targetMask))
            {
                var go = hitTarget.collider.gameObject;

                if (go.CompareTag("Enemy"))
                {
                    _combat.SetTarget(hitTarget.collider.transform);
                    _movement.MoveToTarget(hitTarget.collider.transform, _combat.AttackRange - 0.2f);
                    return;
                }

                var loot = hitTarget.collider.GetComponentInParent<LootItem>();
                if (loot != null && loot.gameObject.activeInHierarchy)
                {
                    _combat.CancelCombat();
                    _interaction.RequestInteract(loot);
                    return;
                }
            }
            
            if (Physics.Raycast(ray, out RaycastHit hitGround, Mathf.Infinity, _groundMask))
            {
                _combat.CancelCombat(); 
                _interaction.Cancel();
                _movement.MoveTo(hitGround.point);
            }
        }
    }
}

