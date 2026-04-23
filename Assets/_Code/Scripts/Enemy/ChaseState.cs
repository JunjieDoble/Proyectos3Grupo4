using UnityEngine;
using UnityEngine.AI;

public class ChaseState : StateMachineBehaviour
{
    private NavMeshAgent _agent;
    private EnemyBehaviour _enemyBehaviour;
    private GameObject _player;
    private float _stoppingDistance = 1.2f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _agent = animator.transform.GetComponent<NavMeshAgent>();
        _enemyBehaviour = animator.transform.GetComponent<EnemyBehaviour>();
        _player = GameObject.Find("Player");

        _agent.isStopped = false;
        _agent.speed = _enemyBehaviour.GetSpeed();
        _agent.stoppingDistance = _stoppingDistance;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float distance = Vector3.Distance(_agent.transform.position, _player.transform.position);
        if (distance <= _stoppingDistance)
        {
            if (!_agent.isStopped)
            {
                _agent.isStopped = true;
                _agent.velocity = Vector3.zero;
            }
        }
        else
        {
            if (_agent.isStopped) _agent.isStopped = false;

            _agent.SetDestination(_player.transform.position);
        }
    }
}
