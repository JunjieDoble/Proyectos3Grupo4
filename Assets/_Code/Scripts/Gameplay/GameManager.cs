using System;
using _Code.Scripts.Character;
using _Code.Scripts.CheckPoint;
using UnityEngine;
using UnityEngine.SceneManagement;
using _Code.Scripts.UI;

namespace _Code.Scripts.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        
        public static Action OnPlayerRespawn;
        public static Action OnPause;
        public static Action OnResume;
        
        [SerializeField] 
        private GameplayParameters parameters;
        public static GameManager Instance;

        private GameObject _deathMenu;
        private GameObject _pauseMenu;

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
            SceneManager.LoadScene("TutorialFix", LoadSceneMode.Additive);
            SceneManager.LoadScene("BigRoom", LoadSceneMode.Additive);
            SceneManager.LoadScene("Terrain", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("LevelPrototype2LW", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("KatsuLevelFinal", LoadSceneMode.Additive);
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

        public void RegisterPauseMenu(GameObject pauseMenu)
        {
            _pauseMenu = pauseMenu;
        }

        private void ShowDeathMenu()
        {
            _deathMenu?.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        public void ShowPauseMenu()
        {
            OnPause?.Invoke();
            _pauseMenu?.SetActive(true);
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        public void ResumeGame()
        {
            OnResume?.Invoke();
            _deathMenu?.SetActive(false);
            _pauseMenu?.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void RespawnPlayer()
        {
            OnPlayerRespawn?.Invoke();
            Checkpoint.RespawnPlayer();
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}