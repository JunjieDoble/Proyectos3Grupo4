using _Code.Scripts.Activables;
using _Code.Scripts.Bases;
using _Code.Scripts.Rooms;
using UnityEngine;

namespace _Code.Scripts.Activators.Connectors
{
    public class HallConnector : Connector
    {
        
        private Wall _wall;
        
        protected override void Awake()
        {
            activables.Add(GetComponentInChildren<Door>());
            if (activables == null) Debug.LogWarning("HallConnector does not have a Door in its children", this);
            else activables.ForEach(a => a?.AddActivator(this));
        }

        protected override bool CheckHit(Collider hit)
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