using UnityEngine;

namespace _Code.Scripts.Character
{
    [CreateAssetMenu(fileName = "PlayerParameters", menuName = "Parameters/Player")]
    public class PlayerParameters : ScriptableObject
    {
        [Header("Movement")]
        public float walkSpeed = 5f;
        public float runSpeed = 10f;   
        public float crouchSpeed = 2.5f;        
        public float jumpHeight = 2f;
        public float jumpDuration = 0.5f;
        public float deathHeight = -100f;
        
        [Header("Control")]
        public float airControl = 0.5f;
        public float jumpStopMultiplier = 0.5f;        
        
        [Header("Gravity")]
        public float fallMultiplier = 2.5f;
        
        [Header("Coyote Time")]
        public float coyoteTime = 0.1f;
        
        [Header("Heights")]
        public float walkHeight = 2f;       
        public float crouchHeight = 1f;
        
        [Header("Camera")]
        public float mouseSensitivity = 1f;
        public float minPitch = -89f;
        public float maxPitch = 89f;
        
         [Header("Interactor")]
         public float interactionDistance = 3f;
         public float throwCheckDistance = 1f;

         [Header("Audio")]
         public float airNoiseRadius = 3f;
         public float crouchNoiseRadius = 2f;
         public float runNoiseRadius = 8f;
         public float walkNoiseRadius = 4f;
    }
}
