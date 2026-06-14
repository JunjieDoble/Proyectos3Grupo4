using UnityEngine;
using _Code.Scripts.Gameplay;
using UnityEngine.SceneManagement;

namespace _Code.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject settingPanel;
        [SerializeField] private GameObject creditsPanel;

        void Start()
        {
            SceneManager.LoadScene("GameManager", LoadSceneMode.Additive);
        }
        
        public void StartGame()
        {
            GameManager.Instance.StartGame();
        }

        public void Settings()
        {
            mainMenuPanel.SetActive(false);
            settingPanel.SetActive(true);
        }

        public void Credits()
        {
            mainMenuPanel.SetActive(false);
            creditsPanel.SetActive(true);
        }

        public void QuitGame()
        {
            Application.Quit();
        }

        public void BackToMainMenu()
        {
            mainMenuPanel.SetActive(true);

            settingPanel.SetActive(false);
            creditsPanel.SetActive(false);
        }
    }
}