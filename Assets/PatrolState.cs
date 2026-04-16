using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class PatrolState : StateMachineBehaviour
{
    private NavMeshAgent _agent;
    private AIBehaviour _aiBehavior;
    private Transform[] _patrolPoints;
    private Vector3 _patrolPosition;
    private int _currentPointIndex;
    private int _patrolPointsCount;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent = animator.transform.parent.GetComponent<NavMeshAgent>();
        _aiBehavior = animator.transform.GetComponent<AIBehaviour>();
        _patrolPoints = _aiBehavior.patrolPoints;
        _currentPointIndex = 0;
        _patrolPosition = _patrolPoints[_currentPointIndex].transform.position;
        _patrolPointsCount = _patrolPoints.Length;

        _agent.isStopped = false;
        _agent.speed = 2f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
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
        }
        _patrolPosition = _patrolPoints[_currentPointIndex].transform.position;
        _agent.SetDestination(_patrolPosition);
    }
}
