using Doors;
using UnityEngine;

namespace _Code.Scripts.RoomsV2
{

    public enum DoorLevel
    {
        Bottom = 0,
        Top = 1
    }
    public class Wall : MonoBehaviour
    {
        [SerializeField] private Door topDoor;
        [SerializeField] private Door bottomDoor;
        
        public Door TopDoor => topDoor;
        public Door BottomDoor => bottomDoor;

        public Door GetDoor(DoorLevel level)
        {
            return level == DoorLevel.Bottom ? BottomDoor : TopDoor;
        }
        
        public bool HasDoor(DoorLevel level)
        {
            return GetDoor(level) != null;
        }

        public void SetDoorOpen(DoorLevel level, bool open)
        {
            Door door = GetDoor(level);
            if (door != null) door.OpenDoor(open);
        }

        public void CloseALlDoors()
        {
            SetDoorOpen(DoorLevel.Bottom, false);
            SetDoorOpen(DoorLevel.Top, false);
        }
    }
}
