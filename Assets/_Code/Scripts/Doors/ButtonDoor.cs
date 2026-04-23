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
            Button.OnButtonReleased += CheckButtons;
        }

        private void OnDisable()
        {
            Button.OnButtonPressed -= CheckButtons;
            Button.OnButtonReleased -= CheckButtons;
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
                    door.OpenDoor(false);
                };
            }
            door.OpenDoor(true);
        }
    
    }
}
