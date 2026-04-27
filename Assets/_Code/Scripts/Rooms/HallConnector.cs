using Doors;
using UnityEngine;

namespace _Code.Scripts.Rooms
{ 
    public class HallConnector : MonoBehaviour, IConnector
    {
        [Header("References")]
        [SerializeField] private Door door;
        [SerializeField] Vector3 checkCenter;
        [SerializeField] Vector3 checkHalfExtents = new (1, 1, 1);
        [SerializeField] LayerMask layerMask;
        private Wall _wall;
        
        private void Awake()
        {
            if (door == null)
            {
                door = GetComponentInChildren<Door>();
                if (door == null) Debug.LogWarning("HallConnector does not have a door", this);
                else door.OpenDoor(false);
            }
            else
            {
                door.OpenDoor(false);
            }
            CheckConnection();
        }

        public void CheckConnection()
        {             
            Vector3 worldCenter = transform.TransformPoint(checkCenter);
            Quaternion worldRotation = transform.rotation;
            Collider[] hits = new Collider[10];
            Physics.OverlapBoxNonAlloc(worldCenter, checkHalfExtents, hits, worldRotation, layerMask); 
            foreach (var hit in hits) 
            { 
                if (hit == null) continue;
                IConnector connector = hit.GetComponentInParent<IConnector>();
                 if (connector != null)
                {
                    if (connector is HallConnector otherHallConnector && otherHallConnector != this && _wall != otherHallConnector._wall)
                    {
                        otherHallConnector.Connect();
                        Connect();
                    }
                }
            }
        }

        public void Connect()
        {
            door?.OpenDoor(true);
        }

        public void Disconnect()
        {
            door?.OpenDoor(false);
        }
        
        void OnDrawGizmos()
        {
            
            Gizmos.color = Color.green;

            Matrix4x4 matrix = Matrix4x4.TRS(
                transform.TransformPoint(checkCenter),
                transform.rotation,
                Vector3.one
            );

            Gizmos.matrix = matrix;
            Gizmos.DrawWireCube(Vector3.zero, checkHalfExtents * 2);
        }

        public void SetWall(Wall wall)
        {
            _wall = wall;
        }
    }
}