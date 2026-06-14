using UnityEngine;

namespace _Code.Scripts.Enemy
{
    public class EnemyAnimatorFields
    {
        public static readonly int SeePlayer = Animator.StringToHash("SeePlayer");
        public static readonly int ToPlayer = Animator.StringToHash("DistanceToPlayer");
        public static readonly int Alert = Animator.StringToHash("Alert");
        public static readonly int Search = Animator.StringToHash("Search");
        public static readonly int PlayerDead = Animator.StringToHash("PlayerDead");
        public static readonly int Speed = Animator.StringToHash("Speed");
        public static readonly int ReachedPoint = Animator.StringToHash("ReachedPoint");
        public static readonly int CurrentPointIndex = Animator.StringToHash("CurrentPointIndex");
   }
}