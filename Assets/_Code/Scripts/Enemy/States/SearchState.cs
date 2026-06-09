using UnityEngine;
using UnityEngine.AI;

namespace _Code.Scripts.Enemy.States
{
    public class SearchState : StateMachineBehaviour
    {
        private static readonly int Search = Animator.StringToHash("Search");
        
        private NavMeshAgent _agent;
        private EnemyBehaviour _enemyBehaviour;
        private Vector3 _searchCenterPosition;
        
        private float _searchRadius;
        private int _maxSearchPoints;
        private float _waitTimePerSearchPoint;
        
        private int _pointsCheckedCount;
        private float _actionTimer;
        private bool _isWaiting;

        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _agent = animator.transform.GetComponent<NavMeshAgent>();
            _enemyBehaviour = animator.transform.GetComponent<EnemyBehaviour>();
            
            _searchRadius = _enemyBehaviour.GetEnemyParameters.searchRadius;
            _maxSearchPoints = _enemyBehaviour.GetEnemyParameters.maxSearchPoints;
            _waitTimePerSearchPoint = _enemyBehaviour.GetEnemyParameters.waitTimePerSearchPoint;
            
            _searchCenterPosition = _enemyBehaviour.GetLastAlertPosition();
            
            _agent.isStopped = false;
            _agent.speed = _enemyBehaviour.GetSpeed();
            _agent.stoppingDistance = 1f;
            _agent.SetDestination(_searchCenterPosition);
            
            _pointsCheckedCount = 0;
            _actionTimer = 0f;
            _isWaiting = false;
        }

        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            _actionTimer += Time.deltaTime;
            if (_actionTimer >= _enemyBehaviour.GetAlertTimeout() + _waitTimePerSearchPoint && !_isWaiting)
            {
                ExitSearchState(animator);
                return;
            }

            if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
            {
                if (!_isWaiting)
                {
                    _isWaiting = true;
                    _actionTimer = 0f;
                    _pointsCheckedCount++;
                }
                else
                {
                    if (_actionTimer >= _waitTimePerSearchPoint)
                    {
                        _isWaiting = false;

                        if (_pointsCheckedCount >= _maxSearchPoints)
                        {
                            ExitSearchState(animator);
                        }
                        else
                        {
                            Vector3 nextSearchPoint = GetRandomPointNear(_agent.transform.position, _searchRadius);
                            _agent.isStopped = false;
                            _agent.SetDestination(nextSearchPoint);
                            _actionTimer = 0f;
                        }
                    }
                }
            }
        }

        private Vector3 GetRandomPointNear(Vector3 center, float radius)
        {
            Debug.Log("GetRandomPointNear");
            for (int i = 0; i < 5; i++)
            {
                Vector3 randomDirection = Random.insideUnitSphere * radius;
                randomDirection += center;
                randomDirection.y = center.y;

                if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, 1.0f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }
    
            return center;
        }

        private void ExitSearchState(Animator animator)
        {
            if (_agent != null) _agent.isStopped = true;
            animator.SetBool(Search, false);
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(Search, false);
        }
    }
}
