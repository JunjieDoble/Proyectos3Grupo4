using System.Collections.Generic;
using UnityEngine;

namespace _Code.Scripts.LinePaths
{
    public class Path : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<MeshRenderer> pathMeshRenderers;
        [SerializeField] private Material activeMaterial;
        [SerializeField] private Material inactiveMaterial;

        private void Awake()
        {
            if (pathMeshRenderers == null || pathMeshRenderers.Count == 0)
            {
                pathMeshRenderers = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
                if (pathMeshRenderers.Count == 0) Debug.LogWarning("Path does not have any MeshRenderers", this);
            }
        }
        
        public void SetActive(bool active)
        {
            foreach (var meshRenderer in pathMeshRenderers)
            {
                meshRenderer.material = active ? activeMaterial : inactiveMaterial;
            }
        }
    }
}