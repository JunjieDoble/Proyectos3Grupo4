using _Code.Scripts.Doors;
using Interactions;
using UnityEngine;

namespace _Code.Scripts.Pickupables
{
    public class KeyPickup : PickupableBase
    {
        [SerializeField]
        private InteractableDoor door;

        private void Awake()
        {
            if (door == null) Debug.LogWarning("Key does not have a door", this);
        }

        private void Start()
        {
            door?.Lock();
        }

        public override void PickUp(IInteractor interactor)
        {
            door.Unlock();
            Destroy(gameObject);
        }
    }
}