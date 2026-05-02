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
        private Coroutine _rotationCoroutine;

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

            if (_rotationCoroutine != null) StopCoroutine(_rotationCoroutine);
            _rotationCoroutine = StartCoroutine(RotationCooldown());
        }

        public void OnHoldCanceled(IInteractor interactor)
        {
            if (_currentInteractor != null && _currentInteractor != interactor) return;
            targetRoom?.CancelRotate();

            if (_rotationCoroutine != null) StopCoroutine(_rotationCoroutine);
            _currentInteractor = null;
        }

        public void OnHoldCompleted(IInteractor interactor)
        {
            if (_currentInteractor != null && _currentInteractor != interactor) return;

            if (_rotationCoroutine != null) StopCoroutine(_rotationCoroutine);
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

        private IEnumerator RotationCooldown()
        {
            yield return new WaitForSeconds(rotationCooldown);

            _currentInteractor = null;
            _rotationCoroutine = null;
        }
    }
}