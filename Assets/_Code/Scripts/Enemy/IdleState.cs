using UnityEngine;
using UnityEngine.AI;

public class IdleState : StateMachineBehaviour
{
    private NavMeshAgent _agent;
    private EnemyBehaviour _enemyBehaviour;
    private float _time;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent = animator.transform.GetComponent<NavMeshAgent>();
        _enemyBehaviour = animator.transform.GetComponent<EnemyBehaviour>();
        _time = 0f;
        _agent.isStopped = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _time += Time.deltaTime;
        if (_time >= _enemyBehaviour.GetIdleTime())
        {
            animator.SetBool("ReachedPoint", false);
            _time = 0f;
        }
        
    }
}
