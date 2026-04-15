using UnityEngine;
using UnityEngine.InputSystem;

namespace Rooms
{
    public class RoomRotator : MonoBehaviour
    {
        [SerializeField] private Room targetRoom;

        void Update()
        {
            //TODO: per debugar. s'ha de fer amb la interacció del player
            if (Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame)
            {
                RotateRoom();
            }
        }

        public void RotateRoom()
        {
            targetRoom?.Rotate();
        }
    }
}