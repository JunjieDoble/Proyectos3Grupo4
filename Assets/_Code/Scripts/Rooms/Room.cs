using UnityEngine;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        private void Awake()
        {
            RoomRegistry.AddRoom(this);
        }

        public void Rotate()
        {
            transform.Rotate(0, 90, 0); //TODO: això és de mentres, fer-ho visual o dreta i esquerra...
        }

        private void OnDestroy()
        {
            RoomRegistry.RemoveRoom(this);
        }
    }
}

