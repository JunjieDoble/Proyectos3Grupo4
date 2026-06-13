using System;
using _Code.Scripts.Rooms;
using UnityEngine;

namespace _Code.Scripts.Bases
{
    public class Connector : Activator
    {
        [Header("Check Properties")]
        [SerializeField] private Vector3 checkCenter;
        [SerializeField] private Vector3 checkHalfExtents = new (1, 1, 1);
        [SerializeField] private LayerMask layerMask;
        
        protected Connector _otherConnector;
        public Connector OtherConnector => _otherConnector;
        
        protected internal void SetOther(Connector other) => _otherConnector = other;

        private void OnEnable()
        {
            Room.OnRoomReset += Reset;
        }
        
        private void OnDisable()
        {
            Room.OnRoomReset -= Reset;
        }       
        
        private void Reset()
        {
            Disconnect();
            CheckConnection();
        }

        private void Start()
        {
            CheckConnection();
        }
        
        public virtual void CheckConnection()
        {
            Vector3 worldCenter = transform.TransformPoint(checkCenter);
            Quaternion worldRotation = transform.rotation;
            Collider[] hits = new Collider[25];
            Vector3 overlapBoxHalfExtents = new Vector3(checkHalfExtents.x * transform.lossyScale.x, checkHalfExtents.y * transform.lossyScale.y, checkHalfExtents.z * transform.lossyScale.z);
            Physics.OverlapBoxNonAlloc(worldCenter, overlapBoxHalfExtents, hits, worldRotation, layerMask);
            foreach (var hit in hits)
            {
                if (CheckHit(hit)) return;
            }
        }

        protected virtual bool CheckHit(Collider hit)
        {
            throw new System.NotImplementedException();
        }

        protected void Connect()
        {
            Activate();
        }
        
        public virtual void Disconnect()
        {
            _otherConnector?.Deactivate();
            _otherConnector?.SetOther(null);
            _otherConnector = null;
            Deactivate();
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
            Vector3 overlapBoxHalfExtents = new Vector3(checkHalfExtents.x * transform.lossyScale.x, checkHalfExtents.y * transform.lossyScale.y, checkHalfExtents.z * transform.lossyScale.z);
            Gizmos.DrawWireCube(Vector3.zero, overlapBoxHalfExtents * 2);
        }
    }
}