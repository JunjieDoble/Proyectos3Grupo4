using System;
using UnityEngine;

public class MinimapCamera : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera minimapCamera;
    [SerializeField] private Transform player;

    private float nearPlaneTarget;

    private void Update()
    {
        if (player == null || minimapCamera == null) return;
        minimapCamera.nearClipPlane = Mathf.Lerp(minimapCamera.nearClipPlane, nearPlaneTarget, Time.deltaTime * 5f);
    }

    private void FixedUpdate()
    {
        if (player == null || minimapCamera == null) return;
        nearPlaneTarget = CalculateNearClipPlane();
    }

    private float CalculateNearClipPlane()
    {
        Ray ray = new Ray(player.position + Vector3.up, player.up);
        if (Physics.Raycast(ray, out RaycastHit hit, (player.position - transform.position).magnitude, LayerMask.GetMask("Default")))
        {
            return transform.position.y - hit.point.y;
        }
        return 0.1f;
    }
}
