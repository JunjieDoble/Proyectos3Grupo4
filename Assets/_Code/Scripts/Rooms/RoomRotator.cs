using _Code.Scripts.Rooms;
using UnityEngine;
using Interactions;

namespace Rooms
{
    public class RoomRotator : MonoBehaviour, IHoldInteractable, ILockable
    {
        [SerializeField] private Room targetRoom;

        private IInteractor _currentInteractor;

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

        public void OnHoldStarted(IInteractor interactor)
        {
            _currentInteractor = interactor;
            RotateRoom();
        }

        public void OnHoldCanceled(IInteractor interactor)
        {
            if (_currentInteractor != interactor) return;

            targetRoom?.CancelRotate();
            _currentInteractor = null;
        }

        public void OnHoldCompleted(IInteractor interactor)
        {
            if (_currentInteractor != interactor) return;

            _currentInteractor = null;
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