using UnityEngine;

public class RangeDetector : MonoBehaviour
{
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private LayerMask detectionMask;
    [SerializeField] private bool showDebugVisuals = true;

    public GameObject DetectedTarget { get; private set; }

    public GameObject UpdateDetector()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, detectionMask);

        if (colliders.Length > 0)
        {
            DetectedTarget = colliders[0].gameObject;
        }
        else
        {
            DetectedTarget = null;
        }

        return DetectedTarget;
    }

    private void OnDrawGizmos()
    {
        if (!showDebugVisuals && !enabled) return;

        Gizmos.color = DetectedTarget != null ? Color.red : Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
