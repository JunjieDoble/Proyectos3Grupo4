using UnityEngine;
using UnityEngine.AI;

public class AlertState : StateMachineBehaviour
{
    private NavMeshAgent _agent;
    private EnemyBehaviour _enemyBehaviour;
    private Vector3 _lastAlertPosition;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent = animator.transform.GetComponent<NavMeshAgent>();
        _enemyBehaviour = animator.transform.GetComponent<EnemyBehaviour>();
        _lastAlertPosition = _enemyBehaviour.GetLastAlertPosition();

        _agent.isStopped = false;
        _agent.speed = _enemyBehaviour.GetSpeed();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Vector3.Distance(_agent.transform.position, _lastAlertPosition) < 1.5f)
        {
            animator.SetBool("Alert", false);
        }

        _agent.SetDestination(_lastAlertPosition);
    }
}
