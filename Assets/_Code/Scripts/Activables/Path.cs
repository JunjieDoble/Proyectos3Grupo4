using System.Collections.Generic;
using _Code.Scripts.Activators.Connectors;
using _Code.Scripts.Bases;
using JetBrains.Annotations;
using UnityEngine;

namespace _Code.Scripts.Activables
{
    public class Path : Activable
    {
        [Header("References")]
        [SerializeField] private Material activeMaterial;
        [SerializeField] private Material inactiveMaterial;
        [SerializeField] [ItemCanBeNull] private List<Activable> target = new();
        private List<MeshRenderer> pathMeshRenderers = new();
        
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
            if (target.Count > 0 && activators.Count > 0) 
                target.ForEach(a => activators.ForEach(a.AddActivator));
        }
        
        public override void ActivatorUpdate()
        {
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