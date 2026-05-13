using _Code.Scripts.Bases;
using UnityEngine;

namespace _Code.Scripts.Activables
{
    public class AnimationToggle : Activable
    {
        public override void ActivatorUpdate()
        { 
            gameObject.GetComponent<Animator>()?.SetBool("IsActive", IsActive());
        }
    }
}