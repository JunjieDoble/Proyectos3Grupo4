using _Code.Scripts.Character;
using UnityEngine;
using UnityEngine.SceneManagement;
using _Code.Scripts.UI;

namespace _Code.Scripts.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] 
        private GameplayParameters parameters;
        public static GameManager Instance;

        private GameObject _deathMenu;

        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            Player.OnPlayerDied += ShowDeathMenu;
        }

        private void OnDisable()
        {
            Player.OnPlayerDied -= ShowDeathMenu;
        }

        public void StartGame()
        {
            SceneManager.LoadScene(parameters?.scenes[0] ?? "DefaultScene");
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void ReturnToMenu()
        {
            _deathMenu = null;
            SceneManager.LoadScene("MainMenu");
        }

        public void RegisterDeathMenu(GameObject deathMenu)
        {
            _deathMenu = deathMenu;
        }

        private void ShowDeathMenu()
        {
            _deathMenu?.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}