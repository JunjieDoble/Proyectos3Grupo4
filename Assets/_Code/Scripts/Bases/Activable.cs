using System.Collections.Generic;
using UnityEngine;

namespace _Code.Scripts.Bases
{
    public class Activable : MonoBehaviour
    {
        [SerializeField]
        private FMODUnity.EventReference activationSound;
        [SerializeField]
        private FMODUnity.EventReference deactivationSound;
        
        protected readonly List<Activator> activators = new();

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
        
        public void PlayStateSound(bool isActivated)
        {
            if (isActivated) PlayActivationSound();
            else PlayDeactivationSound();
        }
        
        private void PlayActivationSound()
        {
            if (!activationSound.IsNull)
            {
                FMODUnity.RuntimeManager.PlayOneShot(activationSound, transform.position);
            }
        }
        
        private void PlayDeactivationSound()
        {
            if (!deactivationSound.IsNull)
            {
                FMODUnity.RuntimeManager.PlayOneShot(deactivationSound, transform.position);
            }
        }
    }
}