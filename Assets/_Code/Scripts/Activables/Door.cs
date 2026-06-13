using _Code.Scripts.Bases;
using UnityEngine;

namespace _Code.Scripts.Activables
{
    public class Door : Activable
    {
        
        private bool _isActive;
        
        public override void ActivatorUpdate()
        {
            if (_isActive == IsActive()) return;
            _isActive = IsActive();
            PlayStateSound(_isActive);
            SetDoorOpen(IsActive());
        }

        private void SetDoorOpen(bool open)
        {
            if (gameObject.GetComponent<Animator>() != null)
            {
                gameObject.GetComponent<Animator>().SetBool("IsActive", open);
            }
            else
                gameObject.SetActive(!open);
        }
    }
}