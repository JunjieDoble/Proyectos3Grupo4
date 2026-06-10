using System.Collections.Generic;
using _Code.Scripts.Bases;
using UnityEngine;

namespace _Code.Scripts.Activables
{
    public class Charger : Activable
    {
        [SerializeField]
        private List<Material> materials = new();
        private float _charge;
    
        void Start() => UpdateVisual();
    
        public override void ActivatorUpdate()
        {
            var activeActivators = activators.FindAll(a => a.IsActive);
            _charge = (float)activeActivators.Count / activators.Count;
            UpdateVisual();
        }

        void UpdateVisual()
        {
            var materialCount = materials.Count;
            if (materialCount == 0) return;
            var materialIndex = Mathf.FloorToInt(_charge * materialCount);
            if (materialIndex >= materialCount) materialIndex = materialCount - 1;
            GetComponent<Renderer>().material = materials[materialIndex];
        }
    }
}
