using System;
using _Code.Scripts.Gameplay;
using Interactions;
using Activator = _Code.Scripts.Bases.Activator;

namespace _Code.Scripts.Activators
{
    public class Terminal : Activator, IInteractable
    {
        private static Terminal _activeTerminal;

        private void OnEnable()
        {
            GameManager.OnPlayerRespawn += DeactivateAll;
        }
        
        private void OnDisable()
        {
            GameManager.OnPlayerRespawn -= DeactivateAll;
        }       

        private void DeactivateAll()
        {
            _activeTerminal?.SetActive(false);
            _activeTerminal = null;
            onActivatorUpdate?.Invoke();
        }

        public void Interact(IInteractor interactor)
        {
            _activeTerminal?.SetActive(false);
            SetActive(true);
            _activeTerminal = this;
            onActivatorUpdate?.Invoke();
        }
    }
}