using UnityEngine;
using _Code.Scripts.CheckPoint;
using _Code.Scripts.Gameplay;

namespace _Code.Scripts.UI
{
    public class DeathMenu : MonoBehaviour
    {
        private GameObject _childObject;

        private void Start()
        {
            _childObject = transform.GetChild(0).gameObject;
            if (GameManager.Instance == null)
            {
                Debug.LogWarning("GameManager instance is null, start from mainMenu scene or add a gameManager to your scene");
                return;
            }
            GameManager.Instance.RegisterDeathMenu(_childObject);
        }

        public void Respawn()
        {
            Checkpoint.RespawnPlayer();
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            _childObject.SetActive(false);
        }

        public void ReturnToMenu()
        {
            GameManager.Instance.ReturnToMenu();
            _childObject.SetActive(false);
        }
    }
}