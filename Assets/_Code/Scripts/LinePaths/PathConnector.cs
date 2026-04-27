using System;
using _Code.Scripts.Rooms;
using UnityEngine;

namespace _Code.Scripts.LinePaths
{
    public class PathConnector : MonoBehaviour, IConnector
    {
        [Header("References")]
        [SerializeField] Vector3 checkCenter;
        [SerializeField] Vector3 checkHalfExtents = new (1, 1, 1);
        [SerializeField] LayerMask layerMask;
        [SerializeField] private Path path;
        private bool _isActive;
        public bool IsActive => _isActive;

        void Awake()
        {
            path = GetComponentInParent<Path>();
            if (path == null) Debug.LogWarning("PathConnector does not have a path", this);
            Disconnect();
            CheckConnection();
        }

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
                if (hit == null) continue;
                IConnector connector = hit.GetComponentInParent<IConnector>();
                if (connector != null)
                {
                    if (connector is PathConnector otherPathConnector && otherPathConnector != this)
                    {
                        if (IsActive || otherPathConnector.IsActive)
                        {
                            Connect();
                            otherPathConnector.Connect();
                        }
                    }
                    if (connector is GeneratorConnector)
                    {
                        Connect();
                    }
                }
            }
        }
        
        public void Connect()
        {
            _isActive = true;
            path?.SetActive(_isActive);
        }

        public void Disconnect()
        {
            _isActive = false;
            path?.SetActive(_isActive);
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