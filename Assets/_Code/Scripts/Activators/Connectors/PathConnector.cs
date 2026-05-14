using System;
using _Code.Scripts.Activables;
using _Code.Scripts.Bases;
using _Code.Scripts.Rooms;
using UnityEngine;

namespace _Code.Scripts.Activators.Connectors
{
    public class PathConnector : Connector
    {
        protected override void Awake()
        {
            activables.Add(GetComponentInParent<Path>());
            if (activables == null || activables.Count == 0) Debug.LogWarning("PathConnector does not have a path", this);
            else
            {
                activables.ForEach(a => a.AddActivator(this));
            }
        }

        private void OnEnable()
        {
            Room.OnEndRotation += CheckConnection;
        }
        
        private void OnDisable()
        {
            Room.OnEndRotation -= CheckConnection;
        }

        public override void CheckConnection()
        {
            base.CheckConnection();
            if (OtherConnector is PathConnector pathConnector)
                pathConnector.activables.ForEach(a => a?.ActivatorUpdate());
            activables.ForEach(a => a?.ActivatorUpdate());
        }

        protected override bool CheckHit(Collider hit)
        {
            if (hit == null) return false;
            Connector other = hit.GetComponentInParent<Connector>();
            if (other != null)
            {
                if (other is PathConnector otherPathConnector && otherPathConnector != this)
                {
                    SetOther(otherPathConnector);
                    otherPathConnector.SetOther(this);
                    return true;
                }

                if (other is GeneratorConnector)
                {
                    Connect();
                    return true;
                }
            }
            return false;
        }
        
        public override void Disconnect()
        {
            if (IsActive)
            {
                Deactivate();
            }
            else
            {
                _otherConnector?.SetActive(false);
            }
            _otherConnector?.SetOther(null);
            _otherConnector = null;
        }
        
    }
}