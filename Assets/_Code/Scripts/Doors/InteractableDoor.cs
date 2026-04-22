using UnityEngine;
using Doors;
using Interactions;

namespace _Code.Scripts.Doors
{
    public class InteractableDoor : MonoBehaviour, IInteractable
    {

        [SerializeField] private Door door;
        [SerializeField] private bool active;
        
        private void Awake()
        {
            door = GetComponentInChildren<Door>();
        }
        
        public bool CanInteract()
        {
            return active;
        }

        public void Interact()
        {
            if(CanInteract()) door.OpenDoor(!door.IsOpen());
        }

        public void CancelInteract()
        {
            // Nothing to do
        }
        
        public void Unlock()
        {
            active = true;
        }
        
        public void Lock()
        {
            active = false;
        }
    }
}

