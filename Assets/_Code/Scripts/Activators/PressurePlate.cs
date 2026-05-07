using _Code.Scripts.Bases;
using UnityEngine;

namespace _Code.Scripts.Activators
{
    [RequireComponent(typeof(Collider))]
    public class PressurePlate : Activator
    {
        [Header("References")]
        [SerializeField] private Activable buttonObject;
        
        private int _buttonState;

        public override bool IsActive => _buttonState > 0;

        protected override void Awake()
        {
            base.Awake();
            if (!buttonObject) buttonObject = GetComponentInChildren<Activable>();            
            if (buttonObject) buttonObject.AddActivator(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.isTrigger) return;
            _buttonState++;
            if (_buttonState == 1)
                onActivatorUpdate?.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.isTrigger) return;
            _buttonState--;
            if (_buttonState == 0)
                onActivatorUpdate?.Invoke();           
        }
        
    }
}