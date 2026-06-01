using UnityEngine;
using _Code.Scripts.Gameplay;
using UnityEngine.SceneManagement;

namespace _Code.Scripts.UI
{
    public class MainMenu : MonoBehaviour
    {

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
            Debug.Log("Not implemented yet");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}