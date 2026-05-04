using _Code.Scripts.Rooms;
using UnityEngine;
using Interactions;
using System.Collections;

namespace Rooms
{
    public class RoomRotator : MonoBehaviour, IHoldInteractable, ILockable
    {
        [SerializeField] private Room targetRoom;
        [SerializeField] private float rotationCooldown = 1.5f;

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
            if (_currentInteractor != null && _currentInteractor != interactor) return;

            _currentInteractor = interactor;
            RotateRoom();
            _currentInteractor = null;
        }

        public void OnHoldCanceled(IInteractor interactor)
        {
            if (_currentInteractor != null && _currentInteractor != interactor) return;
            targetRoom?.CancelRotate();

            _currentInteractor = null;
        }

        public void OnHoldCompleted(IInteractor interactor)
        {
            if (_currentInteractor != null && _currentInteractor != interactor) return;

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