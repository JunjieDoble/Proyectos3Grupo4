using System.Collections.Generic;
using _Code.Scripts.Pickupables;
using UnityEngine;
using Doors;
using Interactions;

namespace _Code.Scripts.Doors
{
    public class InteractableDoor : MonoBehaviour, IInteractable, ILockable
    {

        [SerializeField] private Door door;
        [SerializeField] private bool locked;

        private List<KeyPickup> _keys = new ();
        
        public bool IsLocked() => _keys.Count > 0 || locked;

        public void AddKey(KeyPickup key)
        {
            if (key == null) return;
            _keys.Add(key);
            Lock();
        }

        public void RemoveKey(KeyPickup key)
        {
            if (key == null) return;
            _keys.Remove(key);
            if (_keys.Count == 0) Unlock();
        }
        
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
            door.OpenDoor(false);
        }
    }
}

