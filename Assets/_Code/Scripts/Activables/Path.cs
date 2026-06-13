using System.Collections.Generic;
using _Code.Scripts.Activators.Connectors;
using _Code.Scripts.Bases;
using UnityEngine;

namespace _Code.Scripts.Activables
{
    public class Path : Activable
    {
        [Header("References")]
        [SerializeField] private Material activeMaterial;
        [SerializeField] private Material inactiveMaterial;
        [SerializeField] private Activable target;
        private List<MeshRenderer> pathMeshRenderers;
        
        private bool _isActive;
        public override bool IsActive()
        {
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
            if (_isActive == IsActive()) return;
            PlayStateSound(IsActive());
            _isActive = IsActive();

            foreach (var activator in activators)
            {
                if (activator.IsActive) continue;
                if (activator is Connector connector)
                {
                    if (connector.OtherConnector is PathConnector otherPathConnector)
                    {
                        otherPathConnector.SetActive(_isActive);
                    }
                }
            }
            
            pathMeshRenderers.ForEach(mr => mr.sharedMaterial = _isActive ? activeMaterial : inactiveMaterial);
        }
    }
}