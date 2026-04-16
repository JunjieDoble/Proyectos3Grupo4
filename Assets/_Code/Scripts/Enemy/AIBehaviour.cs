using System.Collections;
using UnityEngine;

public class AIBehaviour : MonoBehaviour
{
    public Transform[] patrolPoints;

    [Header("Stats")]
    [SerializeField] private float detectionRadius = 10f;

    private Vector3 _startPosition;
    private Quaternion _startRotation;

    private Animator _stateAnimationController;
    private GameObject _player;
    private bool _isDead = false;

    private void Start()
    {
        _startPosition = transform.root.position;
        _startRotation = transform.root.rotation;

        _player = GameObject.Find("Player");
        _stateAnimationController = GetComponent<Animator>();
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

        _stateAnimationController.SetFloat("hDistance", HorizontalDistance);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            // Player dead or something
        }
    }

    public void EnemyDead()
    {
        if (_isDead) return;
        _isDead = true;

        _stateAnimationController.SetTrigger("die");

        StartCoroutine(DestroyAfterDeath());
    }

    private IEnumerator DestroyAfterDeath()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(transform.parent.gameObject);
    }
}
