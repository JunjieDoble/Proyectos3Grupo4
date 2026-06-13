using _Code.Scripts.Bases;
using UnityEngine;

namespace _Code.Scripts.Activables
{
    public class AnimationToggle : Activable
    {
        bool _isActive;
        
        public override void ActivatorUpdate()
        { 
            if (_isActive != IsActive())
            {
                _isActive = IsActive();
                PlayStateSound(_isActive);
            }
            gameObject.GetComponent<Animator>()?.SetBool("IsActive", IsActive());
        }
    }
}