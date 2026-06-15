using _Code.Scripts.Activators;
using UnityEngine;

namespace _Code.Scripts.Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class OtherSceneLoader : MonoBehaviour
    {

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GameManager.Instance?.LoadOtherScenes();
                Terminal.DeactivateAll();
                Destroy(gameObject);
            }
        }
        
    }
}
