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

        public void Interact(IInteractor interactor)
        {
            if (IsLocked()) return;
            OnHoldStarted(interactor);
        }

        public void OnHoldStarted(IInteractor interactor) => RotateRoom();

        public void OnHoldCanceled(IInteractor interactor)
        {
            targetRoom?.CancelRotate();
        }

        public void OnHoldCompleted(IInteractor interactor)
        {
            throw new System.NotImplementedException();
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