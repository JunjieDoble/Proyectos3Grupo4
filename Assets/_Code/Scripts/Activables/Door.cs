using _Code.Scripts.Bases;
using UnityEngine;

namespace _Code.Scripts.Activables
{
    public class Door : Activable
    {
        public override void ActivatorUpdate()
        {
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