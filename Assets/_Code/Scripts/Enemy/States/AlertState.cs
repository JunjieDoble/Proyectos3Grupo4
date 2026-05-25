using UnityEngine;
using UnityEngine.AI;

namespace _Code.Scripts.Enemy.States
{
    public class AlertState : StateMachineBehaviour
    {
        private static readonly int Alert = Animator.StringToHash("Alert");
        private static readonly int Search = Animator.StringToHash("Search");
        private NavMeshAgent _agent;
        private EnemyBehaviour _enemyBehaviour;
        private Vector3 _lastAlertPosition;
        private float _timer;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _agent = animator.transform.GetComponent<NavMeshAgent>();
            _enemyBehaviour = animator.transform.GetComponent<EnemyBehaviour>();

            _agent.isStopped = false;
            _agent.speed = _enemyBehaviour.GetSpeed();
            _timer = 0;
            
            _lastAlertPosition = _enemyBehaviour.GetLastAlertPosition();
            _agent.SetDestination(_lastAlertPosition);
            animator.SetBool(Search, false);
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Vector3 currentAlertPos = _enemyBehaviour.GetLastAlertPosition();
            if (Vector3.Distance(_lastAlertPosition, currentAlertPos) > 1f)
            {
                _lastAlertPosition = currentAlertPos;
                _agent.SetDestination(_lastAlertPosition);
            }

            if (_agent.isStopped || _agent.velocity.magnitude < 0.1f)
            {
                _timer += Time.deltaTime;
            } else _timer = 0;

            
            if (Vector3.Distance(_agent.transform.position, _lastAlertPosition) < 1.5f || _timer >= _enemyBehaviour.GetAlertTimeout())
            {
                animator.SetBool(Alert, false);
                animator.SetBool(Search, true);
            }
        }
    }
}
