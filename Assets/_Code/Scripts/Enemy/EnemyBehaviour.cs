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
    [SerializeField] private Transform headTransform;

    private Animator _animator;
    private GameObject _player;
    private EnemyInteractor _enemyInteractor;
    private bool _isDead = false;
    private Color _gizmosColor = Color.green;
    private Vector3 _lastAlertPosition;
    private Vector3 _lastPlayerPosition;
    private int _interactableLayer;
    private GameObject _deathZone;
    private Transform _headTransform;
    private float _idleTime = 5f;
    
    public GameObject drop;

    private void OnEnable()
    {
        Player.OnPlayerDied += PlayerDied;
    }

    private void OnDisable()
    {
        Player.OnPlayerDied -= PlayerDied;
    }
    
    public void Interact(IInteractor interactor) => KillEnemy();
    public void SetDeathZoneActive(bool active) => _deathZone?.gameObject.SetActive(active);
    private void PlayerDied() => _animator.SetBool("PlayerDead", true);
    public GameObject[] GetPatrolPoints() => patrolPoints;
    public float GetSpeed() => speed;
    public float GetIdleTime() => _idleTime;
    public void SetIdleTime(float time)
    {
        _idleTime = time;
    }

    public Vector3 GetLastAlertPosition() => _lastAlertPosition;
    public Vector3 GetLastPlayerPosition() => _lastPlayerPosition;
    public void SetLastPlayerPosition(Vector3 pos)  => _lastPlayerPosition = pos;

    private void Start()
    {
        _player = GameObject.Find("Player");
        _animator = GetComponent<Animator>();
        _enemyInteractor = GetComponent<EnemyInteractor>();
        _interactableLayer = LayerMask.NameToLayer("Interactable");
        _deathZone = transform.Find("DeathZone")?.gameObject;
        if (_deathZone == null) Debug.LogWarning("Enemy doesnt have a DeathZone child");
        _deathZone?.SetActive(false);
        _headTransform = headTransform ?? transform;
    }

    private void Update()
    {
        if (_isDead) return;

        DistanceAndVisionToPlayer();
    }

    private void DistanceAndVisionToPlayer()
    {
        float distanceToPlayer = GetDistanceToPlayer();
        _animator.SetFloat("DistanceToPlayer", distanceToPlayer);

        float angleToPlayer = GetAngleToPlayer();

        RaycastHit hit;
        Vector3 directionToPlayer = _player.transform.position - _headTransform.position;
        Ray ray = new Ray(_headTransform.position, directionToPlayer);
        Debug.DrawLine(_headTransform.position, _headTransform.position + directionToPlayer.normalized * detectionRadius, Color.red);
        Physics.Raycast(ray, out hit, detectionRadius);
        if (hit.collider != null && hit.collider.gameObject == _player && angleToPlayer <= detectionAngle/2f && distanceToPlayer <= detectionRadius)
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
        float distanceToAlert = Vector3.Distance(_headTransform.position, alertPosition);
        if (distanceToAlert <= alertRadius && !_isDead)
        {
            _lastAlertPosition = alertPosition;
            _animator.SetBool("Alert", true);
        }
    }
    
    public void InteractWithInteractable(InteractPoint interactPoint)
    {
        IInteractable interactable = interactPoint.GetInteractable();
        if (interactable != null)
        {
            _enemyInteractor.AssignInteractable(interactable);
            _enemyInteractor.OnInteract();
        }
    }
    
    private float GetDistanceToPlayer()
    {
        if (_player == null) return 0;
        Vector3 playerPos = _player.transform.position;
        Vector3 enemyPos = _headTransform.position;
        return Vector3.Distance(playerPos, enemyPos);
    }

    private float GetAngleToPlayer()
    {
        if (_player == null) return 0;
        Vector3 directionToPlayer = _player.transform.position - transform.position;
        Vector3 directionForward = transform.forward;
        return Vector3.Angle(directionForward, directionToPlayer);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = _gizmosColor;
        Gizmos.DrawWireSphere(headTransform.position, detectionRadius);
        Gizmos.DrawRay(headTransform.position, headTransform.forward * detectionRadius);
    }

}
