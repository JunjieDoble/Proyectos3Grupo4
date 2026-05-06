using _Code.Scripts.Revamp.Activables;
using _Code.Scripts.Revamp.Bases;
using UnityEngine;

namespace _Code.Scripts.Revamp.Activators.Connectors
{
    public class GeneratorConnector : Connector
    {
        public override bool CheckHit(Collider hit)
        {
            if (hit == null) return false;
            Path path = hit.GetComponentInParent<Path>();
            if (path)
            {
                path.SetGenerator(this);
                path.ActivatorUpdate();
                return true;
            }
            return false;
        }
    }
}