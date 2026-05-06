using System;
using UnityEngine;

namespace _Code.Scripts.Revamp.Bases
{
    public class Activator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected Activable activable;
        public Action onActivatorUpdate;
        public virtual bool IsActive { get; private set;}
        public void Activate() => SetActive(true);
        public void Deactivate() => SetActive(false);
        public void Toggle() => SetActive(!IsActive);
        public void SetActive(bool active) 
        {
            if (IsActive == active) return;
            IsActive = active;
            onActivatorUpdate?.Invoke();
        }

        private void Awake()
        {
            if (activable != null) activable.AddActivator(this);
        }
    }
}