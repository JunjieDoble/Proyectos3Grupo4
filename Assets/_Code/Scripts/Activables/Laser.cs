using _Code.Scripts.Bases;
using UnityEngine;

namespace _Code.Scripts.Activables
{
    public class Laser : Activable
    {
        
        [Header("References")]
        [SerializeField] private GameObject laser;
        
        private bool _isActive;

        public override bool IsActive() => activators.Count == 0 || activators.Exists(a => !a.IsActive);

        public override void ActivatorUpdate()
        {
            if (_isActive == IsActive()) return;
            _isActive = IsActive();
            PlayStateSound(_isActive);
            laser.SetActive(IsActive());
        }
    }
}