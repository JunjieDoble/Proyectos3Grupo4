using System;
using System.Collections.Generic;
using Doors;
using Interactions;
using UnityEngine;

namespace _Code.Scripts.Doors
{
    public class ButtonDoor : MonoBehaviour
    {
        [SerializeField] private Door door;
        
        private List<Button> _buttons = new();

        private void Awake()
        {
            door = GetComponentInChildren<Door>();
        }

        public void AddButton(Button button)
        {
            if (_buttons.Contains(button)) return;
            _buttons.Add(button);
            Debug.Log("Active buttons: " + _buttons.Count + "");
        }
        
        public void CheckButtons()
        {
            foreach (var button in _buttons)
            {
                if (!button.IsActive)
                {
                    Debug.Log("Button not active: "+" "+button.gameObject.name);
                    door.OpenDoor(false);
                    return;
                }
            }
            Debug.Log("All buttons active");
            door.OpenDoor(true);
        }
        
        public void CloseDoor()
        {
            door.OpenDoor(false);
        }
    
    }
}
