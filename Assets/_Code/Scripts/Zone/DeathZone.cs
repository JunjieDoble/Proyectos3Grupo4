using _Code.Scripts.Character;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<IDie>() != null)
        {
            other.GetComponent<IDie>().Die();
        }
        else if (other.CompareTag("Enemy"))
        {
            other.GetComponent<IEnemy>().KillEnemy();
        }
    }
}
