using System;
using UnityEngine;

namespace _Code.Scripts.Bases
{
    public class Activator : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] protected Activable activable;
        public Action onActivatorUpdate;
        public virtual bool IsActive { get; private set;}
        protected void Activate() => SetActive(true);
        protected void Deactivate() => SetActive(false);
        public void Toggle() => SetActive(!IsActive);
        public void SetActive(bool active) 
        {
            if (IsActive == active) return;
            IsActive = active;
            onActivatorUpdate?.Invoke();
        }

        protected virtual void Awake()
        {
            if (activable != null) activable.AddActivator(this);
        }
    }
}