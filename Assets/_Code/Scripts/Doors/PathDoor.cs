using System;
using Doors;
using Interactions;
using UnityEngine;

namespace _Code.Scripts.Doors
{
    public class PathDoor : MonoBehaviour, ILockable
    {
        
        [SerializeField] private Door door;
        
        private void Awake()
        {
            door = GetComponentInChildren<Door>();
            if (door == null) Debug.LogWarning("PathDoor does not have a door", this);
        }
        
        public bool IsLocked() => door.IsOpen();

        public void Lock()
        {
            door.OpenDoor(false);
        }

        public void Unlock()
        {
            door.OpenDoor(true);
        }
    }
}