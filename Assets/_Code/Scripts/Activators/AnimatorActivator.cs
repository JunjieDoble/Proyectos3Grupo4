using System.Collections.Generic;
using _Code.Scripts.Bases;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

namespace _Code.Scripts.Activators
{
    public class AnimatorActivator : MonoBehaviour
    {
        [SerializeField]
        private Activable firstDoor;
        private Activator firstDoorActivator;

        void Start()
        {
            if (firstDoor)
            {
                firstDoorActivator = transform.AddComponent<Activator>();
                firstDoor.AddActivator(firstDoorActivator);
                firstDoorActivator.SetActive(false);
            }
        }
        
        public void ActivateFirstDoor()
        {
            firstDoorActivator.SetActive(true);
        }
    
    }
}
