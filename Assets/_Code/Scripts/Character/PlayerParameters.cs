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
    }
}
