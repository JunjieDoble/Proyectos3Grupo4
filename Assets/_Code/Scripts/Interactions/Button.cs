using System;
using _Code.Scripts.Doors;
using UnityEngine;

namespace Interactions
{
    [RequireComponent(typeof(Collider))]
    public class Button : MonoBehaviour
    {

        [SerializeField] private ButtonDoor buttonDoor;
        [SerializeField] private GameObject buttonObject;
        
        public static Action OnButtonPressed;
        public static Action OnButtonReleased;
        
        private int _isActive;
        
        public bool IsActive => _isActive > 0;

        private void Awake()
        {
            if (buttonDoor == null) Debug.LogWarning("ButtonInteractor does not have a buttonDoor", this);
            if (buttonObject == null) buttonObject = gameObject;
            buttonDoor?.AddButton(this);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Button pressed!");
            _isActive++;            
            if (_isActive == 1)
            {
                PressButton();
            }
            Debug.Log("Colliders: " + _isActive + "");
        }

        private void PressButton()
        {
            buttonObject.transform.localPosition += Vector3.down * 0.1f;
            buttonDoor?.CheckButtons();
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Button released!");
            _isActive--;            
            Debug.Log("Colliders: " + _isActive + "");
            if (_isActive == 0)
                ReleaseButton();
        }

        private void ReleaseButton()
        {
            buttonObject.transform.localPosition -= Vector3.down * 0.1f;
            buttonDoor?.CloseDoor();
        }
    }
}