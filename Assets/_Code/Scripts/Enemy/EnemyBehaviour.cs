using _Code.Scripts.Character;
using _Code.Scripts.Enemy;
using Interactions;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour, IEnemy, IInteractable
{
    [Header("Enemy Parameters")]
    [SerializeField] private EnemyParameters enemyParameters;
    [SerializeField] private Transform headTransform;
    
    [Header("Patrol Settings")]
    [SerializeField] private GameObject[] patrolPoints;
    
    private Animator _animator;
    private Player _player;
    private NavMeshAgent _agent;
    private EnemyInteractor _enemyInteractor;
    private bool _isDead;
    private Color _gizmosColor = Color.green;
    private Vector3 _lastAlertPosition;
    private Vector3 _lastPlayerPosition;
    private float _idleTime;
    
    private int _interactableLayer;
    private GameObject _deathZone;
    private Transform _headTransform;
    private Light _FOVLight;
    
    public GameObject drop;

    private void OnEnable() => Player.OnPlayerDied += PlayerDied;
    private void OnDisable() => Player.OnPlayerDied -= PlayerDied;
    private void PlayerDied() => _animator.SetBool("PlayerDead", true);
    
    public void Interact(IInteractor interactor) => KillEnemy();
    public void SetDeathZoneActive(bool active) => _deathZone?.gameObject.SetActive(active);

    public GameObject[] GetPatrolPoints() => patrolPoints;
    public float GetSpeed() => enemyParameters.speed;
    public float GetChaseSpeed() => enemyParameters.chaseSpeed;
    public float GetIdleTime() => _idleTime;
    public float GetAlertTimeout() => enemyParameters.alertTimeoutDuration;
    public EnemyParameters GetEnemyParameters => enemyParameters;

    public void SetIdleTime(float time) => _idleTime = time;
    public Vector3 GetLastAlertPosition() => _lastAlertPosition;
    public Vector3 GetLastPlayerPosition() => _lastPlayerPosition;
    public void SetLastPlayerPosition(Vector3 pos)  => _lastPlayerPosition = pos;

    private void Start()
    {
        _player = GameObject.Find("Player").GetComponent<Player>();
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyInteractor = GetComponent<EnemyInteractor>();
        _interactableLayer = LayerMask.NameToLayer("Interactable");
        _deathZone = transform.Find("DeathZone")?.gameObject;
        if (_deathZone == null) Debug.LogWarning("Enemy doesnt have a DeathZone child");
        _deathZone?.SetActive(false);
        _headTransform = headTransform ?? transform;
        _idleTime = enemyParameters.idleTime;
        _FOVLight = GetComponentInChildren<Light>();
    }

    private void Update()
    {
        if (_isDead) return;
        DistanceAndVisionToPlayer();
    }

    private bool PlayerAvailable()
    {
        if (!_player) return false;
        return !_player.IsDead();
    }
    
    private float DistanceToPlayer() => Vector3.Distance(_player.transform.position, _headTransform.position);

    private void DistanceAndVisionToPlayer()
    {
        if (!PlayerAvailable()) return;
        
        _animator.SetFloat("DistanceToPlayer", DistanceToPlayer());

        Vector3 directionToPlayer = (_player.transform.position - _headTransform.position).normalized;
        float angleToPlayer = Vector3.Angle(_headTransform.forward, directionToPlayer);

        if (DistanceToPlayer() <= enemyParameters.detectionRadius && angleToPlayer <= enemyParameters.detectionAngle / 2f)
        {
            if (!Physics.Linecast(_headTransform.position, _player.transform.position, enemyParameters.obstacleMask))
            {
                _animator.SetBool("SeePlayer", true);
                _gizmosColor = Color.red;
                _FOVLight.color = Color.red;
                return;
            }
        }

        _animator.SetBool("SeePlayer", false);
        _gizmosColor = Color.green;
        _FOVLight.color = Color.green;
    }

    public void KillEnemy()
    {
        
    }

    public void ListenForSound(Vector3 soundPosition, float noiseRadius)
    {
        if (_isDead) return;
        
        float distanceToSound = Vector3.Distance(_headTransform.position, soundPosition);
        if (distanceToSound <= noiseRadius)
        {
            AlertEnemy(soundPosition);
        }
    }

    public void AlertEnemy(Vector3 alertPosition)
    {
        NavMeshPath path = new NavMeshPath();
        if (_agent.CalculatePath(alertPosition, path) && path.status == NavMeshPathStatus.PathComplete)
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

    public void RotateTowards(Vector3 targetPosition)
    {
        Vector3 directionToTarget = (targetPosition - _headTransform.position).normalized;
        directionToTarget.y = 0;
        if (directionToTarget != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * enemyParameters.rotationSpeed);
        }
    }
    
    private void OnDrawGizmos()
    {
        Transform origin = headTransform ?? transform;
        Gizmos.color = _gizmosColor;
        Gizmos.DrawWireSphere(origin.position, enemyParameters.detectionRadius);

        Vector3 leftBoundary = Quaternion.AngleAxis(-enemyParameters.detectionAngle / 2f, Vector3.up) * transform.forward;
        Vector3 rightBoundary = Quaternion.AngleAxis(enemyParameters.detectionAngle / 2f, Vector3.up) * transform.forward;

        Gizmos.DrawRay(origin.position, leftBoundary * enemyParameters.detectionRadius);
        Gizmos.DrawRay(origin.position, rightBoundary * enemyParameters.detectionRadius);
    }
}
