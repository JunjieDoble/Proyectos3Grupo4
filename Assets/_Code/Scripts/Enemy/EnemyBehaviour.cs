using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] public Transform[] patrolPoints;
    [SerializeField] public float speed = 2f;
    [SerializeField] private float detectionRadius = 10f;

    private Animator _animator;
    private GameObject _player;
    private bool _isDead = false;

    private void Start()
    {
        _player = GameObject.Find("Player");
        _animator = GetComponent<Animator>();

        // _animator.SetBool("Patrol", true);
    }

    private void Update()
    {
        if (_isDead) return;

        DistanceToPlayer();
    }

    private void DistanceToPlayer()
    {
        Vector3 playerPos = _player.transform.position;
        Vector3 enemyPos = transform.position;

        Vector2 playerXZ = new Vector2(playerPos.x, playerPos.z);
        Vector2 EnemyXZ = new Vector2(enemyPos.x, enemyPos.z);
        float HorizontalDistance = Vector2.Distance(playerXZ, EnemyXZ);
        _animator.SetFloat("DistanceToPlayer", HorizontalDistance);

        Vector3 directionToPlayer = transform.position - _player.transform.position;
        Vector3 directionForward = transform.forward;
        float angleToPlayer = Vector3.Angle(directionForward, directionToPlayer);
        _animator.SetFloat("AngleToPlayer", angleToPlayer);

        /*RaycastHit hit;
        Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius);*/
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            // Dmg or kill the player
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
