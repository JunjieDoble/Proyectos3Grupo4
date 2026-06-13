using _Code.Scripts.Bases;
using UnityEngine;

namespace _Code.Scripts.Activables
{
    public class Button : Activable
    {
        
        [Header("References")]
        [SerializeField] private GameObject buttonObject;
        [SerializeField] private Material activeMaterial;
        [SerializeField] private Material inactiveMaterial;
        
        private bool _isActive;
        
        private void Awake()
        {
            if (!buttonObject) buttonObject = gameObject;
            buttonObject.GetComponent<MeshRenderer>().material = inactiveMaterial;
        }

        public override void ActivatorUpdate()
        {
            if (_isActive == IsActive()) return;
            _isActive = IsActive();
            PlayStateSound(_isActive);
            if (IsActive()) PressButton();
            else ReleaseButton();
        }

        private void PressButton()
        {
            buttonObject.transform.localPosition += Vector3.down * 0.1f;
            buttonObject.GetComponent<Renderer>().sharedMaterial = activeMaterial;
        }
        
        private void ReleaseButton()
        {
            buttonObject.transform.localPosition -= Vector3.down * 0.1f;
            buttonObject.GetComponent<Renderer>().sharedMaterial = inactiveMaterial;
        }
    }
}