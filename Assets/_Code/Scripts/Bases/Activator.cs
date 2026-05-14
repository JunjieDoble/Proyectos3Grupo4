using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Code.Scripts.Bases
{
    public class Activator : MonoBehaviour
    {
        [Header("References")] [SerializeField]
        protected List<Activable> activables = new();
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
            if (activables != null && activables.Count > 0)
                activables.ForEach(a => a?.AddActivator(this));
        }
    }
}