using UnityEngine;

namespace Rooms
{
    public class Room : MonoBehaviour
    {
        private Quaternion _startRotation;
        private void Awake()
        {
            RoomRegistry.AddRoom(this);
        }

        public void StartRotate()
        {
            _startRotation = transform.rotation;
            transform.Rotate(0, 90, 0); //TODO: això és de mentres, fer-ho visual...
        }

        public void CancelRotate()
        {
            transform.rotation = _startRotation;
        }

        private void OnDestroy()
        {
            RoomRegistry.RemoveRoom(this);
        }
    }
}

