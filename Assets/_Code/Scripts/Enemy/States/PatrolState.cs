using UnityEngine;
using UnityEngine.AI;

namespace _Code.Scripts.Enemy.States
{
    public class PatrolState : StateMachineBehaviour
    {
        private static readonly int CurrentPointIndex = Animator.StringToHash("CurrentPointIndex");
        private static readonly int ReachedPoint = Animator.StringToHash("ReachedPoint");
        
        private NavMeshAgent _agent;
        private EnemyBehaviour _enemyBehaviour;
        private GameObject[] _patrolPoints;
        private Vector3 _patrolPosition;
        private int _currentPointIndex;
        private int _patrolPointsCount;
        private bool _pointReached;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _agent = animator.transform.GetComponent<NavMeshAgent>();
            _enemyBehaviour = animator.transform.GetComponent<EnemyBehaviour>();
            _patrolPoints = _enemyBehaviour.GetPatrolPoints();

            _currentPointIndex = animator.GetInteger(CurrentPointIndex);
            _patrolPointsCount = _patrolPoints.Length;
            GetNextPatrolPoint();

            _agent.isStopped = false;
            _pointReached = false;
            _agent.speed = _enemyBehaviour.GetSpeed();
            _agent.stoppingDistance = 1f;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (Vector3.Distance(_agent.transform.position, _patrolPosition) < 1.5f)
            {
                if (_pointReached) return;
                CheckForInteraction();
                _pointReached = true;
                animator.SetInteger(CurrentPointIndex, _currentPointIndex);
                animator.SetBool(ReachedPoint, true);
            }
            else
            {
                animator.SetBool(ReachedPoint, false);
            }
        }

        private void CheckForInteraction()
        {
            _patrolPoints[_currentPointIndex].TryGetComponent(out InteractPoint interactPoint);
            if (interactPoint)
            {
                _enemyBehaviour.InteractWithInteractable(interactPoint);
                if (interactPoint.GetOverrideIdleTime())
                    _enemyBehaviour.SetIdleTime(interactPoint.IdleTime);
            }
        }

        private void GetNextPatrolPoint()
        {
            if (_patrolPointsCount == 0) return;

            for (int i = 0; i < _patrolPointsCount; i++)
            {
                int nextPointIndex = (_currentPointIndex + 1 + i) % _patrolPointsCount;
                Vector3 nextPatrolPoint = _patrolPoints[nextPointIndex].transform.position;
                
                NavMeshPath path = new NavMeshPath();

                if (_agent.CalculatePath(nextPatrolPoint, path) && path.status == NavMeshPathStatus.PathComplete)
                {
                    _currentPointIndex = nextPointIndex;
                    _agent.SetDestination(nextPatrolPoint);
                    _patrolPosition = nextPatrolPoint;
                    return;
                }
            }
        }
    }
}
