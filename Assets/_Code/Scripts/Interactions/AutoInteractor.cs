using System;
using Interactions;
using UnityEngine;

namespace _Code.Scripts.Interactions
{
    public class AutoInteractor : MonoBehaviour, IInteractor
    {
    
        [SerializeReference] private GameObject interactableObject;
        [SerializeField] private float loopTime = 5f;
        private float _time;
        private IInteractable interactable;
    
        public Transform Transform => transform;
        public GameObject GameObject => gameObject;

        private void Awake()
        {
            interactable = interactableObject.GetComponent<IInteractable>();
            if (interactable == null) Debug.LogWarning("AutoInteractor does not have an interactable", this);
        }

        private void Update()
        {
            _time += Time.deltaTime;
            if (_time >= loopTime)
            {
                interactable?.Interact(this);
                _time = 0;
            }
        }
    
    }
}
