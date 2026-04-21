using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    [Header("Parameters")]
    [SerializeField] public Transform[] patrolPoints;
    [SerializeField] public float speed = 2f;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float detectionAngle = 180f;

    private Animator _animator;
    private GameObject _player;
    private bool _isDead = false;
    private Color _gizmosColor = Color.green;

    private void Start()
    {
        _player = GameObject.Find("Player");
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_isDead) return;

        DistanceAndVisionToPlayer();
    }

    private void DistanceAndVisionToPlayer()
    {
        Vector3 playerPos = _player.transform.position;
        Vector3 enemyPos = transform.position;

        Vector2 playerXZ = new Vector2(playerPos.x, playerPos.z);
        Vector2 EnemyXZ = new Vector2(enemyPos.x, enemyPos.z);
        float HorizontalDistance = Vector2.Distance(playerXZ, EnemyXZ);
        _animator.SetFloat("DistanceToPlayer", HorizontalDistance);

        Vector3 directionToPlayer = _player.transform.position - transform.position;
        Vector3 directionForward = transform.forward;
        float angleToPlayer = Vector3.Angle(directionForward, directionToPlayer);

        if (HorizontalDistance <= detectionRadius && angleToPlayer <= detectionAngle / 2f)
        {
            _animator.SetBool("SeePlayer", true);
            _gizmosColor = Color.red;
        }
        else
        {
            _animator.SetBool("SeePlayer", false);
            _gizmosColor = Color.green;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            // Dmg or kill the player
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.DrawRay(transform.position, transform.forward * detectionRadius);
    }
}
