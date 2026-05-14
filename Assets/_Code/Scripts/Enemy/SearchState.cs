using UnityEngine;
using UnityEngine.AI;

public class SearchState : StateMachineBehaviour
{
    private NavMeshAgent _agent;
    private EnemyBehaviour _enemyBehaviour;
    private Vector3 _lastPlayerPosition;
    private float _stoppingDistance = 1.2f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("Entered Search State");
        _agent = animator.transform.GetComponent<NavMeshAgent>();
        _enemyBehaviour = animator.transform.GetComponent<EnemyBehaviour>();
        _lastPlayerPosition = _enemyBehaviour.GetLastPlayerPosition();

        _agent.isStopped = false;
        _agent.speed = _enemyBehaviour.GetSpeed();
        _agent.stoppingDistance = _stoppingDistance;

        animator.SetBool("Search", true);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(_agent.transform.position, _lastPlayerPosition);
        if (distance <= _stoppingDistance)
        {
            if (!_agent.isStopped)
            {
                _agent.isStopped = true;
                _agent.velocity = Vector3.zero;
            }

            animator.SetBool("Search", false);
        }
        else
        {
            if (_agent.isStopped) _agent.isStopped = false;

            _agent.SetDestination(_lastPlayerPosition);
        }
    }
}
