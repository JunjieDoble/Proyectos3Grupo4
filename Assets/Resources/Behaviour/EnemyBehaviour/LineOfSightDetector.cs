using UnityEngine;

public class LineOfSightDetector : MonoBehaviour
{
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float detectionAngle = 90f;
    [SerializeField] private LayerMask playerLayerMask;

    [SerializeField] private bool showDebugVisuals = true;

    public GameObject PerformDetection(GameObject potentialTarget)
    {
        RaycastHit hit;
        Vector3 directionToPlayer = potentialTarget.transform.position - transform.position;
        Vector3 directionForward = transform.forward;
        float angleToPlayer = Vector3.Angle(directionForward, directionToPlayer);

        Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRange, playerLayerMask);

        if (hit.collider != null && hit.collider.gameObject == potentialTarget && angleToPlayer <= detectionAngle)
        {
            if (showDebugVisuals && this.enabled)
            {
                Debug.DrawLine(transform.position,
                               potentialTarget.transform.position, Color.green);
            }
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }
}
