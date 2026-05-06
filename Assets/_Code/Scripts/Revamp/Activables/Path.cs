using System.Collections.Generic;
using _Code.Scripts.Revamp.Activators.Connectors;
using _Code.Scripts.Revamp.Bases;
using UnityEngine;

namespace _Code.Scripts.Revamp.Activables
{
    public class Path : Activable
    {
        [Header("References")]
        [SerializeField] private List<MeshRenderer> pathMeshRenderers;
        [SerializeField] private Material activeMaterial;
        [SerializeField] private Material inactiveMaterial;
        [SerializeField] private Activable target;
        private GeneratorConnector _generatorConnector;
        
        private bool _isActive;
        public override bool IsActive()
        {
            if (_generatorConnector) return true;
            return activators.Exists(a => a.IsActive);
        }
        
        private void Awake()
        {
            if (pathMeshRenderers == null || pathMeshRenderers.Count == 0)
            {
                pathMeshRenderers = new List<MeshRenderer>(GetComponentsInChildren<MeshRenderer>());
                if (pathMeshRenderers.Count == 0) Debug.LogWarning("Path does not have any MeshRenderers", this);
            }
        }

        private void Start()
        {
            if (target && activators.Count > 0) activators.ForEach(a => target.AddActivator(a));
        }
        
        public override void ActivatorUpdate()
        {
            if (IsActive() == _isActive) return;
            _isActive = IsActive();
            activators.ForEach(a => a.SetActive(_isActive));
            pathMeshRenderers.ForEach(mr => mr.sharedMaterial = _isActive ? activeMaterial : inactiveMaterial);
        }
        
        public void SetGenerator(GeneratorConnector generatorConnector)
        {
            _generatorConnector = generatorConnector;
        }
    }
}