using UnityEngine;
using UnityEngine.AI;

public class IdleState : StateMachineBehaviour
{
    private NavMeshAgent _agent;
    private float _time;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent = animator.transform.GetComponent<NavMeshAgent>();
        _time = 0f;
        _agent.isStopped = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _time += Time.deltaTime;
        if (_time >= 5f)
        {
            animator.SetBool("ReachedPoint", false);
            _time = 0f;
        }
        
    }
}
