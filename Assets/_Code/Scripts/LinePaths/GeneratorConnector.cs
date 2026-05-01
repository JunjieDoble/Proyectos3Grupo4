using _Code.Scripts.Rooms;
using UnityEngine;

namespace _Code.Scripts.LinePaths
{
    public class GeneratorConnector : MonoBehaviour, IConnector
    {
        [Header("References")]
        [SerializeField] Vector3 checkCenter;
        [SerializeField] Vector3 checkHalfExtents = new (1, 1, 1);
        [SerializeField] LayerMask layerMask;
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
                PathConnector pathConnector = hit.GetComponent<PathConnector>();
                if (pathConnector != null)
                {
                    Connect();
                    pathConnector.Connect();
                    pathConnector.SetGenerator(this);
                    break;
                }
                Path path = hit.GetComponentInParent<Path>();
                if (path)
                {
                    path.SetGenerator(this);
                    path.SetActive(true);
                    break;
                }
            }
        }

        public void Connect()
        {
            // TODO: Generator animations or effects
        }

        public void Disconnect()
        {
            // TODO: Generator animations or effects
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