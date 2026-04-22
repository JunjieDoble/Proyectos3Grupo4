using UnityEngine;
using Interactions;

namespace Rooms
{
    public class RoomRotator : MonoBehaviour, IHoldInteractable, ILockable
    {
        [SerializeField] private Room targetRoom;

        public void Awake()
        {
            if (targetRoom == null) Debug.LogWarning("RoomRotator does not have a targetRoom", this);
        }

        public void RotateRoom()
        {
            targetRoom?.StartRotate();
        }

        public void Interact()
        {
            if (IsLocked()) return;
            OnHoldStarted();
        }

        public void OnHoldStarted() => RotateRoom();

        public void OnHoldCanceled()
        {
            targetRoom?.CancelRotate();
        }

        public bool IsLocked()
        {
            return false;
        }

        public void Lock()
        {
            // TODO: Implement lock
        }

        public void Unlock()
        {
            // TODO: Implement unlock
        }
    }
}