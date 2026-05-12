using _Code.Scripts.Activables;
using _Code.Scripts.Bases;
using UnityEngine;

namespace _Code.Scripts.Activators.Connectors
{
    public class GeneratorConnector : Connector
    {
        public override bool IsActive => true;

        protected override bool CheckHit(Collider hit)
        {
            if (hit == null) return false;
            Path path = hit.GetComponentInParent<Path>();
            if (path)
            {
                path.AddActivator(this);
                activable = path;
                path.ActivatorUpdate();
                return true;
            }
            return false;
        }
    }
}