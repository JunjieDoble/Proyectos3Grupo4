using System;
using System.Collections.Generic;
using _Code.Scripts.Bases;
using _Code.Scripts.Interactions;
using Interactions;
using TMPro;
using UnityEngine;

namespace _Code.Scripts.FinalRoom
{
    [RequireComponent(typeof(Animator))]
    public class SignalDevice : Activable, IInteractable
    {
        private static readonly int Active = Animator.StringToHash("IsActive");
        [SerializeField] private Material readyToActivateMaterial;
        [SerializeField] private Material inactivatedMaterial;
        [SerializeField] private GameObject squareHologram;
        [SerializeField]
        private List<Animator> activableComponents;
        private SignalVFX _signalVFX;
        private SignalParticleSystem _signalParticleSystem;
        private int _originalLayer;
        private bool _canSetOutlines = true;
        
        private void Start()
        {
            _signalVFX = GetComponentInChildren<SignalVFX>();
            _signalParticleSystem = GetComponentInChildren<SignalParticleSystem>();

            if (_signalVFX == null || _signalParticleSystem == null )
            {
                Debug.LogError("SignalDevice is missing required components!");
            }
            _originalLayer = gameObject.layer;
        }

        private void FixedUpdate()
        {
            ActivatorUpdate();
        }

        public override void ActivatorUpdate()
        {
            if (IsActive())
            {
                ActivateSignalDevice();
            }
        }

        public void ActivateSignalDevice()
        {
            Debug.Log("Activating Signal Device");
            squareHologram.GetComponent<MeshRenderer>().sharedMaterial = readyToActivateMaterial;

            TextMeshPro[] components = squareHologram.GetComponentsInChildren<TextMeshPro>();
            foreach (var component in components)
            {
                component.text = "ACTIVE";
            }
        }

        public void OnSignalParticleStart()
        {
            _signalParticleSystem.Activate();
        }

        public void OnSignalVFXStart()
        {
            _signalVFX.Activate();
        }

        public GameObject GameObject => gameObject;
    
        public void Interact(IInteractor interactor)
        {
            if (IsActive())
            {
                activableComponents.ForEach(component => component.SetBool(Active, true));
                _canSetOutlines = false;
                SetOutlinesRecursively(false);
            }
        }

        public void SetOutlines(bool value)
        {
            if (!_canSetOutlines) return;
            SetOutlinesRecursively(value);
        }

        private void SetOutlinesRecursively(bool value)
        {
            if (value && gameObject.layer != LayerMask.NameToLayer("Outline"))
            {
                _originalLayer = gameObject.layer;
            }
            gameObject.layer = value ? LayerMask.NameToLayer("Outline") : _originalLayer;
            Transform[] children = GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                child.gameObject.layer = value ? LayerMask.NameToLayer("Outline") : _originalLayer;
            }
        }
    }
}
