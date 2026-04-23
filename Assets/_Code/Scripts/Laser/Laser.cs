using UnityEngine;

namespace Laser
{
    public class Laser : MonoBehaviour
    {

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                //Player dead
                Debug.Log("Player dead by laser");
            }
        }
    }
    
    //Això de sota és utilitzant el lineRenderer. Donava problemes en rotar les sales (el linerenderer es quedava quiet) així que de moment és un objecte sol amb trigger
    //Es podria posar al update mentre roti la sala, però si tot lo dels lasers no ha de ser gaire més complex, no crec que valgui la pena complicar-ho més.
    
    /*
    [RequireComponent(typeof(LineRenderer))]
    [RequireComponent(typeof(BoxCollider))]
    public class Laser : MonoBehaviour
    {
        private BoxCollider laserCollider;

        void Start()
        {
            laserCollider = GetComponent<BoxCollider>();
            InitializeLaser();
        }

        void InitializeLaser()
        {
            Vector3 hitPoint;
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hitInfo, 100f))
            {
                hitPoint = hitInfo.point;
            }
            else
            {
                Debug.LogWarning("Laser did not hit anything, extending to max distance.");
                hitPoint = transform.position + transform.forward * 100f;
            }
            var lineRenderer = GetComponent<LineRenderer>();
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, hitPoint);

            Vector3 direction = hitPoint - transform.position;
            float length = direction.magnitude;

            laserCollider.transform.position = transform.position + direction * 0.5f;
            laserCollider.transform.rotation = Quaternion.LookRotation(direction);

            laserCollider.size = new Vector3(laserCollider.size.x, laserCollider.size.y, length);
            laserCollider.center = new Vector3(0f, 0f, 0f);
        }
        */

        
    
}

