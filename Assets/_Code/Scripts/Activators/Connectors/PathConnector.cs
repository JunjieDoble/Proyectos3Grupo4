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
            activable = GetComponentInParent<Path>();
            if (activable == null) Debug.LogWarning("PathConnector does not have a path", this);
            else
            {
                activable.AddActivator(this);
            }
        }

        private void OnEnable()
        {
            Room.OnEndRotation += DisconnectAndCheck;
        }

        private void DisconnectAndCheck()
        {
            Disconnect();
            CheckConnection();
        }
        
        private void OnDisable()
        {
            Room.OnEndRotation -= DisconnectAndCheck;
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
                    if (activable.IsActive() || otherPathConnector.activable.IsActive())
                    {
                        Connect();
                        otherPathConnector.Connect();
                    }
                    Debug.Log("Connected to " + other.name);
                    return true;
                }

                if (other is GeneratorConnector)
                {
                    Debug.Log("Connected to " + other.name);
                    Connect();
                    return true;
                }
            }
            return false;
        }
    }
}