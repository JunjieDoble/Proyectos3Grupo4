using UnityEngine;
using UnityEngine.AI;

namespace _Code.Scripts.Enemy.States
{
    public class AlertState : StateMachineBehaviour
    {
        private NavMeshAgent _agent;
        private EnemyBehaviour _enemyBehaviour;
        private Vector3 _lastAlertPosition;
        private float _timer;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _agent = animator.transform.GetComponent<NavMeshAgent>();
            _enemyBehaviour = animator.transform.GetComponent<EnemyBehaviour>();
            _lastAlertPosition = _enemyBehaviour.GetLastAlertPosition();

            _agent.isStopped = false;
            _agent.speed = _enemyBehaviour.GetSpeed();
            _agent.SetDestination(_lastAlertPosition);
            _timer = 0;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _timer += Time.deltaTime;
            
            if (Vector3.Distance(_agent.transform.position, _lastAlertPosition) < 1.5f || _timer >= _enemyBehaviour.GetAlertTimeout())
            {
                animator.SetBool("Alert", false);
                animator.SetBool("ReachedPoint", true);
            }
        }
    }
}
