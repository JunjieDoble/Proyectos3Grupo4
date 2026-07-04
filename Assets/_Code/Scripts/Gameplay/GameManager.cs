using System;
using _Code.Scripts.Character;
using _Code.Scripts.CheckPoint;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Code.Scripts.Gameplay
{
    public class GameManager : MonoBehaviour
    {
        
        public static Action OnPlayerRespawn;
        public static Action OnPause;
        public static Action OnResume;
        
        [SerializeField] 
        private GameplayParameters parameters;

        [SerializeField] private bool useCheckpoints;
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
            //SceneManager.LoadScene("TutorialFix", LoadSceneMode.Single);
            SceneManager.LoadScene("KatsuLevelFinal", LoadSceneMode.Single);
            SceneManager.LoadSceneAsync("Terrain", LoadSceneMode.Additive);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void LoadOtherScenes()
        {
            SceneManager.UnloadSceneAsync("Terrain");
            SceneManager.LoadSceneAsync("BigRoom", LoadSceneMode.Additive);
            //SceneManager.LoadSceneAsync("LevelPrototype2LW", LoadSceneMode.Additive);
            SceneManager.LoadSceneAsync("KatsuLevelFinal", LoadSceneMode.Additive);
        }

        public void ReturnToMenu()
        {
            HideDeathMenu();
            HidePauseMenu();
            SceneManager.LoadScene("MainMenu");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
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

        private void HideDeathMenu()
        {
            _deathMenu?.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void HidePauseMenu()
        {
            _pauseMenu?.SetActive(false);
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            if (useCheckpoints)
            {
                OnPlayerRespawn?.Invoke();
                Checkpoint.RespawnPlayer();
            }
            else
            {
                StartGame();
            }
        }
    }
}