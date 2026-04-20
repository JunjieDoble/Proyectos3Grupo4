using _Code.Scripts.Doors;
using UnityEngine;
using Interactions;

namespace _Code.Scripts.Keys
{
    public class Key : MonoBehaviour, IInteractable
    {
        [SerializeField]
        private InteractableDoor door;
        [SerializeField]
        private bool canPickUp = true;
        
        private void Awake()
        {
            if (door == null) Debug.LogWarning("Key does not have a door", this);
        }

        private void Start()
        {
            door?.Lock();
        }

        void PickUp()
        {
            door.Unlock();
            Destroy(gameObject);
        }

        public bool CanInteract()
        {
            return canPickUp;
        }

        public void Interact()
        {
            if(CanInteract()) PickUp();
        }

        public void CancelInteract()
        {
                // Nothing to do
        }
    }
}