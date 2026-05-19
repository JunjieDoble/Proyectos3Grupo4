using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PatrolState : StateMachineBehaviour
{
    private NavMeshAgent _agent;
    private EnemyBehaviour _enemyBehaviour;
    private GameObject[] _patrolPoints;
    private Vector3 _patrolPosition;
    private int _currentPointIndex;
    private int _patrolPointsCount;
    private bool _pointReached;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent = animator.transform.GetComponent<NavMeshAgent>();
        _enemyBehaviour = animator.transform.GetComponent<EnemyBehaviour>();
        _patrolPoints = _enemyBehaviour.GetPatrolPoints();

        _currentPointIndex = animator.GetInteger("CurrentPointIndex");
        _patrolPosition = _patrolPoints[_currentPointIndex].transform.position;
        _patrolPointsCount = _patrolPoints.Length;

        _agent.isStopped = false;
        _pointReached = false;
        _agent.speed = _enemyBehaviour.GetSpeed();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(_agent.transform.position, _patrolPosition) < 1.5f)
        {
            if (_pointReached) return;
            _patrolPoints[_currentPointIndex].TryGetComponent(out InteractPoint interactPoint);
            if (interactPoint)
            {
                _enemyBehaviour.InteractWithInteractable(interactPoint);
                if (interactPoint.GetOverrideIdleTime())
                    _enemyBehaviour.SetIdleTime(interactPoint.IdleTime);
            }
            _pointReached = true;

            if (_currentPointIndex >= _patrolPointsCount - 1)
            {
                _currentPointIndex = 0;
                
            }
            else
            {
                _currentPointIndex = _currentPointIndex + 1;
            }
            animator.SetInteger("CurrentPointIndex", _currentPointIndex);
            animator.SetBool("ReachedPoint", true);
        }

        _patrolPosition = _patrolPoints[_currentPointIndex].transform.position;
        _agent.SetDestination(_patrolPosition);
    }
}
