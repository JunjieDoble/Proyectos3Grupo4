using UnityEngine;

namespace MiniMap
{
    public class ResetRotation : MonoBehaviour
    {
        [SerializeField] private Transform playerTransform;
        void Update()
        {
            transform.localRotation = Quaternion.Euler(-playerTransform.eulerAngles.x, -playerTransform.eulerAngles.y, -playerTransform.eulerAngles.z);
        }
    }
}