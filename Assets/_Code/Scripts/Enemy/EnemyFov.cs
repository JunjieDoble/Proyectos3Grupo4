using UnityEngine;

public class EnemyFov : MonoBehaviour
{
    private int _layerMask;
    private float _floorDetectionTime = 0.2f;
    private float _timer = 0f;

    private void Start()
    {
        _layerMask = ~LayerMask.GetMask("Enemy");
    }


    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _floorDetectionTime)
        {
            _timer = 0f;
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.forward + Vector3.down * 0.5f, out hit, 10f, _layerMask)) //transform.forward és la línia que creua les dues bases del fov object
            {
                var distanceToGround = hit.distance;
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, distanceToGround/2f);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyBehaviour enemyBehaviour = GetComponentInParent<EnemyBehaviour>();
            if (enemyBehaviour != null)
            {
                enemyBehaviour.SetPlayerInFOV();
            }
        }
    }
}
