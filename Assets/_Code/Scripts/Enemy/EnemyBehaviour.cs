using _Code.Scripts.Character;
using _Code.Scripts.Enemy;
using Interactions;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour, IEnemy, IInteractable
{
    private static readonly int SeePlayer = Animator.StringToHash("SeePlayer");
    private static readonly int ToPlayer = Animator.StringToHash("DistanceToPlayer");
    private static readonly int Alert = Animator.StringToHash("Alert");
    private static readonly int PlayerDead = Animator.StringToHash("PlayerDead");
    private static readonly int Speed = Animator.StringToHash("Speed");

    [Header("Enemy Parameters")]
    [SerializeField] private EnemyParameters enemyParameters;
    [SerializeField] private Transform headTransform;
    
    [Header("Patrol Settings")]
    [SerializeField] private GameObject[] patrolPoints;
    
    private Animator _animator;
    private Player _player;
    private EnemyInteractor _enemyInteractor;
    private bool _isDead;
    private Color _gizmosColor = Color.green;
    private Vector3 _lastAlertPosition;
    private Vector3 _lastPlayerPosition;
    private float _idleTime;
    
    private GameObject _deathZone;
    private Transform _headTransform;
    private Light _fovLight;
    private NavMeshAgent _agent;
    
    public GameObject drop;

    private void OnEnable() => Player.OnPlayerDied += PlayerDied;
    private void OnDisable() => Player.OnPlayerDied -= PlayerDied;
    private void PlayerDied() => _animator.SetBool(PlayerDead, true);
    
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
        _enemyInteractor = GetComponent<EnemyInteractor>();
        _deathZone = transform.Find("DeathZone")?.gameObject;
        if (_deathZone == null) Debug.LogWarning("Enemy doesnt have a DeathZone child");
        _deathZone?.SetActive(false);
        _headTransform = headTransform ?? transform;
        _idleTime = enemyParameters.idleTime;
        _fovLight = GetComponentInChildren<Light>();
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_isDead) return;
        DistanceAndVisionToPlayer();
        HearPlayer();
        _animator?.SetFloat(Speed, _agent?.velocity.magnitude ?? 0);
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
        
        _animator.SetFloat(ToPlayer, DistanceToPlayer());

        Vector3 directionToPlayer = (_player.transform.position - _headTransform.position).normalized;
        float angleToPlayer = Vector3.Angle(_headTransform.forward, directionToPlayer);

        if (DistanceToPlayer() <= enemyParameters.detectionRadius && angleToPlayer <= enemyParameters.detectionAngle / 2f)
        {
            if (!Physics.Linecast(_headTransform.position, _player.transform.position, enemyParameters.obstacleMask))
            {
                _animator.SetBool(SeePlayer, true);
                SetLastPlayerPosition(_player.transform.position);
                _gizmosColor = Color.red;
                _fovLight.color = Color.red;
                return;
            }
        }

        _animator.SetBool(SeePlayer, false);
        _gizmosColor = Color.green;
        _fovLight.color = Color.green;
    }
    private void HearPlayer()
    {
        if (!PlayerAvailable()) return;
        MovementController playerMovement = _player.GetComponent<MovementController>();
        ListenForSound(_player.transform.position, playerMovement.GetCurrentNoiseRadius());
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
        _lastAlertPosition = alertPosition;
        _animator.SetBool(Alert, true);
    }
    
    public void InteractWithInteractable(InteractPoint interactPoint)
    {
        IInteractable interactable = interactPoint.GetInteractable();
        _enemyInteractor.AssignInteractable(interactable);
        _enemyInteractor.OnInteract();
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
        Gizmos.DrawRay(headTransform.position, headTransform.forward);
    }
}
