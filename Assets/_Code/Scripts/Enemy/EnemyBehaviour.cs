using _Code.Scripts.Character;
using Interactions;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour, IEnemy, IInteractable
{
    [Header("Parameters")]
    [SerializeField] private GameObject[] patrolPoints;
    [SerializeField] private float speed = 2f;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float alertRadius = 15f;
    [SerializeField] private float detectionAngle = 180f;
    [SerializeField] private LayerMask playerLayerMask;

    private Animator _animator;
    private GameObject _player;
    private EnemyInteractor _enemyInteractor;
    private bool _isDead = false;
    private Color _gizmosColor = Color.green;
    private Vector3 _lastAlertPosition;
    private Vector3 _lastPlayerPosition;
    private int _interactableLayer;
    private GameObject _deathZone;

    public GameObject drop;

    private void OnEnable()
    {
        Player.OnPlayerDied += PlayerDied;
    }

    private void OnDisable()
    {
        Player.OnPlayerDied -= PlayerDied;
    }

    private void Start()
    {
        _player = GameObject.Find("Player");
        _animator = GetComponent<Animator>();
        _enemyInteractor = GetComponent<EnemyInteractor>();
        _interactableLayer = LayerMask.NameToLayer("Interactable");
        _deathZone = transform.Find("DeathZone")?.gameObject;
        if (_deathZone == null) Debug.LogWarning("Enemy doesnt have a DeathZone child");
        _deathZone?.SetActive(false);
    }

    private void Update()
    {
        if (_isDead) return;

        DistanceAndVisionToPlayer();
    }

    private void DistanceAndVisionToPlayer()
    {
        float HorizontalDistance = GetDistanceToPlayer();
        _animator.SetFloat("DistanceToPlayer", HorizontalDistance);

        float angleToPlayer = GetAngleToPlayer();

        RaycastHit hit;
        Vector3 directionToPlayer = _player.transform.position - transform.position;
        Physics.Raycast(transform.position, directionToPlayer, out hit, detectionRadius);
        if (hit.collider != null && hit.collider.gameObject == _player && angleToPlayer <= detectionAngle/2f && HorizontalDistance <= detectionRadius)
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

    public void KillEnemy()
    {
        float angleToPlayer = GetAngleToPlayer();

        // Detect if the player is behind the enemy when killing it
        if (angleToPlayer >= detectionAngle /2f && !_isDead)
        {
            Debug.Log("Enemy killed from behind");
            _isDead = true;
            _animator.SetTrigger("Die");

            Destroy(gameObject);
            if (drop)
                Instantiate(drop, transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("Cannot kill the enemy from the front");
        }
    }

    public void AlertEnemy(Vector3 alertPosition)
    {
        float distanceToAlert = Vector3.Distance(transform.position, alertPosition);
        if (distanceToAlert <= alertRadius && !_isDead)
        {
            _lastAlertPosition = alertPosition;
            _animator.SetBool("Alert", true);
        }
    }

    public void Interact(IInteractor interactor)
    {
        KillEnemy();
    }

    public void InteractWithInteractable(GameObject inte)
    {
        if (inte.TryGetComponent(out InteractPoint interactPoint))
        {
            IInteractable interactable = interactPoint.GetInteractable();
            if (interactable != null)
            {
                _enemyInteractor.AssignInteractable(interactable);
                _enemyInteractor.OnInteract();
            }
        }
    }

    public void SetDeathZoneActive(bool active)
    {
        _deathZone?.gameObject.SetActive(active);
    }

    private void PlayerDied()
    {
        _animator.SetBool("PlayerDead", true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.DrawRay(transform.position, transform.forward * detectionRadius);
    }

    private float GetDistanceToPlayer()
    {
        Vector3 playerPos = _player.transform.position;
        Vector3 enemyPos = transform.position;

        Vector2 playerXZ = new Vector2(playerPos.x, playerPos.z);
        Vector2 EnemyXZ = new Vector2(enemyPos.x, enemyPos.z);
        return Vector2.Distance(playerXZ, EnemyXZ);
    }

    private float GetAngleToPlayer()
    {
        Vector3 directionToPlayer = _player.transform.position - transform.position;
        Vector3 directionForward = transform.forward;
        return Vector3.Angle(directionForward, directionToPlayer);
    }

    public GameObject[] GetPatrolPoints()
    {
        return patrolPoints;
    }

    public float GetSpeed()
    {
            return speed;
    }

    public Vector3 GetLastAlertPosition()
    {
        return _lastAlertPosition;
    }

    public Vector3 GetLastPlayerPosition()
    {
        return _lastPlayerPosition;
    }

    public void SetLastPlayerPosition(Vector3 pos)
    {
        _lastPlayerPosition = pos;
    }
}
