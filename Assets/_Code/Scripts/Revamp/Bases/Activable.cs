using System.Collections.Generic;
using UnityEngine;

namespace _Code.Scripts.Revamp.Bases
{
    public class Activable : MonoBehaviour
    {
        protected List<Activator> activators = new();

        public void AddActivator(Activator activator)
        {
            if (activators.Contains(activator)) return;
            activators.Add(activator);
            activator.onActivatorUpdate += ActivatorUpdate;
        }

        public void RemoveActivator(Activator activator)
        {
            if (!activators.Contains(activator)) return;
            activators.Remove(activator);
            activator.onActivatorUpdate -= ActivatorUpdate;
        }
        public virtual bool IsActive() => activators.Count == 0 || activators.TrueForAll(a => a.IsActive);
        public virtual void ActivatorUpdate() {}
    }
}