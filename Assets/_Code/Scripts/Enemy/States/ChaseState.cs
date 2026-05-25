using UnityEngine;
using UnityEngine.AI;

namespace _Code.Scripts.Enemy.States
{
    public class ChaseState : StateMachineBehaviour
    {
        private NavMeshAgent _agent;
        private EnemyBehaviour _enemyBehaviour;
        private GameObject _player;
        private float _stoppingDistance = 1.5f;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _agent = animator.transform.GetComponent<NavMeshAgent>();
            _player = GameObject.Find("Player");
            _enemyBehaviour = animator.transform.GetComponent<EnemyBehaviour>();
            _enemyBehaviour.SetDeathZoneActive(true);

            _agent.isStopped = false;
            _agent.speed = _enemyBehaviour.GetChaseSpeed();
            _agent.stoppingDistance = _stoppingDistance;

            animator.SetBool("Alert", false);
            animator.SetBool("Search", false);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_player == null) return;
            
            float distance = Vector3.Distance(_agent.transform.position, _player.transform.position);
            
            if (distance <= _stoppingDistance)
            {
                _agent.isStopped = true;
                _agent.velocity = Vector3.zero;
                _enemyBehaviour.RotateTowards(_player.transform.position);
            }
            else
            {
                _agent.isStopped = false;
                _agent.SetDestination(_player.transform.position);
            }

            _enemyBehaviour.SetLastPlayerPosition(_player.transform.position);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _enemyBehaviour.SetDeathZoneActive(false);
            animator.SetBool("Search", true);
        }
    }
}
