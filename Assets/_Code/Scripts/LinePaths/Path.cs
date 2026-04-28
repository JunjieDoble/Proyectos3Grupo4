using System.Collections.Generic;
using Interactions;
using UnityEngine;

namespace _Code.Scripts.LinePaths
{
    public class Path : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private List<MeshRenderer> pathMeshRenderers;
        [SerializeField] private Material activeMaterial;
        [SerializeField] private Material inactiveMaterial;
        [SerializeField] private GameObject lockableTarget;
        
        private bool _isActive;
        public bool IsActive => _isActive || _generatorConnector;
        private List<PathConnector> _connectors = new();
        private GeneratorConnector _generatorConnector;

        private void Awake()
        {
            if (pathMeshRenderers == null || pathMeshRenderers.Count == 0)
            {
                pathMeshRenderers = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
                if (pathMeshRenderers.Count == 0) Debug.LogWarning("Path does not have any MeshRenderers", this);
            }
        }
        
        public void AddConnector(PathConnector connector) => _connectors.Add(connector);
        
        public void SetActive(bool active)
        {
            if (_generatorConnector && !active) return;
            if (_isActive == active) return;
            _isActive = active;
            foreach (var meshRenderer in pathMeshRenderers)
            {
                meshRenderer.material = active ? activeMaterial : inactiveMaterial;
            }
            
            if (lockableTarget)
            {
                ILockable lockable = lockableTarget.GetComponent<ILockable>();
                if (lockable != null)
                {
                    if (active) lockable.Unlock();
                    else lockable.Lock();
                }
            }
        }

        public void Activate(PathConnector pathConnector)
        {
            if (IsActive) return;
            SetActive(true);
            foreach (PathConnector connector in _connectors)
            {
                if (connector == pathConnector) continue;
                connector.Connect();
            }
        }

        public void SetGenerator(GeneratorConnector generatorConnector)
        {
            _generatorConnector = generatorConnector;
        }
    }
}