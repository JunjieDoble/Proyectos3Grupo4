using UnityEngine;

namespace _Code.Scripts.Revamp.Bases
{
    public class Connector : Activator
    {
        [Header("Check Properties")]
        [SerializeField] private Vector3 checkCenter;
        [SerializeField] private Vector3 checkHalfExtents = new (1, 1, 1);
        [SerializeField] private LayerMask layerMask;
        
        private Connector _otherConnector;
        
        protected void SetOther(Connector other) => _otherConnector = other;

        private void Start()
        {
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
                if (CheckHit(hit)) return;
            }
        }

        public virtual bool CheckHit(Collider hit)
        {
            throw new System.NotImplementedException();
        }

        public void Connect()
        {
            Activate();
        }
        
        public void Disconnect()
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
            Gizmos.DrawWireCube(Vector3.zero, checkHalfExtents * 2);
        }
    }
}