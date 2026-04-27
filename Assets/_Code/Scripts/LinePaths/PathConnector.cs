using System;
using System.Collections.Generic;
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

        void Awake()
        {
            path = GetComponentInParent<Path>();
            if (path == null) Debug.LogWarning("PathConnector does not have a path", this);
        }
        
        private void OnEnable()
        {
            Room.OnStartRotation += Disconnect;
            Room.OnEndRotation += CheckConnection;
        }
        
        private void OnDisable()
        {
            Room.OnStartRotation -= Disconnect;
            Room.OnEndRotation -= CheckConnection;
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
                if (!hit) continue;
                IConnector connector = hit.GetComponentInParent<IConnector>();
                if (connector != null)
                {
                    if (connector is PathConnector otherPathConnector && otherPathConnector != this)
                    {
                        if (path.IsActive || otherPathConnector.path.IsActive)
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
            path?.SetActive(true);
        }

        public void Disconnect()
        {
            path?.SetActive(false);
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