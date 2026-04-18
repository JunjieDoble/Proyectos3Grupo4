using UnityEngine;

namespace Doors
{
    public class Door : MonoBehaviour
    {

        private bool _isOpen;

        public void OpenDoor(bool open)
        {
            _isOpen = open;
            gameObject.SetActive(!open);
        }

        public bool IsOpen()
        {
            return _isOpen;
        }

    }
}