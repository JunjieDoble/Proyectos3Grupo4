using UnityEngine;
using Doors;
using Interactions;

namespace _Code.Scripts.Doors
{
    public class InteractableDoor : MonoBehaviour, IInteractable, ILockable
    {

        [SerializeField] private Door door;
        [SerializeField] private bool locked;
        
        public bool IsLocked() => locked;
        
        private void Awake()
        {
            door = GetComponentInChildren<Door>();
        }

        public void Interact(IInteractor interactor)
        {
            if (IsLocked()) return;
            door.OpenDoor(!door.IsOpen());
        }
        
        public void Unlock()
        {
            locked = false;
        }

        public void Lock()
        {
            locked = true;
        }
    }
}

