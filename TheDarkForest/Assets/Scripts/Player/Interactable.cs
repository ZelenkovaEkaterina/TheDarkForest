using UnityEngine;

namespace Player
{
    public abstract class Interactable : MonoBehaviour
    {
        [SerializeField] protected float _interactRange = 1.5f;

        public float InteractRange => _interactRange;
        public virtual bool IsAvailable => gameObject.activeInHierarchy;

        public void Interact(GameObject actor) => OnInteract(actor);
        protected abstract void OnInteract(GameObject actor);
    }
}