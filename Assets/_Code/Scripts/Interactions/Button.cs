using System;
using _Code.Scripts.Doors;
using Unity.VisualScripting;
using UnityEngine;

namespace Interactions
{
    public class Button : MonoBehaviour
    {

        [SerializeField] private ButtonDoor buttonDoor;
        
        public static Action OnButtonPressed;
        public static Action OnButtonReleased;
        
        private bool _isActive;
        
        public bool IsActive => _isActive;

        private void Awake()
        {
            if (buttonDoor == null) Debug.LogWarning("ButtonInteractor does not have a buttonDoor", this);
            buttonDoor?.AddButton(this);
        }
        
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Button pressed!");
            _isActive = true;
            OnButtonPressed?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Button released!");
            _isActive = false;
            OnButtonReleased?.Invoke();
        }
    }
}