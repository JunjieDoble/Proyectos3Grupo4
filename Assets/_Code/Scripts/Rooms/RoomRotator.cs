using UnityEngine;
using Interactions;

namespace Rooms
{
    public class RoomRotator : MonoBehaviour, IInteractable
    {
        [SerializeField] private Room targetRoom;

        public void Awake()
        {
            if (targetRoom == null) Debug.LogWarning("RoomRotator does not have a targetRoom", this);
        }

        public void RotateRoom()
        {
            targetRoom?.Rotate();
        }

        public void Interact()
        {
            RotateRoom();
        }

        public bool CanInteract()
        {
            return true; //TODO: afegir condicions més endavant
        }
    }
}