using _Code.Scripts.Bases;
using UnityEngine;

namespace _Code.Scripts.Activables
{
    public class Laser : Activable
    {
        
        [Header("References")]
        [SerializeField] private GameObject laser;

        public override bool IsActive() => activators.Count == 0 || activators.TrueForAll(a => !a.IsActive);

        public override void ActivatorUpdate()
        {
            laser.SetActive(IsActive());
        }
    }
}