using UnityEngine;

namespace _Code.Scripts.Character
{
    [CreateAssetMenu(fileName = "PlayerParameters", menuName = "Parameters/Player")]
    public class PlayerParameters : ScriptableObject
    {
        [Header("Speeds")]
        public float walkSpeed = 5f;
        public float runSpeed = 10f;   
        public float crouchSpeed = 2.5f;        
        public float jumpSpeed = 8f;     
        
        [Header("Heights")]
        public float walkHeight = 2f;       
        public float crouchHeight = 1f;
        
        [Header("Control")]
        public float airControl = 0.5f;
        public float jumpStopMultiplier = 0.5f;
        
        [Header("Camera")]
        public float mouseSensitivity = 1f;
        public float minPitch = -89f;
        public float maxPitch = 89f;
    }
}
