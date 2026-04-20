using UnityEngine;

public class IdleState : StateMachineBehaviour
{
    private float _time;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _time = 0f;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _time += Time.deltaTime;
        if (_time >= 3f)
        {
            animator.SetBool("ReachedPoint", false);
            _time = 0f;
        }
        
    }
}
