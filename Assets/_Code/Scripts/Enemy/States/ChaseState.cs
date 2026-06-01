using UnityEngine;
using UnityEngine.AI;

namespace _Code.Scripts.Enemy.States
{
    public class ChaseState : StateMachineBehaviour
    {
        private static readonly int Alert = Animator.StringToHash("Alert");
        private static readonly int Search = Animator.StringToHash("Search");
        private NavMeshAgent _agent;
        private EnemyBehaviour _enemyBehaviour;
        private float _stoppingDistance = 1.5f;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _agent = animator.transform.GetComponent<NavMeshAgent>();
            _enemyBehaviour = animator.transform.GetComponent<EnemyBehaviour>();
            _enemyBehaviour.SetDeathZoneActive(true);

            _agent.isStopped = false;
            _agent.speed = _enemyBehaviour.GetChaseSpeed();
            _agent.stoppingDistance = _stoppingDistance;

            animator.SetBool(Alert, false);
            animator.SetBool(Search, false);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float distance = Vector3.Distance(_agent.transform.position, _enemyBehaviour.GetLastPlayerPosition());
            
            if (distance <= _stoppingDistance)
            {
                _agent.isStopped = true;
                _agent.velocity = Vector3.zero;
            }
            else
            {
                _agent.isStopped = false;
                _agent.SetDestination(_enemyBehaviour.GetLastPlayerPosition());
            }
            _enemyBehaviour.RotateTowards(_enemyBehaviour.GetLastPlayerPosition());
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _enemyBehaviour.SetDeathZoneActive(false);
            animator.SetBool(Search, true);
        }
    }
}
