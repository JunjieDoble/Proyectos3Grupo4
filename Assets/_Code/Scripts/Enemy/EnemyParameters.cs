using UnityEngine;

namespace _Code.Scripts.Enemy
{
    [CreateAssetMenu(fileName = "EnemyParameters", menuName = "Parameters/Enemy")]
    public class EnemyParameters : ScriptableObject
    {
        [Header("Movement")]
        public float speed = 2f;
        public float chaseSpeed = 3.5f;   
        public float rotationSpeed = 5f;
        
        [Header("Detection")]
        public float detectionRadius = 10f;
        public float detectionAngle = 90f;
        public LayerMask obstacleMask;
        public float alertTimeoutDuration = 5f;
        
        [Header("Idle")]
        public float idleTime = 5f;

        [Header("Search")]
        public float searchRadius = 5f;
        public int maxSearchPoints = 3;
        public float waitTimePerSearchPoint = 2f;
    }
}
