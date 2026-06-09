using System;
using UnityEngine;
using _Code.Scripts.CheckPoint;
using _Code.Scripts.Gameplay;

namespace _Code.Scripts.UI
{
    public class PauseMenu : MonoBehaviour
    {
        public static PauseMenu Instance;
        private GameObject _childObject;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            _childObject = transform.GetChild(0).gameObject;
            if (GameManager.Instance == null)
            {
                Debug.LogWarning("GameManager instance is null, start from mainMenu scene or add a gameManager to your scene");
                return;
            }
            GameManager.Instance.RegisterPauseMenu(_childObject);
        }

        public void ResumeGame()
        {
            Debug.Log("PauseMenu.ResumeGame() called");
            GameManager.Instance?.ResumeGame();
            _childObject.SetActive(false);
        }

        public void ReturnToMainMenu()
        {
            Debug.Log("PauseMenu.ReturnToMainMenu() called");
            GameManager.Instance.ReturnToMenu();
            _childObject.SetActive(false);
        }
    }
}