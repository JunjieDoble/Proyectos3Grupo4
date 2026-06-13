using System;
using _Code.Scripts.Gameplay;
using Interactions;
using UnityEngine;
using Activator = _Code.Scripts.Bases.Activator;

namespace _Code.Scripts.Activators
{
    public class Terminal : Activator, IInteractable
    {
        private static Terminal _activeTerminal;
        [SerializeField]
        private FMODUnity.EventReference activationSound;
        [SerializeField]
        private FMODUnity.EventReference deactivationSound;

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
            if (_activeTerminal)
            {
                _activeTerminal.SetActive(false);
                if (!deactivationSound.IsNull)
                {
                    FMODUnity.RuntimeManager.PlayOneShot(deactivationSound, _activeTerminal.transform.position);
                }
            }
            _activeTerminal?.SetActive(false);
            SetActive(true);
            _activeTerminal = this;
            onActivatorUpdate?.Invoke();
            if (!activationSound.IsNull)
            {
                FMODUnity.RuntimeManager.PlayOneShot(activationSound, transform.position);
            }
        }
    }
}