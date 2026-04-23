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
        
        private void OnEnable()
        {
            Button.OnButtonPressed += CheckButtons;
            Button.OnButtonReleased += CloseDoor;
        }

        private void OnDisable()
        {
            Button.OnButtonPressed -= CheckButtons;
            Button.OnButtonReleased -= CloseDoor;
        }

        public void AddButton(Button button)
        {
            if (_buttons.Contains(button)) return;
            _buttons.Add(button);
            Debug.Log("Active buttons: " + _buttons.Count + "");
        }
        
        private void CheckButtons()
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
        
        private void CloseDoor()
        {
            door.OpenDoor(false);
        }
    
    }
}
