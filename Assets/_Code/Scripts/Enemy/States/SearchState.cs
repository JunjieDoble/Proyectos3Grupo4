using UnityEngine;
using UnityEngine.AI;

namespace _Code.Scripts.Enemy.States
{
    public class SearchState : StateMachineBehaviour
    {
        private static readonly int Search = Animator.StringToHash("Search");
        
        private NavMeshAgent _agent;
        private EnemyBehaviour _enemyBehaviour;
        private Vector3 _lastPlayerPosition;
        private float _stoppingDistance = 1.5f;
        private float _searchTimer;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _agent = animator.transform.GetComponent<NavMeshAgent>();
            _enemyBehaviour = animator.transform.GetComponent<EnemyBehaviour>();
            _lastPlayerPosition = _enemyBehaviour.GetLastPlayerPosition();

            _agent.isStopped = false;
            _agent.speed = _enemyBehaviour.GetSpeed();
            _agent.stoppingDistance = _stoppingDistance;
            _agent.SetDestination(_lastPlayerPosition);
            _searchTimer = 0f;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (_agent.isStopped || _agent.velocity.magnitude < 0.1f)
                _searchTimer += Time.deltaTime;
            
            var distance = Vector3.Distance(_agent.transform.position, _lastPlayerPosition);

            if (!(distance <= _stoppingDistance) && !(_searchTimer >= _enemyBehaviour.GetAlertTimeout())) return;
            _agent.isStopped = true;
            animator.SetBool(Search, false);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(Search, false);
        }
    }
}
