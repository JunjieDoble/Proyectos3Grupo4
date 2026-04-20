using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PatrolState : StateMachineBehaviour
{
    private NavMeshAgent _agent;
    private EnemyBehaviour _enemyBehaviour;
    private Transform[] _patrolPoints;
    private Vector3 _patrolPosition;
    private int _currentPointIndex;
    private int _patrolPointsCount;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent = animator.transform.GetComponent<NavMeshAgent>();
        _enemyBehaviour = animator.transform.GetComponent<EnemyBehaviour>();
        _patrolPoints = _enemyBehaviour.patrolPoints;
        _currentPointIndex = 0;
        _patrolPosition = _patrolPoints[_currentPointIndex].transform.position;
        _patrolPointsCount = _patrolPoints.Length;

        _agent.isStopped = false;
        _agent.speed = _enemyBehaviour.speed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(_agent.transform.position, _patrolPosition) < 1.2f)
        {
            if (_currentPointIndex >= _patrolPointsCount - 1)
            {
                _currentPointIndex = 0;
            }
            else
            {
                _currentPointIndex = _currentPointIndex + 1;
            }
            animator.SetBool("ReachedPoint", true);
        }
        _patrolPosition = _patrolPoints[_currentPointIndex].transform.position;
        _agent.SetDestination(_patrolPosition);
    }
}
