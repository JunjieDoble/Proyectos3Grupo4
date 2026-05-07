using _Code.Scripts.Revamp.Activables;
using _Code.Scripts.Revamp.Bases;
using _Code.Scripts.Rooms;
using UnityEngine;

namespace _Code.Scripts.Revamp.Activators.Connectors
{
    public class HallConnector : Connector
    {
        
        private Wall _wall;
        
        private void Awake()
        {
            activable = GetComponentInChildren<Door>();
            if (activable == null) Debug.LogWarning("HallConnector does not have a Door in its children", this);
            else activable.AddActivator(this);
        }

        public override bool CheckHit(Collider hit)
        {
            if (hit == null) return false;
            HallConnector otherHallConnector = hit.GetComponentInParent<HallConnector>();
            if (otherHallConnector != null)
            {
                if (otherHallConnector != this && _wall != otherHallConnector._wall)
                {
                    otherHallConnector.Connect();
                    SetOther(otherHallConnector);
                    otherHallConnector.SetOther(this);
                    Connect();
                    return true;
                }
            }
            return false;
        }
        
        public void SetWall(Wall wall) => _wall = wall;
    }
}