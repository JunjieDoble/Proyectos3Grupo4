using System.Collections;
using Interactions;
using UnityEngine;

namespace _Code.Scripts.RoomsV2
{
    public class RoomRotator : MonoBehaviour, IHoldInteractable
    {
        [SerializeField] private Room targetRoom;
        [SerializeField] private float rotationDuration = 1.2f;
        [SerializeField] private bool rotateClockwise = true;

        private Coroutine _rotationCoroutine;
         private bool _isRotating;
         
        public void Interact(IInteractor interactor)
        {
            
        }

        public void OnHoldStarted(IInteractor interactor)
        {
            
        }

        public void OnHoldCanceled(IInteractor interactor)
        {
            
        }

        public void OnHoldCompleted(IInteractor interactor)
        {
            if (_isRotating || targetRoom == null) return;
            
            _rotationCoroutine = StartCoroutine(RotateRoomRoutine());
        }
        
        private IEnumerator RotateRoomRoutine()
        {
            _isRotating = true;

            Transform roomTransform = targetRoom.transform;
            Quaternion startRotation = roomTransform.rotation;
            Quaternion endRotation = startRotation * Quaternion.Euler(0f, rotateClockwise ? 90f : -90f, 0f);

            float elapsed = 0f;

            while (elapsed < rotationDuration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / rotationDuration);
                roomTransform.rotation = Quaternion.Slerp(startRotation, endRotation, t);
                yield return null;
            }

            roomTransform.rotation = endRotation;
            targetRoom.AddRotationStepClockwise();

            targetRoom.RefreshRoom();

            _isRotating = false;
            _rotationCoroutine = null;
        }
        
    }
}